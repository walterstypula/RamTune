using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    public class DefinitionLoader
    {
        public IReadOnlyList<Definition> Definitions { get; set; }

        private IReadOnlyDictionary<string, Scaling> ScalingMetadata { get; set; }

        public Definition GetDefinitionByInternalId(string interalId)
        {
            var ecuMetaData = this.Definitions.First(p => p.RomId.InternalIdString == interalId);
            List<string> inheritiedRomIds = new List<string>();
            //Remove any tables the address is not defined.
            var loadedTable = LoadBaseTables(interalId, inheritiedRomIds);
            var tables = loadedTable
                            .Where(t => t.Address != null)
                            .ToList();

            LoadTableScaling(tables, inheritiedRomIds);

            ecuMetaData.Tables = tables;

            return ecuMetaData;
        }

        public List<Table> LoadBaseTables(string baseRomXmlId, List<string> inheritedRomIds)
        {
            if (string.IsNullOrEmpty(baseRomXmlId))
            {
                //No base table to load.
                return null;
            }

            var rom = Definitions.First(d => d.RomId.XmlId == baseRomXmlId);
            var tables = Clone(rom.Tables);

            inheritedRomIds.Add(baseRomXmlId);
            List<Table> output = LoadBaseTables(rom.Base, inheritedRomIds);

            if (output == null)
            {
                //No base tables to merge return tables.
                return tables;
            }

            foreach (var sourceTable in tables)
            {
                var targetTable = output.FirstOrDefault(t => t.Name == sourceTable.Name);

                if (targetTable == null)
                {
                    //table doesn't exist in base definition. Assuming its a new definition.
                    output.Add(sourceTable);
                    continue;
                }

                CopyValues(targetTable, sourceTable);
                RecursiveLoadAxisMetaData(targetTable, sourceTable);
            }

            return output;
        }

        public void LoadDefinitions(IEnumerable<string> definitionPaths)
        {
            var scalings = new Dictionary<string, Scaling>();
            var definitions = new List<Definition>();

            foreach (var path in definitionPaths)
            {
                var romScalings = LoadDefinition<DefinitionScalings>(path);
                foreach (var scaling in romScalings?.Scalings ?? Enumerable.Empty<Scaling>())
                {
                    var scalingName = $"{romScalings.RomId.XmlId}{scaling.Name}";

                    if (scalings.ContainsKey(scalingName))
                    {
                        continue;
                    }

                    scalings.Add(scalingName, scaling);
                }

                var rom = LoadDefinition<Definition>(path);

                if (rom == null)
                {
                    continue;;
                }

                 //KNOWN ISSUE
                definitions.Add(rom);
            }

            Definitions = definitions;
            ScalingMetadata = scalings;
        }

        public void LoadTableScaling(List<Table> tables, List<string> inheritedRomIds)
        {
            for (int i = 0; i < tables.Count; i++)
            {
                var table = tables[i];

                Scaling scaling = GetScaling(inheritedRomIds, table.ScalingName);

                if (scaling == null)
                {
                    tables.Remove(table);
                    i--;
                    continue;
                }

                table.Scaling = scaling;

                foreach (var axis in table.Axis)
                {
                    var scalingName = axis.ScalingName != null ? axis.ScalingName : table.ScalingName;
                    var axisScaling = GetScaling(inheritedRomIds, scalingName);

                    axis.Scaling = axisScaling;
                }
            }
        }

        private Scaling GetScaling(List<string> inheritedRomIds, string scalingName)
        {
            foreach (var baseRomId in inheritedRomIds)
            {
                var combinedScalingName = $"{baseRomId}{scalingName}";

                if (ScalingMetadata.TryGetValue(combinedScalingName, out var scaling))
                {
                    return scaling;
                }
            }

            return null;
        }

        private T Clone<T>(T source)
        {
            var json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<T>(json);
        }

        private void CopyValues<T>(T target, T source)
        {
            if (source == null)
                return;

            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                if (prop.PropertyType.GetInterfaces().Contains(typeof(IList)))
                {
                    continue;
                }

                var sourceValue = prop.GetValue(source, null);
                var defaultValue = prop.PropertyType.UnderlyingSystemType.GetDefault();
                if (!Equals(sourceValue, defaultValue))
                {
                    //var currentTargetValue = prop.GetValue(target, null);
                    //if (!Equals(sourceValue, defaultValue))
                    //{
                    //    continue;
                    //}

                    prop.SetValue(target, sourceValue, null);
                }
            }
        }

        private T LoadDefinition<T>(string path)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StreamReader(path))
                {
                    return (T) xmlSerializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                //TODO: log to output
                return default(T);
            }
        }

        private void RecursiveLoadAxisMetaData(Table target, Table source)
        {
            foreach (var targetAxis in target.Axis)
            {
                var sourceAxis = source?.Axis?.FirstOrDefault(a =>
                {
                    if (a.Name == "Y")
                    {
                        a.Type = targetAxis.Type;
                        a.Name = targetAxis.Name;
                    }
                    else if (a.Name == "X")
                    {
                        a.Type = targetAxis.Type;
                        a.Name = targetAxis.Name;
                    }

                    return a.Name == targetAxis.Name || a.Type == targetAxis.Type;
                });
                CopyValues(targetAxis, sourceAxis);
            }
        }
    }
}