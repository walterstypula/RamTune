using RamTune.Core.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace RamTune.Core
{
    public enum ComparisonStatus
    {
        Identical,
        Different,
        SizeMismatch,
        OnlyLeft,
        OnlyRight,
    }

    public class TableComparison
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public Table Left { get; set; }

        public Table Right { get; set; }

        public ComparisonStatus Status { get; set; }
    }

    /// <summary>
    /// Matches the tables of two loaded ROMs by name and compares their data and axes.
    /// </summary>
    public static class RomComparer
    {
        public static List<TableComparison> Compare(IEnumerable<Table> leftTables, ITableReader left, IEnumerable<Table> rightTables, ITableReader right)
        {
            var leftByName = ByName(leftTables);
            var rightByName = ByName(rightTables);
            var names = leftByName.Keys.Union(rightByName.Keys);

            var result = new List<TableComparison>();

            foreach (var name in names)
            {
                leftByName.TryGetValue(name, out var leftTable);
                rightByName.TryGetValue(name, out var rightTable);

                var comparison = new TableComparison
                {
                    Name = name,
                    Category = (leftTable ?? rightTable).Category,
                    Left = leftTable,
                    Right = rightTable,
                };

                if (leftTable == null)
                {
                    comparison.Status = ComparisonStatus.OnlyRight;
                }
                else if (rightTable == null)
                {
                    comparison.Status = ComparisonStatus.OnlyLeft;
                }
                else
                {
                    comparison.Status = CompareTable(leftTable, left, rightTable, right);
                }

                result.Add(comparison);
            }

            return result.OrderBy(c => c.Category).ThenBy(c => c.Name).ToList();
        }

        public static ComparisonStatus CompareTable(Table leftTable, ITableReader left, Table rightTable, ITableReader right)
        {
            var leftParts = ReadParts(leftTable, left);
            var rightParts = ReadParts(rightTable, right);

            if (leftParts.Count != rightParts.Count || leftParts.Zip(rightParts, (l, r) => l.Values.Count != r.Values.Count).Any(d => d))
            {
                return ComparisonStatus.SizeMismatch;
            }

            var same = leftParts.Zip(rightParts, (l, r) => SameValues(l, r)).All(s => s);

            return same ? ComparisonStatus.Identical : ComparisonStatus.Different;
        }

        private static Dictionary<string, Table> ByName(IEnumerable<Table> tables)
        {
            var byName = new Dictionary<string, Table>();

            foreach (var table in tables ?? Enumerable.Empty<Table>())
            {
                if (table.Name != null && !byName.ContainsKey(table.Name))
                {
                    byName.Add(table.Name, table);
                }
            }

            return byName;
        }

        private class Part
        {
            public Scaling Scaling { get; set; }

            public bool IsStatic { get; set; }

            public List<byte[]> Values { get; set; }
        }

        /// <summary>
        /// Data first, then each axis, flattened. Element counts follow the same rules the table view uses.
        /// </summary>
        private static List<Part> ReadParts(Table table, ITableReader reader)
        {
            var xAxis = table.GetAxis(AxisType.XAxis, AxisType.StaticXAxis);
            var yAxis = table.GetAxis(AxisType.YAxis, AxisType.StaticYAxis);

            var columns = xAxis?.Elements;
            var rows = yAxis?.Elements;

            if (xAxis == null && yAxis != null)
            {
                columns = rows;
                rows = 1;
            }

            var parts = new List<Part>
            {
                new Part
                {
                    Scaling = table.Scaling,
                    Values = reader.LoadTableData(table, columns, rows).SelectMany(r => r).ToList(),
                }
            };

            foreach (var axis in new[] { xAxis, yAxis }.Where(a => a != null))
            {
                parts.Add(new Part
                {
                    Scaling = axis.Scaling,
                    IsStatic = axis.IsStaticAxis(),
                    Values = reader.LoadAxisData(axis).SelectMany(r => r).ToList(),
                });
            }

            return parts;
        }

        private static bool SameValues(Part left, Part right)
        {
            if (left.IsStatic || right.IsStatic || left.Scaling.IsCompatibleWith(right.Scaling))
            {
                return left.Values.Zip(right.Values, (l, r) => l.SequenceEqual(r)).All(s => s);
            }

            return left.Values.Zip(right.Values, (l, r) => left.Scaling.GetDisplayValue(l) == right.Scaling.GetDisplayValue(r)).All(s => s);
        }
    }

    public static class ScalingCompatibilityExtensions
    {
        /// <summary>
        /// True when raw bytes mean the same value in both scalings, so values can be compared and copied byte for byte.
        /// Endianness doesn't matter here: element bytes are already in host order once read.
        /// </summary>
        public static bool IsCompatibleWith(this Scaling scaling, Scaling other)
        {
            if (scaling == null || other == null)
            {
                return scaling == other;
            }

            return scaling.StorageType == other.StorageType
                && scaling.ToExpr == other.ToExpr
                && scaling.FrExpr == other.FrExpr;
        }
    }
}
