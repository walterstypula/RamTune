using MVVM;
using RamTune.Core;
using RamTune.Core.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    /// <summary>
    /// Side-by-side comparison of two ROMs, with copying of whole tables or selected cells in either direction.
    /// Both ROMs are edited in memory; nothing is written to disk until one side is saved.
    /// </summary>
    public class CompareWindowVM : ViewModelBase
    {
        private readonly DefinitionLoader _definitionLoader;

        private ITableReader _left;
        private ITableReader _right;
        private List<Table> _leftTables;
        private List<Table> _rightTables;
        private List<TableComparisonVM> _comparisons = new List<TableComparisonVM>();

        private RelayCommand _openLeftCommand;
        private RelayCommand _openRightCommand;
        private RelayCommand _saveLeftCommand;
        private RelayCommand _saveRightCommand;
        private RelayCommand _copyTableToRightCommand;
        private RelayCommand _copyTableToLeftCommand;
        private RelayCommand _copySelectedToRightCommand;
        private RelayCommand _copySelectedToLeftCommand;
        private RelayCommand _selectedItemChangedCommand;

        public CompareWindowVM(DefinitionLoader loader)
        {
            _definitionLoader = loader;
            StatusMessage = "Open a ROM on each side to compare them.";
        }

        public string LeftPath
        {
            get { return Get<string>(nameof(LeftPath)); }
            private set { Set(nameof(LeftPath), value); OnPropertyChanged(nameof(LeftTitle)); }
        }

        public string RightPath
        {
            get { return Get<string>(nameof(RightPath)); }
            private set { Set(nameof(RightPath), value); OnPropertyChanged(nameof(RightTitle)); }
        }

        public bool LeftHasChanges
        {
            get { return Get<bool>(nameof(LeftHasChanges)); }
            private set { Set(nameof(LeftHasChanges), value); OnPropertyChanged(nameof(LeftTitle)); }
        }

        public bool RightHasChanges
        {
            get { return Get<bool>(nameof(RightHasChanges)); }
            private set { Set(nameof(RightHasChanges), value); OnPropertyChanged(nameof(RightTitle)); }
        }

        public bool HasUnsavedChanges => LeftHasChanges || RightHasChanges;

        public string LeftTitle => Title(_left, LeftPath, LeftHasChanges, "Left");

        public string RightTitle => Title(_right, RightPath, RightHasChanges, "Right");

        public IEnumerable<CompareGroupVM> Groups
        {
            get { return Get<IEnumerable<CompareGroupVM>>(nameof(Groups)); }
            private set { Set(nameof(Groups), value); }
        }

        public TableComparisonVM Selected
        {
            get { return Get<TableComparisonVM>(nameof(Selected)); }
            private set { Set(nameof(Selected), value); }
        }

        public bool ShowOnlyDifferences
        {
            get { return Get<bool>(nameof(ShowOnlyDifferences)); }
            set { Set(nameof(ShowOnlyDifferences), value); ApplyFilter(); }
        }

        public string FilterText
        {
            get { return Get<string>(nameof(FilterText)); }
            set { Set(nameof(FilterText), value); ApplyFilter(); }
        }

        public string Summary
        {
            get { return Get<string>(nameof(Summary)); }
            private set { Set(nameof(Summary), value); }
        }

        public string StatusMessage
        {
            get { return Get<string>(nameof(StatusMessage)); }
            private set { Set(nameof(StatusMessage), value); }
        }

        public ICommand OpenLeftCommand => this.InitCommand(ref _openLeftCommand, p => OpenRom(right: false), delegate { return true; });

        public ICommand OpenRightCommand => this.InitCommand(ref _openRightCommand, p => OpenRom(right: true), delegate { return true; });

        public ICommand SaveLeftCommand => this.InitCommand(ref _saveLeftCommand, p => SaveRom(right: false), delegate { return _left != null; });

        public ICommand SaveRightCommand => this.InitCommand(ref _saveRightCommand, p => SaveRom(right: true), delegate { return _right != null; });

        public ICommand CopyTableToRightCommand => this.InitCommand(ref _copyTableToRightCommand, p => Copy(toRight: true, selectedOnly: false), delegate { return CanCopy(); });

        public ICommand CopyTableToLeftCommand => this.InitCommand(ref _copyTableToLeftCommand, p => Copy(toRight: false, selectedOnly: false), delegate { return CanCopy(); });

        public ICommand CopySelectedToRightCommand => this.InitCommand(ref _copySelectedToRightCommand, p => Copy(toRight: true, selectedOnly: true), delegate { return CanCopy(); });

        public ICommand CopySelectedToLeftCommand => this.InitCommand(ref _copySelectedToLeftCommand, p => Copy(toRight: false, selectedOnly: true), delegate { return CanCopy(); });

        public ICommand SelectedItemChangedCommand => this.InitCommand(ref _selectedItemChangedCommand, p => SelectTable(p as TableComparisonVM), delegate { return true; });

        /// <summary>
        /// Loads a ROM into one side. Also used directly by tests and command-line callers.
        /// </summary>
        public void LoadRom(string path, bool right)
        {
            var reader = new LoadedRomManager(Common.ReadRomIntoMemory(path), _definitionLoader);

            // The loader shares one Definition object per ROM ID, so keep this side's own table list.
            var tables = reader.Rom.Tables.ToList();

            if (right)
            {
                _right = reader;
                _rightTables = tables;
                RightPath = path;
                RightHasChanges = false;
            }
            else
            {
                _left = reader;
                _leftTables = tables;
                LeftPath = path;
                LeftHasChanges = false;
            }

            BuildComparisons();
        }

        public void SelectTable(TableComparisonVM comparison)
        {
            if (comparison == null)
            {
                return;
            }

            comparison.Refresh();
            Selected = comparison;
            StatusMessage = $"{comparison.Name}: {comparison.StatusText}.";
        }

        /// <summary>
        /// Copies the selected table, or only the cells selected in the source side, from one ROM into the other.
        /// </summary>
        public void Copy(bool toRight, bool selectedOnly)
        {
            var comparison = Selected;
            var source = toRight ? comparison?.LeftTable : comparison?.RightTable;
            var target = toRight ? comparison?.RightTable : comparison?.LeftTable;
            var sourceSide = toRight ? "left" : "right";
            var targetSide = toRight ? "right" : "left";

            if (source == null || target == null)
            {
                StatusMessage = "Select a table that exists in both ROMs first.";
                return;
            }

            if (!source.HasSameShape(target))
            {
                StatusMessage = $"Can't copy '{comparison.Name}': it has a different size in each ROM.";
                return;
            }

            if (selectedOnly && source.SelectedCellCount == 0)
            {
                StatusMessage = $"Select the cells to copy in the {sourceSide} table first.";
                return;
            }

            var changed = target.CopyFrom(source, selectedOnly);

            if (changed == 0)
            {
                StatusMessage = $"Nothing to copy: the {(selectedOnly ? "selected cells already match" : "tables are already identical")}.";
                return;
            }

            if (toRight)
            {
                RightHasChanges = true;
            }
            else
            {
                LeftHasChanges = true;
            }

            comparison.Refresh();
            var shared = ReloadTablesSharingAxes(comparison, toRight);
            UpdateSummary();
            ApplyFilter();

            StatusMessage = $"Copied {changed} cell(s) of '{comparison.Name}' from {sourceSide} to {targetSide}."
                          + (shared > 0 ? $" Its axis is shared with {shared} other table(s), which now use the copied axis too." : string.Empty)
                          + $" Save the {targetSide} ROM to keep it.";
        }

        public string SaveRom(bool right, string path = null)
        {
            var reader = right ? _right : _left;
            var sourcePath = right ? RightPath : LeftPath;

            if (reader == null)
            {
                return null;
            }

            path ??= Common.SaveFile(Common.RomFileFilter, SuggestedSaveName(sourcePath));

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var checksumNote = reader.Save(path);

            if (right)
            {
                RightPath = path;
                RightHasChanges = false;
            }
            else
            {
                LeftPath = path;
                LeftHasChanges = false;
            }

            StatusMessage = $"Saved {(right ? "right" : "left")} ROM to {path}. {checksumNote}";
            return path;
        }

        private void OpenRom(bool right)
        {
            var path = Common.SelectFile(Common.RomFileFilter);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            try
            {
                LoadRom(path, right);
            }
            catch (InvalidOperationException)
            {
                StatusMessage = $"No definition matches the ROM ID in {Path.GetFileName(path)}.";
            }
        }

        private bool CanCopy()
        {
            return Selected?.LeftTable != null && Selected?.RightTable != null;
        }

        private void BuildComparisons()
        {
            Selected = null;

            if (_left == null || _right == null)
            {
                Groups = null;
                Summary = null;
                StatusMessage = $"Open the {(_left == null ? "left" : "right")} ROM to compare.";
                return;
            }

            _comparisons = RomComparer.Compare(_leftTables, _left, _rightTables, _right)
                                      .Select(c => new TableComparisonVM(c, _left, _right))
                                      .ToList();

            Groups = _comparisons.GroupBy(c => c.Category ?? "(no category)")
                                 .Select(g => new CompareGroupVM { Name = g.Key, Tables = g.ToList(), IsVisible = true })
                                 .ToList();

            UpdateSummary();
            ApplyFilter();
            StatusMessage = "Pick a table. Differing cells are outlined in orange.";
        }

        /// <summary>
        /// A copy that changed an axis also changes every other table that points at the same axis.
        /// Reloads those tables on the target side and returns how many there were.
        /// </summary>
        private int ReloadTablesSharingAxes(TableComparisonVM copied, bool right)
        {
            var axes = copied.AxisAddresses(right).ToHashSet();

            if (axes.Count == 0)
            {
                return 0;
            }

            var sharing = _comparisons.Where(c => c != copied && c.AxisAddresses(right).Any(axes.Contains)).ToList();

            foreach (var comparison in sharing)
            {
                comparison.Reload(right);
            }

            return sharing.Count;
        }

        private void UpdateSummary()
        {
            int Count(ComparisonStatus status) => _comparisons.Count(c => c.Status == status);

            Summary = $"{_comparisons.Count} tables: {Count(ComparisonStatus.Different)} different, "
                    + $"{Count(ComparisonStatus.SizeMismatch)} different size, "
                    + $"{Count(ComparisonStatus.OnlyLeft)} only left, {Count(ComparisonStatus.OnlyRight)} only right";
        }

        private void ApplyFilter()
        {
            if (Groups == null)
            {
                return;
            }

            var text = FilterText?.Trim();

            foreach (var group in Groups)
            {
                foreach (var table in group.Tables)
                {
                    var matchesStatus = !ShowOnlyDifferences || table.Status != ComparisonStatus.Identical;
                    var matchesText = string.IsNullOrEmpty(text) || table.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;

                    table.IsVisible = matchesStatus && matchesText;
                }

                group.IsVisible = group.Tables.Any(t => t.IsVisible);
            }
        }

        private static string Title(ITableReader reader, string path, bool hasChanges, string side)
        {
            if (reader == null)
            {
                return $"{side}: no ROM loaded";
            }

            return $"{side}: {reader.Rom?.RomId?.XmlId}  {Path.GetFileName(path)}{(hasChanges ? " *" : string.Empty)}";
        }

        private static string SuggestedSaveName(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var name = Path.GetFileNameWithoutExtension(path);
            return Path.Combine(Path.GetDirectoryName(path), $"{name}_modified{Path.GetExtension(path)}");
        }
    }
}
