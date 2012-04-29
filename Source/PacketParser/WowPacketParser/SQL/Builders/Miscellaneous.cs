using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Store;

namespace WowPacketParser.SQL.Builders
{
    public static class Miscellaneous
    {
        public static string StartInformation()
        {
            var result = String.Empty;

            if (!Storage.StartActions.IsEmpty)
            {
                var rows = new List<QueryBuilder.SQLInsertRow>();
                foreach (var startActions in Storage.StartActions)
                {
                    var comment = new QueryBuilder.SQLInsertRow();
                    comment.HeaderComment = startActions.Key.Item1 + " - " + startActions.Key.Item2;
                    rows.Add(comment);

                    foreach (var action in startActions.Value.Actions)
                    {
                        var row = new QueryBuilder.SQLInsertRow();

                        row.AddValue("race", startActions.Key.Item1);
                        row.AddValue("class", startActions.Key.Item2);
                        row.AddValue("button", action.Button);
                        row.AddValue("action", action.Id);
                        row.AddValue("type", action.Type);
                        if (action.Type == ActionButtonType.Spell)
                            row.Comment = StoreGetters.GetName(StoreNameType.Spell, (int)action.Id, false);
                        if (action.Type == ActionButtonType.Item)
                            row.Comment = StoreGetters.GetName(StoreNameType.Item, (int)action.Id, false);

                        rows.Add(row);
                    }
                }

                result = new QueryBuilder.SQLInsert("playercreateinfo_action", rows, 2).Build();
            }

            if (!Storage.StartPositions.IsEmpty)
            {
                var rows = new List<QueryBuilder.SQLInsertRow>();
                foreach (var startPosition in Storage.StartPositions)
                {
                    var comment = new QueryBuilder.SQLInsertRow();
                    comment.HeaderComment = startPosition.Key.Item1 + " - " + startPosition.Key.Item2;
                    rows.Add(comment);

                    var row = new QueryBuilder.SQLInsertRow();

                    row.AddValue("race", startPosition.Key.Item1);
                    row.AddValue("class", startPosition.Key.Item2);
                    row.AddValue("map", startPosition.Value.Map);
                    row.AddValue("zone", startPosition.Value.Zone);
                    row.AddValue("position_x", startPosition.Value.Position.X);
                    row.AddValue("position_y", startPosition.Value.Position.Y);
                    row.AddValue("position_z", startPosition.Value.Position.Z);

                    row.Comment = StoreGetters.GetName(StoreNameType.Map, startPosition.Value.Map, false) + " - " +
                                  StoreGetters.GetName(StoreNameType.Zone, startPosition.Value.Zone, false);

                    rows.Add(row);
                }

                result += new QueryBuilder.SQLInsert("playercreateinfo", rows, 2).Build();
            }

            if (!Storage.StartSpells.IsEmpty)
            {
                var rows = new List<QueryBuilder.SQLInsertRow>();
                foreach (var startSpells in Storage.StartSpells)
                {
                    var comment = new QueryBuilder.SQLInsertRow();
                    comment.HeaderComment = startSpells.Key.Item1 + " - " + startSpells.Key.Item2;
                    rows.Add(comment);

                    foreach (var spell in startSpells.Value.Spells)
                    {
                        var row = new QueryBuilder.SQLInsertRow();

                        row.AddValue("race", startSpells.Key.Item1);
                        row.AddValue("class", startSpells.Key.Item2);
                        row.AddValue("Spell", spell);
                        row.AddValue("Note", StoreGetters.GetName(StoreNameType.Spell, (int)spell, false));

                        rows.Add(row);
                    }
                }

                result = new QueryBuilder.SQLInsert("playercreateinfo_spell", rows, 2).Build();
            }

            return result;
        }
    }
}
