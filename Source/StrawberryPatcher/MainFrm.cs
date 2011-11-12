using System;
using System.Windows.Forms;
using System.IO;

namespace Strawberry_Patcher
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }
        string wowPath, wowFile;
        bool rightWowFile;

        private void MainFrm_Load(object sender, EventArgs e)
        {
            rightWowFile = false;
            patchStatusBox.Text = "Please open your WoW Executable.";
        }

        private void openWowBtn_Click(object sender, EventArgs e)
        {
            uint clientVersion = 14480;
            try
            {
                if (!rightWowFile)
                {
                    OpenFileDialog openWow = new OpenFileDialog();
                    openWow.Filter = "Executable (*.exe)|*.exe";
                    openWow.ShowDialog();

                    // Set wowFile to selected file.
                    wowPath = openWow.FileName;
                    wowFile = openWow.SafeFileName;

                    if (!wowPath.Equals(""))
                    {
                        patchStatusBox.Text = "You have " + wowFile + " selected as your WoW Executable.";

                        BinaryReader wowReader = new BinaryReader(File.Open(wowPath, FileMode.Open, FileAccess.ReadWrite));
                        wowReader.BaseStream.Seek((int)Offsets.versionOffset, SeekOrigin.Begin);

                        if (wowReader.ReadByte() == 0x90)
                        {
                            patchWowBtn.Enabled = true;
                            rightWowFile = true;
                            patchWowBtn.ForeColor = System.Drawing.Color.Green;
                            openWowBtn.Enabled = false;
                        }
                        else
                            patchStatusBox.Text += "\nPlease select a Wow.exe with client build " + clientVersion;

                        wowReader.Close();
                    }
                    else
                        rightWowFile = false;
                }

                if (rightWowFile)
                    patchStatusBox.Text += "\nReady to patch...";
            }
            catch (EndOfStreamException)
            {
                MessageBox.Show("What are you doing?! Select a valid Wow.exe!!!");
            }
            
        }

        private void patchWowBtn_Click(object sender, EventArgs e)
        {
            byte[] patchedBytes = { 0xB8, 0x00, 0x00, 0x00, 0x00, 0x90, 0xE9, 0xA9, 0x00, 0x00, 0x00, 0x90 };    // unpatched: { 0x8B, 0x45, 0x0C, 0x83, 0xF8 };
            byte[] patchedBytes2 = { 0xEB, 0x28 };    // unpatched: { 0x74, 0x28 };

            BinaryReader wowReader = new BinaryReader(File.Open(wowPath, FileMode.Open, FileAccess.ReadWrite));
            wowReader.BaseStream.Seek((int)Offsets.Send2Offset, SeekOrigin.Begin);

            // Check if the selected Wow.exe already patched.
            if (wowReader.ReadByte() == 0xB8)
            {
                wowReader.BaseStream.Seek((int)Offsets.CommsHandlerOffset, SeekOrigin.Begin);

                if (wowReader.ReadByte() == 0xEB)
                {
                    rightWowFile = false;
                    MessageBox.Show(wowFile + " already patched!!!");

                    wowReader.Close();
                    patchWowBtn.ForeColor = System.Drawing.Color.Red;
                    openWowBtn.Enabled = true;
                }
                wowReader.Close();
            }
            else
            {
                wowReader.Close();
                patchStatusBox.Text = "Backup " + wowFile + "...";
                // Backup unpatched Wow.exe
                File.Copy(wowPath, wowPath + ".bak", true);

                patchStatusBox.Text += "\n" + wowFile + " backed up to\n" + wowPath + ".bak";

                // Start BinaryWriter.
                BinaryWriter wowWriter = new BinaryWriter(File.Open(wowPath, FileMode.Open, FileAccess.ReadWrite));

                patchStatusBox.Text += "\n\nPatching...";

                // Patch NetClient::Send2
                wowWriter.BaseStream.Seek((int)Offsets.Send2Offset, SeekOrigin.Begin);
                wowWriter.Write(patchedBytes);

                // Patch NetClient::ClientConnectionResumeCommsHandler
                wowWriter.BaseStream.Seek((int)Offsets.CommsHandlerOffset, SeekOrigin.Begin);
                wowWriter.Write(patchedBytes2);

                wowWriter.Close();

                patchStatusBox.Text += "\n\n" + wowFile + "\nsucessfully patched";
                patchWowBtn.ForeColor = System.Drawing.Color.Red;

                rightWowFile = false;
            }
        }
    }
}
