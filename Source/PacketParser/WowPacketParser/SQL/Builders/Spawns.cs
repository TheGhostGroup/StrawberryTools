﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Store.Objects;
using Guid = WowPacketParser.Misc.Guid;

namespace WowPacketParser.SQL.Builders
{
    public static class Spawns
    {
        public static string Creature(Dictionary<Guid, Unit> units)
        {
            if (units.Count == 0)
                return string.Empty;

            const string tableName = "creature";

            uint count = 0;
            var rows = new List<QueryBuilder.SQLInsertRow>();
            foreach (var unit in units)
            {
                var row = new QueryBuilder.SQLInsertRow();

                var creature = unit.Value;

                if (Settings.AreaFilters.Length > 0)
                    if (!(creature.Area.ToString(CultureInfo.InvariantCulture).MatchesFilters(Settings.AreaFilters)))
                        continue;

                var spawnTimeSecs = creature.GetDefaultSpawnTime();
                var movementType = 0; // TODO: Find a way to check if our unit got random movement
                var spawnDist = (movementType == 1) ? 5 : 0;

                row.AddValue("id", unit.Key.GetEntry());
                row.AddValue("map", creature.Map);
                row.AddValue("spawnMask", 1);
                row.AddValue("phaseMask", creature.PhaseMask);
                row.AddValue("modelid", creature.Model);
                row.AddValue("position_x", creature.Movement.Position.X);
                row.AddValue("position_y", creature.Movement.Position.Y);
                row.AddValue("position_z", creature.Movement.Position.Z);
                row.AddValue("orientation", creature.Movement.Orientation);
                row.AddValue("curhealth", creature.MaxHealth);
                row.AddValue("spawntimesecs", spawnTimeSecs);
                row.AddValue("spawndist", spawnDist);
                row.AddValue("MovementType", movementType);
                row.Comment = StoreGetters.GetName(StoreNameType.Unit, (int)unit.Key.GetEntry(), false);
                row.Comment += " (Area: " + StoreGetters.GetName(StoreNameType.Area, creature.Area, false) + ")";

                if (creature.IsTemporarySpawn())
                {
                    row.CommentOut = true;
                    row.Comment += " - !!! might be temporary spawn !!!";
                }
                else
                    ++count;

                rows.Add(row);
            }

            var result = new StringBuilder();
            // delete query for GUIDs
            //var delete = new QueryBuilder.SQLDelete(Tuple.Create("@CGUID+0", "@CGUID+" + --count), "guid", tableName);
            //result.Append(delete.Build());

            var sql = new QueryBuilder.SQLInsert(tableName, rows, withDelete: false);
            result.Append(sql.Build());
            return result.ToString();
        }

        public static string GameObject(Dictionary<Guid, GameObject> gameObjects)
        {
            if (gameObjects.Count == 0)
                return string.Empty;

            const string tableName = "gameobject";

            uint count = 0;
            var rows = new List<QueryBuilder.SQLInsertRow>();
            foreach (var gameobject in gameObjects)
            {
                var row = new QueryBuilder.SQLInsertRow();

                var go = gameobject.Value;

                if (Settings.AreaFilters.Length > 0)
                    if (!(go.Area.ToString(CultureInfo.InvariantCulture).MatchesFilters(Settings.AreaFilters)))
                        continue;

                uint animprogress = 0;
                uint state = 0;
                UpdateField uf;
                if (go.UpdateFields.TryGetValue(UpdateFields.GetUpdateField(GameObjectField.GAMEOBJECT_BYTES_1), out uf))
                {
                    var bytes = uf.UInt32Value;
                    state = (bytes & 0x000000FF);
                    animprogress = Convert.ToUInt32((bytes & 0xFF000000) >> 24);
                }

                var spawnTimeSecs = go.GetDefaultSpawnTime();

                if (gameobject.Key.GetEntry() != 0)
                {
                    row.AddValue("id", gameobject.Key.GetEntry());
                    row.AddValue("map", go.Map);
                    row.AddValue("spawnMask", 1);
                    row.AddValue("phaseMask", go.PhaseMask);
                    row.AddValue("position_x", go.Movement.Position.X);
                    row.AddValue("position_y", go.Movement.Position.Y);
                    row.AddValue("position_z", go.Movement.Position.Z);
                    row.AddValue("orientation", go.Movement.Orientation);
                    row.AddValue("rotation0", go.Movement.Rotation.X);
                    row.AddValue("rotation1", go.Movement.Rotation.Y);
                    row.AddValue("rotation2", go.Movement.Rotation.Z);
                    row.AddValue("rotation3", go.Movement.Rotation.W);
                    row.AddValue("spawntimesecs", spawnTimeSecs);
                    row.AddValue("animprogress", animprogress);
                    row.AddValue("state", state);
                    row.Comment = StoreGetters.GetName(StoreNameType.GameObject, (int)gameobject.Key.GetEntry(), false);
                    row.Comment += " (Area: " + StoreGetters.GetName(StoreNameType.Area, go.Area, false) + ")";

                    if (go.IsTemporarySpawn())
                    {
                        row.CommentOut = true;
                        row.Comment += " - !!! might be temporary spawn !!!";
                    }
                    else
                        ++count;

                    rows.Add(row);
                }
            }

            var result = new StringBuilder();

            var sql = new QueryBuilder.SQLInsert(tableName, rows, withDelete: false);
            result.Append(sql.Build());
            return result.ToString();
        }
    }
}
