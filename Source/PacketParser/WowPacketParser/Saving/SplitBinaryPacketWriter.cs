﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;

namespace WowPacketParser.Saving
{
    public class FileLock<T>
    {
        private const int Timeout = 3000;
        private static readonly Dictionary<T, References> Locks = new Dictionary<T, References>();

        public IDisposable Lock(T fileName)
        {
            Monitor.Enter(Locks);
            References obj;
            if (Locks.TryGetValue(fileName, out obj))
            {
                obj.Addquire();
                Monitor.Exit(Locks);
                if (!Monitor.TryEnter(obj, Timeout))
                    throw new TimeoutException(String.Format("{0}", fileName));
            }
            else
            {
                obj = new References();
                Monitor.Enter(obj);
                Locks.Add(fileName, obj);
                Monitor.Exit(Locks);
            }

            return new Locker<T>(fileName);
        }

        private static void Unlock(T fileName)
        {
            lock (Locks)
            {
                References obj;
                if (Locks.TryGetValue(fileName, out obj))
                {
                    Monitor.Exit(obj);
                    if (0 == obj.Release())
                        Locks.Remove(fileName);
                }
            }
        }

        private class References
        {
            private int _count = 1;
            public void Addquire()
            {
                Interlocked.Increment(ref _count);
            }

            public int Release()
            {
                return Interlocked.Decrement(ref _count);
            }
        }

        class Locker<T2> : IDisposable
        {
            private readonly T2 _fileName;

            public Locker(T2 fileName)
            {
                _fileName = fileName;
            }

            public void Dispose()
            {
                FileLock<T2>.Unlock(_fileName);
            }
        }
    }

    public static class SplitBinaryPacketWriter
    {
        private static readonly FileLock<string> Locks = new FileLock<string>();
        private const string Folder = "split"; // might want to move to config later

        [SuppressMessage("Microsoft.Reliability", "CA2000", Justification = "fileStream is disposed when writer is disposed.")]
        public static void Write(IEnumerable<Packet> packets, Encoding encoding)
        {
            Directory.CreateDirectory(Folder); // not doing anything if it exists already

            foreach (var packet in packets)
            {
                var fileName = Folder + "/" + Opcodes.GetOpcodeName(packet.Opcode) + "." + Settings.DumpFormat.ToString().ToLower();
                try
                {
                    using (Locks.Lock(fileName))
                    {
                        var fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None);
                        using (var writer = new BinaryWriter(fileStream, encoding))
                        {
                            if (Settings.DumpFormat != DumpFormatType.Pkt)
                            {
                                writer.Write((ushort) packet.Opcode);
                                writer.Write((int) packet.Length);
                                writer.Write((byte) packet.Direction);
                                writer.Write((ulong) Utilities.GetUnixTimeFromDateTime(packet.Time));
                                writer.Write(packet.GetStream(0));
                            }
                            else
                            {
                                writer.Write(packet.Opcode);
                                writer.Write((int) packet.Length);
                                writer.Write((int) Utilities.GetUnixTimeFromDateTime(packet.Time));
                                writer.Write((byte) packet.Direction);
                                writer.Write(packet.GetStream(0));
                            }
                        }
                    }
                }
                catch(TimeoutException)
                {
                    Trace.WriteLine(string.Format("Timeout trying to write Opcode to {0} ignoring opcode", fileName));
                }
            }
        }
    }
}
