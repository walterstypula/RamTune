/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 06/05/2011
 * Time: 14:24
 *
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */

using RamTune.Controls.Converters;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RamTune.Controls
{
    /// <summary>
    /// Description of FlexGrid.
    /// </summary>
    /// <summary>
    /// Description of Grid.
    /// </summary>
    [TemplatePart(Name = "PART_Grid", Type = typeof(Grid))]
    public class FlexGrid : SelectionBase
    {
        public FlexGrid()
        {
            DefaultStyleKey = typeof(FlexGrid);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(FlexGrid),
                                        new FrameworkPropertyMetadata(OnDataChanged));

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void OnDataChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs args)
        {
        }

        private static void OnColumnsChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs args)
        {
            if (dObj is FlexGrid flexGrid)
            {
                flexGrid.OnApplyTemplate();
            }
        }

        public static readonly DependencyProperty ColumnHeadersProperty =
            DependencyProperty.Register(nameof(ColumnHeaders), typeof(IList), typeof(FlexGrid), new FrameworkPropertyMetadata(OnColumnsChanged));

        public IList ColumnHeaders
        {
            get { return (IList)GetValue(ColumnHeadersProperty); }
            set
            {
                SetValue(ColumnHeadersProperty, value);
            }
        }

        public static readonly DependencyProperty RowsHeadersProperty =
           DependencyProperty.Register(nameof(RowHeaders), typeof(IList), typeof(FlexGrid), new FrameworkPropertyMetadata(OnRowsChanged));

        public IList RowHeaders
        {
            get
            {
                return (IList)GetValue(RowsHeadersProperty);
            }
            set
            {
                SetValue(RowsHeadersProperty, value);
            }
        }

        private static void OnRowsChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs args)
        {
            if (dObj is FlexGrid flexGrid)
            {
                flexGrid.OnApplyTemplate();
            }
        }

        public static readonly DependencyProperty SelectedCellIndexesProperty =
            DependencyProperty.Register("SelectedCellIndexes", typeof(ObservableCollection<int[]>), typeof(FlexGrid),
                                        new FrameworkPropertyMetadata(new ObservableCollection<int[]>()));

        public ObservableCollection<int[]> SelectedCellIndexes
        {
            get { return (ObservableCollection<int[]>)GetValue(SelectedCellIndexesProperty); }
            set { SetValue(SelectedCellIndexesProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Grid grid = base.GetTemplateChild("PART_Grid") as Grid;

            grid.Children.Clear();

            BuildGrid(grid);
            PopulateGrid(grid);
        }

        private void PopulateGrid(Grid grid)
        {
            if (grid.RowDefinitions.Count == 0 || grid.ColumnDefinitions.Count == 0)
            {
                return;
            }

            var converter = new DataArrayBindingConverter();

            for (int r = 1; r < grid.RowDefinitions.Count; r += 1)
            {
                for (int c = 1; c < grid.ColumnDefinitions.Count; c += 1)
                {
                    GridCell cell = new GridCell(this);
                    cell.SetValue(Grid.RowProperty, r);
                    cell.SetValue(Grid.ColumnProperty, c);

                    ControlsHelper.CreateControlBinding(cell, this, nameof(SelectionRangeColumnEnd), GridCell.SelectionRangeColumnEndProperty, BindingMode.TwoWay);
                    ControlsHelper.CreateControlBinding(cell, this, nameof(SelectionRangeColumnStart), GridCell.SelectionRangeColumnStartProperty, BindingMode.TwoWay);
                    ControlsHelper.CreateControlBinding(cell, this, nameof(SelectionRangeRowEnd), GridCell.SelectionRangeRowEndProperty, BindingMode.TwoWay);
                    ControlsHelper.CreateControlBinding(cell, this, nameof(SelectionRangeRowStart), GridCell.SelectionRangeRowStartProperty, BindingMode.TwoWay);
                    ControlsHelper.CreateControlBinding(cell, this, nameof(IsMouseDown), GridCell.IsMouseDownProperty, BindingMode.TwoWay);
                    ControlsHelper.CreateControlBinding(cell, this, nameof(InitialColumnStart), GridCell.InitialColumnStartProperty, BindingMode.TwoWay);
                    ControlsHelper.CreateControlBinding(cell, this, nameof(InitialRowStart), GridCell.InitialRowStartProperty, BindingMode.TwoWay);
                    ControlsHelper.CreateControlBinding(cell, this, nameof(Data), GridCell.TextProperty, BindingMode.TwoWay, converter, new int[] { r, c });

                    grid.Children.Add(cell);
                }
            }
        }

        protected void BuildGridColumns()
        {
            if (base.GetTemplateChild("PART_Grid") is Grid grid)
            {
                var columnStyle = TryFindResource("ColumnHeaders") as Style;

                var yAxisUnitsColumn = new ColumnDefinition() { MinWidth = 42 };
                grid.ColumnDefinitions.Add(yAxisUnitsColumn);

                var emptyLabel = new Label
                {
                    Style = columnStyle
                };

                emptyLabel.SetValue(Grid.ColumnProperty, grid.ColumnDefinitions.IndexOf(yAxisUnitsColumn));
                grid.Children.Add(emptyLabel);

                int columnCounter = 0;

                if (ColumnHeaders == null || ColumnHeaders.Count <= 0)
                    return;

                for (int i = 0; i < ColumnHeaders.Count; i++)
                {
                    var columnDefintion = new ColumnDefinition() { MinWidth = 42 };
                    grid.ColumnDefinitions.Add(columnDefintion);

                    var columnHeader = ColumnHeaders[i];

                    var columnLabel = new Label
                    {
                        Content = columnHeader,
                        Style = columnStyle
                    };
                    columnLabel.SetValue(Grid.ColumnProperty, grid.ColumnDefinitions.IndexOf(columnDefintion));

                    grid.Children.Add(columnLabel);
                    columnCounter++;
                }
            }
        }

        protected void BuildGridRows()
        {
            if (base.GetTemplateChild("PART_Grid") is Grid grid)
            {
                var emptyRow = new RowDefinition() { MinHeight = 25 };
                grid.RowDefinitions.Add(emptyRow);

                int rowCounter = 0;

                if (RowHeaders == null || RowHeaders.Count <= 0)
                    return;

                for (int i = 0; i < RowHeaders.Count; i++)
                {
                    var rowDefintion = new RowDefinition() { MinHeight = 25 };
                    grid.RowDefinitions.Add(rowDefintion);

                    var rowHeader = RowHeaders[i];

                    var rowLabel = new Label
                    {
                        Content = rowHeader,
                        Style = TryFindResource("RowHeaders") as Style
                    };
                    rowLabel.SetValue(Grid.RowProperty, grid.RowDefinitions.IndexOf(rowDefintion));

                    grid.Children.Add(rowLabel);
                    rowCounter++;
                }
            }
        }

        private void BuildGrid(Grid grid)
        {
            if ((RowHeaders?.Count ?? 0) == 0 || (ColumnHeaders?.Count ?? 0) == 0)
            {
                return;
            }

            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();

            BuildGridColumns();
            BuildGridRows();
        }
    }

    public class FlexColumn : FrameworkElement
    {
        public static readonly DependencyProperty ColumnNameProperty =
            DependencyProperty.Register("ColumnName", typeof(string), typeof(FlexColumn),
                                        new FrameworkPropertyMetadata());

        public string ColumnName
        {
            get { return (string)GetValue(ColumnNameProperty); }
            set { SetValue(ColumnNameProperty, value); }
        }
    }

    public class FlexRow : FrameworkElement
    {
        public static readonly DependencyProperty RowNameProperty =
            DependencyProperty.Register("RowName", typeof(string), typeof(FlexRow),
                                        new FrameworkPropertyMetadata());

        public string RowName
        {
            get { return (string)GetValue(RowNameProperty); }
            set { SetValue(RowNameProperty, value); }
        }
    }
}