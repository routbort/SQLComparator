using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Threading;
using System.Text;
using System.Data.OleDb;
using System.Data.Odbc;

namespace SQLComparator
{
    public partial class ComparisonControl
    {
        #region Constants

        private const string MATCH_COLUMN = "MatchResult";

        #endregion Constants

        #region Instance variables

        private List<BackgroundWorker> _Workers = new List<BackgroundWorker>();
        private CompareDataGridControl _Grid1;
        private CompareDataGridControl _Grid2;
        private Color _HighlightHeader = Color.LightGray;
        private Color _HighlightCell = Color.DarkGoldenrod;
        private Color _HighlightMismatchedRow = Color.DarkGray;
        private GridComparisonResult _Result = null;
        private string _ResultMessage = "";
        private System.ComponentModel.BackgroundWorker _backgroundWorker;
        private int _NumberOfColumnMismatchesAllowed = 0;
        private bool _UseAdvancedCompare = false;
        private List<string> _ColumnsMapped;
        private List<string> _ColumnsWithMismatches;
        private bool _GridIsScrolling = false;

        #endregion Instance variables

        #region Events

        public delegate void CompareCompletedEventHandler(object sender, GridComparisonResult result);
        public event CompareCompletedEventHandler CompareCompleted;

        #endregion Events

        #region Public properties

        public string ConnectionString1
        {
            get
            {
                return _Grid1.ConnectionString;
            }
            set
            {
                _Grid1.ConnectionString = value;
            }
        }

        public string ConnectionString2
        {
            get
            {
                return _Grid2.ConnectionString;
            }
            set
            {
                _Grid2.ConnectionString = value;
            }
        }

        public string Query1
        {
            get
            {
                return _Grid1.Query;
            }
            set
            {
                _Grid1.Query = value;

            }
        }

        public string Query2
        {
            get
            {
                return _Grid2.Query;
            }
            set
            {
                _Grid2.Query = value;
            }
        }

        public System.Windows.Forms.Orientation Orientation
        {
            get
            {
                return this.SplitterControl1.Splitter.Orientation;
            }
            set
            {
                this.SplitterControl1.Splitter.Orientation = value;
                this.SplitterControl1.Center();
            }
        }

        public int NumberOfColumnMismatchesAllowed
        {
            get { return _NumberOfColumnMismatchesAllowed; }
            set { _NumberOfColumnMismatchesAllowed = value; }
        }

        public bool UseAdvancedCompare
        {
            get { return _UseAdvancedCompare; }
            set { _UseAdvancedCompare = value; }
        }

        public GridComparisonResult Result
        {
            get { return _Result; }
        }

        public DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
        {
            get { return this._Grid1.DataGridView1.AutoSizeColumnsMode; }
            set
            {
                this._Grid1.DataGridView1.AutoSizeColumnsMode = value;
                this._Grid2.DataGridView1.AutoSizeColumnsMode = value;
            }

        }

        #endregion

        #region Constructors

        public ComparisonControl()
        {

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.

            _Grid1 = new CompareDataGridControl();
            _Grid2 = new CompareDataGridControl();
            this.SplitterControl1.Splitter.Panel1.Controls.Add(_Grid1);
            this.SplitterControl1.Splitter.Panel2.Controls.Add(_Grid2);
            _Grid1.Dock = DockStyle.Fill;
            _Grid2.Dock = DockStyle.Fill;

        }

        #endregion

        #region Public methods

        private string GetTSVContents(DataGridView grid, bool First)
        {
            StringBuilder sbCSV = new StringBuilder();
            List<string> cols = new List<string>();
            foreach (DataGridViewTextBoxColumn col in grid.Columns)
            {
                cols.Add(col.Name);
                if (First)
                {
                    sbCSV.Append(col.Name);
                    sbCSV.Append("\t");
                }
            }

            if (First) sbCSV.Append(System.Environment.NewLine);

            foreach (DataGridViewRow row in grid.Rows)
                if (First || (!row.Cells["MatchResult"].Value.ToString().Contains("Full") && !row.Cells["MatchResult"].Value.ToString().Contains("Partial")))
                {
                    foreach (string col in cols)
                    {
                        sbCSV.Append(row.Cells[col].Value.ToString().Replace(System.Environment.NewLine, ""));
                        sbCSV.Append("\t");
                    }
                    sbCSV.Append(System.Environment.NewLine);
                }

            return sbCSV.ToString();
        }

        public string GetTSV()
        {
            string output1 = GetTSVContents(this._Grid1.DataGridView1, true);
            string output2 = GetTSVContents(this._Grid2.DataGridView1, true);

            return GetTSVContents(this._Grid1.DataGridView1, true) + GetTSVContents(this._Grid2.DataGridView1, false);

        }


        public void Abort()
        {

            foreach (BackgroundWorker bw in _Workers)
            {
                bw.CancelAsync();
            }

            _Workers.Clear();
            _Grid1.SetMessage("");
            _Grid2.SetMessage("");

        }

        public void Compare()
        {
            if (_Grid1.InProgress || _Grid2.InProgress)
                Abort();
            _Grid1.SetMessage("Query in progress ....");
            _Grid2.SetMessage("Query in progress ....");
            _Grid1.ErrorMessage = "";
            _Grid2.ErrorMessage = "";

            try
            {
                string[] IllegalWords = { "truncate", "delete", "update", "insert" };

                foreach (string IllegalWord in IllegalWords)
                {
                    if ((_Grid1.Query.ToUpper().IndexOf(IllegalWord.ToUpper(), 0) + 1) != 0 | (_Grid2.Query.ToUpper().IndexOf(IllegalWord.ToUpper(), 0) + 1) != 0)
                    {
                        _Grid1.SetMessage("");
                        _Grid2.SetMessage("");
                        _Result = new GridComparisonResult(false, "Illegal keyword in query detected: " + IllegalWord);
                        if (CompareCompleted != null)
                            CompareCompleted(this, _Result);
                        return;
                    }
                }

                this.Cursor = Cursors.WaitCursor;
                BackgroundWorker bgw = new BackgroundWorker();
                bgw.WorkerSupportsCancellation = true;
                this._Workers.Add(bgw);
                this._Grid1.InProgress = true;
                bgw.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
                bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                bgw.RunWorkerAsync(_Grid1);

                this._Grid2.InProgress = true;
                bgw = new BackgroundWorker();
                bgw.WorkerSupportsCancellation = true;
                this._Workers.Add(bgw);
                bgw.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
                bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                bgw.RunWorkerAsync(_Grid2);

                this.Visible = true;

            }
            catch (Exception ex)
            {

                _Result = new GridComparisonResult(false, "ERROR: " + ex.Message);
                this.Visible = false;
                if (CompareCompleted != null)
                    CompareCompleted(this, _Result);
                return;

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }


        }

        #endregion

        #region Implementation

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this._Workers.Remove((BackgroundWorker)sender);
            if (e.Cancelled)
                return;
            CompareDataGridControl Grid = (CompareDataGridControl)e.Result;

            if (Grid.PendingErrorMessage != "")
                Grid.ErrorMessage = Grid.PendingErrorMessage;

            Grid.InProgress = false;

            if (Grid.ErrorMessage != "")
            {
                System.Diagnostics.Debug.WriteLine(Grid.ErrorMessage);
            }

            if (Grid.IsValid && (_Grid1.InProgress || _Grid2.InProgress))
            {
                //One query has completed but the other is still ongoing
                Grid.SetMessage("Query complete.  Waiting for other query ...");
            }
            else
            {
                if (_Grid1.IsValid && _Grid2.IsValid)
                {
                    _Grid1.SetMessage("Comparing query results (" + _Grid1.DataTable.Rows.Count.ToString() + " rows) - this may take a while ...");
                    _Grid2.SetMessage("Comparing query results (" + _Grid2.DataTable.Rows.Count.ToString() + " rows) - this may take a while ...");
                    this.Refresh();
                    CompareGrids();
                }
                else
                {
                    _Result = new GridComparisonResult(false, _Grid1.ErrorMessage + System.Environment.NewLine + _Grid2.ErrorMessage);
                    if (CompareCompleted != null)
                        CompareCompleted(this, _Result);

                    //Need to show any valid grids; normally this is done after the comparison to increase speed

                    if (_Grid1.IsValid)
                    {
                        _Grid1.BindDataTable();
                        PrepareGrid(_Grid1, true);
                    }

                    if (_Grid2.IsValid)
                    {
                        _Grid2.BindDataTable();
                        PrepareGrid(_Grid2, true);
                    }
                }
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            e.Result = DoQuery(e.Argument, worker, e);
        }

        private CompareDataGridControl DoQuery(object Argument, BackgroundWorker worker, DoWorkEventArgs e)
        {
            CompareDataGridControl Grid = (CompareDataGridControl)Argument;
            DataSet ds = new DataSet();
            bool UseODBC = false;
            OleDbConnection cnOLEDB = null;
            OdbcConnection cnODBC = null;

            try
            {
                cnOLEDB = new OleDbConnection(Grid.ConnectionString);
                cnOLEDB.Open();
            }
            catch (ArgumentException ex)
            {
                UseODBC = true;
            }
            catch (Exception ex)
            {
                Grid.PendingErrorMessage = ex.Message;
                return Grid;
            }


            try
            {
                if (UseODBC)
                {
                    cnODBC = new OdbcConnection(Grid.ConnectionString);
                    cnODBC.Open();
                    OdbcCommand dc = new OdbcCommand(Grid.Query, cnODBC);
                    dc.CommandTimeout = 600;
                    OdbcDataAdapter da = new OdbcDataAdapter(dc);
                    da.Fill(ds, "Table");
                    if (ds.Tables.Count == 1)
                        Grid.DataTable = ds.Tables["Table"];
                    else
                    {
                        //try to merge multiple tables
                        DataTable output = ds.Tables[0];
                        for (int index = 1; index < ds.Tables.Count; index++)
                            output.Merge(ds.Tables[index]);
                        Grid.DataTable = output;
                    }
                }
                else
                {
                    OleDbCommand dc = new OleDbCommand(Grid.Query, cnOLEDB);
                    dc.CommandTimeout = 600;
                    OleDbDataAdapter da = new OleDbDataAdapter(dc);
                    da.Fill(ds, "Table");
                    if (ds.Tables.Count == 1)
                        Grid.DataTable = ds.Tables["Table"];
                    else
                    {
                        //try to merge multiple tables
                        DataTable output = ds.Tables[0];
                        for (int index = 1; index < ds.Tables.Count; index++)
                            output.Merge(ds.Tables[index]);
                        Grid.DataTable = output;
                    }
                }
            }
            catch (Exception ex)
            {
                Grid.PendingErrorMessage = ex.Message;
            }

            return Grid;

        }

        private void CompareGrids()
        {
            if (_UseAdvancedCompare)
            {
                bool Success = CompareGridsAdvanced();

                _Grid1.BindDataTable();
                _Grid2.BindDataTable();

                PrepareGrid(_Grid1, false);
                PrepareGrid(_Grid2, false);

                if (Success)
                    HighlightChangesAdvanced();

                if (CompareCompleted != null)
                    CompareCompleted(this, _Result);

            }

            else
            {
                _Grid1.BindDataTable();
                _Grid2.BindDataTable();

                PrepareGrid(_Grid1, false);
                PrepareGrid(_Grid2, false);

                CompareGridsSimple();
            }
        }

        private void CompareGridsSimple()
        {

            _ResultMessage = "";

            _Grid1.Grid.CurrentCell = null;
            _Grid2.Grid.CurrentCell = null;

            if (_Grid1.Grid.Rows.Count != _Grid2.Grid.Rows.Count)
            {
                _ResultMessage += "Row counts do not match (" + _Grid1.Grid.Rows.Count.ToString() + " versus " + _Grid2.Grid.Rows.Count.ToString() + ")" + System.Environment.NewLine;
            }

            if (_Grid1.Grid.Columns.Count != _Grid2.Grid.Columns.Count)
            {
                _ResultMessage += "Column counts do not match.(" + _Grid1.Grid.Columns.Count.ToString() + " versus " + _Grid2.Grid.Columns.Count.ToString() + ")" + System.Environment.NewLine;
            }

            _ResultMessage += MapColumns();

            if (_ResultMessage != "")
            {
                _ResultMessage += System.Environment.NewLine + "CANNOT COMPARE using simple algorithm." + System.Environment.NewLine;
                _Grid1.Enabled = false;
                _Grid2.Enabled = false;
                _Result = new GridComparisonResult(false, _ResultMessage);
                if (CompareCompleted != null)
                    CompareCompleted(this, _Result);
                return;

            }

            _Grid1.Enabled = true;
            _Grid2.Enabled = true;

            DataGridViewCell Cell1 = null;
            DataGridViewCell Cell2 = null;
            DataGridViewCell oCell = null;

            int RowDiffCount = 0;
            int ColDiffCount = 0;
            int CellDiffCount = 0;

            _Grid1.SetBackColor(_Grid1.Grid.DefaultCellStyle.BackColor);
            _Grid2.SetBackColor(_Grid2.Grid.DefaultCellStyle.BackColor);

            bool RowChanged = false;

            foreach (DataGridViewRow row in _Grid1.Grid.Rows)
            {
                foreach (DataGridViewColumn column in _Grid1.Grid.Columns)
                {
                    Cell1 = row.Cells[column.Name];
                    Cell2 = _Grid2.Grid.Rows[row.Index].Cells[column.Index];
                    if (Cell1.Value.ToString() != Cell2.Value.ToString())
                    {

                        CellDiffCount += 1;

                        if (!RowChanged)
                        {
                            RowChanged = true;
                            RowDiffCount += 1;

                            //Need to back track highlighting
                            for (int i = 0; i < Cell1.ColumnIndex; i++)
                            {
                                oCell = row.Cells[i];
                                if (oCell.Style.BackColor == _Grid1.Grid.DefaultCellStyle.BackColor)
                                {
                                    oCell.Style.BackColor = _HighlightHeader;
                                    _Grid2.Grid.Rows[row.Index].Cells[i].Style.BackColor = _HighlightHeader;
                                }
                            }
                        }
                        Cell1.Style.BackColor = _HighlightCell;
                        Cell2.Style.BackColor = _HighlightCell;
                        column.HeaderCell.Style.BackColor = _HighlightHeader;
                        _Grid2.Grid.Columns[column.Index].HeaderCell.Style.BackColor = _HighlightHeader;
                    }
                    else
                    {
                        if (RowChanged)
                        {
                            Cell1.Style.BackColor = _HighlightHeader;
                            Cell2.Style.BackColor = _HighlightHeader;
                        }

                    }
                }
                RowChanged = false;
            }

            ColDiffCount = 0;

            foreach (DataGridViewColumn column in _Grid1.Grid.Columns)
            {
                if (column.HeaderCell.Style.BackColor != _Grid1.Grid.DefaultCellStyle.BackColor)
                {
                    ColDiffCount += 1;
                    foreach (DataGridViewRow row in _Grid1.Grid.Rows)
                    {
                        if (row.Cells[column.Index].Style.BackColor != _HighlightCell)
                        {
                            row.Cells[column.Index].Style.BackColor = _HighlightHeader;
                            _Grid2.Grid.Rows[row.Index].Cells[column.Index].Style.BackColor = _HighlightHeader;
                        }
                    }
                }

            }


            if (CellDiffCount != 0)
            {
                _ResultMessage += RowDiffCount + " out of " + _Grid1.Grid.Rows.Count + " rows are different" + System.Environment.NewLine;
                _ResultMessage += ColDiffCount + " out of " + _Grid1.Grid.Columns.Count + " cols are different" + System.Environment.NewLine;
                _ResultMessage += CellDiffCount + " out of " + _Grid1.Grid.Rows.Count * _Grid1.Grid.Columns.Count + " cells are different" + System.Environment.NewLine;

                _Result = new GridComparisonResult(false, _ResultMessage);
            }
            else
            {
                _Result = new GridComparisonResult(true, "IDENTICAL");
            }

            if (CompareCompleted != null)
                CompareCompleted(this, _Result);

        }

        private string MapColumns()
        {
            string result = "";
            this._ColumnsMapped = new List<string>();

            foreach (DataColumn col in _Grid1.DataTable.Columns)
            {

                if (_Grid2.DataTable.Columns.Contains(col.ColumnName) && col.ColumnName != MATCH_COLUMN)
                {
                    _ColumnsMapped.Add(col.ColumnName);
                }
                else
                {
                    if (col.ColumnName != MATCH_COLUMN)
                        result += "Column '" + col.ColumnName + "' only present in result 1" + System.Environment.NewLine;
                }
            }

            //Now find any unmapped columns  from Grid2 to update the comparison message



            foreach (DataColumn col in _Grid2.DataTable.Columns)
            {
                if (!_ColumnsMapped.Contains(col.ColumnName) && col.ColumnName != MATCH_COLUMN)
                {
                    result += "Column '" + col.ColumnName + "' only present in result 2" + System.Environment.NewLine;
                }

            }

            return result;

        }

        private void ReorderColumns()
        {
            if (this._ColumnsMapped == null) MapColumns();

            _Grid1.DataGridView1.Columns[MATCH_COLUMN].DisplayIndex = 0;
            _Grid2.DataGridView1.Columns[MATCH_COLUMN].DisplayIndex = 0;

            int i = 1;
            foreach (string ColumnName in _ColumnsMapped)
            {
                _Grid1.Grid.Columns[ColumnName].DisplayIndex = i;
                _Grid2.Grid.Columns[ColumnName].DisplayIndex = i;
                i++;
            }


        }

        private int ColMisMatchCount(DataRow row1, DataRow row2, int MaxMismatchesAllowed)
        {
            int DiffCount = 0;

            //We will iterate only across mapped columns - we cannot compare inserted or deleted columns
            foreach (string ColName in this._ColumnsMapped)
                if (row1[ColName].ToString() != row2[ColName].ToString())
                {
                    DiffCount++;
                    if (DiffCount > MaxMismatchesAllowed) break;
                }
            return DiffCount;

        }

        private void HighlightRow(DataGridViewRow row, Color color)
        {
            foreach (DataGridViewCell cell in row.Cells)
                cell.Style.BackColor = color;
        }

        private void HighlightMismatchedRows(CompareDataGridControl grid)
        {
            foreach (DataGridViewRow row in grid.Grid.Rows)
            {
                MatchDetail md = row.Cells[MATCH_COLUMN].Value as MatchDetail;
                if (md != null && md.Type == MatchDetail.MatchType.None)
                    HighlightRow(row, this._HighlightMismatchedRow);
            }
        }

        private void HighlightMismatchedColumns(CompareDataGridControl grid)
        {
            foreach (DataGridViewColumn col in grid.Grid.Columns)
                if (!_ColumnsMapped.Contains(col.Name) && col.Name != MATCH_COLUMN)
                {
                    col.HeaderCell.Style.BackColor = _HighlightHeader;
                    foreach (DataGridViewRow row in grid.Grid.Rows)
                    {
                        row.Cells[col.Name].Style.BackColor = _HighlightHeader;
                    }
                }
                else
                {
                    if (_ColumnsWithMismatches.Contains(col.Name))
                    {
                        col.HeaderCell.Style.BackColor = _HighlightHeader;
                        foreach (DataGridViewRow row in grid.Grid.Rows)
                        {
                            DataGridViewCell cell = row.Cells[col.Name];
                            if (cell.Style.BackColor == Color.Empty)
                                cell.Style.BackColor = _HighlightHeader;
                        }
                    }
                }


        }

        private void HighlightCells(int RowIndex, string ColumnName, Color color, bool BothGrids)
        {
            _Grid1.Grid.Rows[RowIndex].Cells[ColumnName].Style.BackColor = color;
            if (BothGrids)
                _Grid2.Grid.Rows[RowIndex].Cells[ColumnName].Style.BackColor = color;
        }

        private void PrepareGrid(CompareDataGridControl grid, bool AllowSort)
        {
            grid.Grid.CurrentCell = null;
            grid.Enabled = true;
            if (grid.DataGridView1.Columns.Contains(MATCH_COLUMN))
                grid.DataGridView1.Columns[MATCH_COLUMN].DisplayIndex = 0;
            foreach (DataGridViewColumn col in grid.DataGridView1.Columns)
                col.SortMode = (AllowSort ? DataGridViewColumnSortMode.Automatic : DataGridViewColumnSortMode.NotSortable);
            grid.SetBackColor(grid.Grid.DefaultCellStyle.BackColor);
            grid.Dock = DockStyle.Fill;
            grid.ColumnWidthChanged += new CompareDataGridControl.ColumnWidthChangedEventHandler(ColumnWidthChanged);
            grid.DataGridView1.Scroll += new System.Windows.Forms.ScrollEventHandler(Grid_Scroll);

        }

        private int GetMismatchCount(int?[][] Compare,
                                        CompareDataGridControl BigGrid,
                                        int BigGridIndex,
                                        CompareDataGridControl LittleGrid,
                                        int LittleGridIndex)
        {
            if (Compare[BigGridIndex][LittleGridIndex] == null)
                Compare[BigGridIndex][LittleGridIndex] = ColMisMatchCount(BigGrid.DataTable.Rows[BigGridIndex], LittleGrid.DataTable.Rows[LittleGridIndex], _NumberOfColumnMismatchesAllowed);
            return Compare[BigGridIndex][LittleGridIndex].Value;
        }


        private int GetMismatchCountDict(Dictionary<int, Dictionary<int, int>> CompareDict,
                                        CompareDataGridControl BigGrid,
                                        int BigGridIndex,
                                        CompareDataGridControl LittleGrid,
                                        int LittleGridIndex)
        {

            if (!CompareDict.ContainsKey(BigGridIndex))
                CompareDict[BigGridIndex] = new Dictionary<int, int>();
            if (!CompareDict[BigGridIndex].ContainsKey(LittleGridIndex)) ;
            CompareDict[BigGridIndex][LittleGridIndex] = ColMisMatchCount(BigGrid.DataTable.Rows[BigGridIndex], LittleGrid.DataTable.Rows[LittleGridIndex], _NumberOfColumnMismatchesAllowed);
            return CompareDict[BigGridIndex][LittleGridIndex];

        }

        private void HighlightChangesAdvanced()
        {

            ReorderColumns();

            _Grid1.Grid.Sort(_Grid1.Grid.Columns[MATCH_COLUMN], ListSortDirection.Ascending);
            _Grid2.Grid.Sort(_Grid2.Grid.Columns[MATCH_COLUMN], ListSortDirection.Ascending);

            //Highlight cell differences
            int CellDiffCount = 0;
            foreach (DataGridViewRow row in _Grid1.Grid.Rows)
            {
                int RowIndex = row.Index;
                MatchDetail md = row.Cells[MATCH_COLUMN].Value as MatchDetail;
                if (md == null) continue;
                if (md.Type == MatchDetail.MatchType.Partial)
                {
                    HighlightCells(RowIndex, MATCH_COLUMN, _HighlightHeader, true);
                    foreach (DataGridViewCell cell in row.Cells)
                        if (_ColumnsMapped.Contains(cell.OwningColumn.Name))
                            if (cell.Value.ToString() != _Grid2.Grid.Rows[RowIndex].Cells[cell.OwningColumn.Name].Value.ToString())
                            {
                                CellDiffCount++;
                                HighlightCells(RowIndex, cell.OwningColumn.Name, _HighlightCell, true);
                                if (!this._ColumnsWithMismatches.Contains(cell.OwningColumn.Name))
                                    this._ColumnsWithMismatches.Add(cell.OwningColumn.Name);
                            }
                            else
                            {
                                HighlightCells(RowIndex, cell.OwningColumn.Name, _HighlightHeader, true);
                            }
                }
            }

            //Highlight full mismatch rows

            HighlightMismatchedRows(_Grid1);
            HighlightMismatchedRows(_Grid2);

            foreach (DataGridViewRow row in _Grid1.Grid.Rows)
            {
                MatchDetail md = row.Cells[MATCH_COLUMN].Value as MatchDetail;
                if (md != null && md.Type == MatchDetail.MatchType.None2)
                    HighlightRow(row, this._HighlightMismatchedRow);
            }
            foreach (DataGridViewRow row in _Grid2.Grid.Rows)
            {
                MatchDetail md = row.Cells[MATCH_COLUMN].Value as MatchDetail;
                if (md != null && md.Type == MatchDetail.MatchType.None1)
                    HighlightRow(row, this._HighlightMismatchedRow);
            }

            //Highlight missing/inserted columns

            HighlightMismatchedColumns(_Grid1);
            HighlightMismatchedColumns(_Grid2);

            //Count diferences

            if (MatchDetail.GetTypeCount(MatchDetail.MatchType.None)
                + MatchDetail.GetTypeCount(MatchDetail.MatchType.Partial)
                + MatchDetail.GetTypeCount(MatchDetail.MatchType.None1)
                + MatchDetail.GetTypeCount(MatchDetail.MatchType.None2) == 0)
            {
                _Result = new GridComparisonResult(true, _ResultMessage + "IDENTICAL");
            }
            else
            {
                _ResultMessage += MatchDetail.GetTypeCount(MatchDetail.MatchType.Full).ToString() + " full row matches. " + System.Environment.NewLine;
                _ResultMessage += MatchDetail.GetTypeCount(MatchDetail.MatchType.Partial).ToString() + " partial row matches. " + System.Environment.NewLine;
                _ResultMessage += MatchDetail.GetTypeCount(MatchDetail.MatchType.None2) + ":"
                               + MatchDetail.GetTypeCount(MatchDetail.MatchType.None1)
                               + " total unmatched row matches. " + System.Environment.NewLine;
                _ResultMessage += this._ColumnsWithMismatches.Count.ToString() + " columns with mismatched data." + System.Environment.NewLine;
                _ResultMessage += CellDiffCount.ToString() + " total cells different in partially matched rows." + System.Environment.NewLine;
                _Result = new GridComparisonResult(false, _ResultMessage);
            }

            _Grid1.Grid.CurrentCell = null;
            _Grid2.Grid.CurrentCell = null;

        }

        private bool CompareGridsAdvanced()
        {

            _ResultMessage = MapColumns();
            this._ColumnsWithMismatches = new List<string>();

            if (_ColumnsMapped.Count == 0)
            {
                _Result = new GridComparisonResult(false, "Cannot compare grids - no columns are shared."
                          + System.Environment.NewLine + _ResultMessage);
                return false;
            }

            MatchDetail.Reset();
            _Grid1.DataTable.Columns.Add(new DataColumn(MATCH_COLUMN, typeof(MatchDetail)));
            _Grid2.DataTable.Columns.Add(new DataColumn(MATCH_COLUMN, typeof(MatchDetail)));

            //Prepare for comparisons in which the two grids may not have the same row count
            CompareDataGridControl BigGrid;
            CompareDataGridControl LittleGrid;

            if (_Grid1.DataTable.Rows.Count >= _Grid2.DataTable.Rows.Count)
            {
                BigGrid = _Grid1;
                LittleGrid = _Grid2;
            }
            else
            {
                BigGrid = _Grid2;
                LittleGrid = _Grid1;
            }

            int BigRowcount = BigGrid.DataTable.Rows.Count;
            int LittleRowcount = LittleGrid.DataTable.Rows.Count;
            Dictionary<int, Dictionary<int, int>> CompareDict = new Dictionary<int, Dictionary<int, int>>();

            int?[][] Compare = new int?[BigRowcount][];
            for (int i = 0; i < Compare.Length; i++)
                Compare[i] = new int?[LittleRowcount];

            bool[] BigGridMatched = new bool[BigRowcount];
            bool[] LittleGridMatched = new bool[LittleRowcount];

            //First cycle - find all perfect matches
            for (int i = 0; i < BigRowcount; i++)
                for (int j = 0; j < LittleRowcount; j++)
                    if (!LittleGridMatched[j] && GetMismatchCount(Compare, BigGrid, i, LittleGrid, j) == 0)
                    {
                        MatchDetail md = MatchDetail.GetMatchDetail(MatchDetail.MatchType.Full);
                        BigGrid.DataTable.Rows[i][MATCH_COLUMN] = md;
                        LittleGrid.DataTable.Rows[j][MATCH_COLUMN] = md;
                        BigGridMatched[i] = true;
                        LittleGridMatched[j] = true;
                        break;
                    }


            //Second cycle - find all partial matches
            for (int i = 0; i < BigRowcount; i++)
                if (!BigGridMatched[i])
                    for (int j = 0; j < LittleRowcount; j++)
                        if (!LittleGridMatched[j] && GetMismatchCountDict(CompareDict, BigGrid, i, LittleGrid, j) <= this._NumberOfColumnMismatchesAllowed)
                        {
                            MatchDetail md = MatchDetail.GetMatchDetail(MatchDetail.MatchType.Partial);
                            BigGrid.DataTable.Rows[i][MATCH_COLUMN] = md;
                            LittleGrid.DataTable.Rows[j][MATCH_COLUMN] = md;
                            BigGridMatched[i] = true;
                            LittleGridMatched[j] = true;
                            break;
                        }


            //Last cycle - for each grid, mark remaining rows as mismatches and inserting blank rows to allow alignment
            for (int i = 0; i < BigRowcount; i++)
                if (!BigGridMatched[i])
                {
                    MatchDetail md = MatchDetail.GetMatchDetail(MatchDetail.MatchType.None1);
                    DataRow dr = LittleGrid.DataTable.NewRow();
                    LittleGrid.DataTable.Rows.Add(dr);
                    dr[MATCH_COLUMN] = md;
                    BigGrid.DataTable.Rows[i][MATCH_COLUMN] = md;
                }

            for (int j = 0; j < LittleRowcount; j++)
                if (!LittleGridMatched[j])
                {
                    MatchDetail md = MatchDetail.GetMatchDetail(MatchDetail.MatchType.None2);
                    DataRow dr = BigGrid.DataTable.NewRow();
                    BigGrid.DataTable.Rows.Add(dr);
                    dr[MATCH_COLUMN] = md;
                    LittleGrid.DataTable.Rows[j][MATCH_COLUMN] = md;
                }

            return true;

        }

        private void ColumnWidthChanged(object sender, System.Windows.Forms.DataGridViewColumnEventArgs e)
        {

            DataGridView Target = null;
            if (sender == _Grid1.DataGridView1)
            {
                Target = _Grid2.Grid;
            }
            else
            {
                Target = _Grid1.Grid;
            }
            Target.Columns[e.Column.Index].Width = e.Column.Width;

        }

        private void Grid_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            if (!_GridIsScrolling)
            {
                _GridIsScrolling = true;
                CompareDataGridControl ActiveGrid = (sender == _Grid1.DataGridView1 ? _Grid1 : _Grid2);
                CompareDataGridControl PassiveGrid = (sender == _Grid1.DataGridView1 ? _Grid2 : _Grid1);
                switch (e.ScrollOrientation)
                {
                    case ScrollOrientation.HorizontalScroll:
                        try
                        {
                            PassiveGrid.Grid.HorizontalScrollingOffset = ActiveGrid.Grid.HorizontalScrollingOffset;
                        }
                        catch { }
                        break;
                    case ScrollOrientation.VerticalScroll:
                        try
                        {
                            PassiveGrid.Grid.FirstDisplayedScrollingRowIndex = ActiveGrid.Grid.FirstDisplayedScrollingRowIndex;
                        }
                        catch { }
                        break;
                }
                _GridIsScrolling = false;
            }
        }

        private void SplitterControl1_SplitMoved()
        {
            _Grid2.Grid.HorizontalScrollingOffset = _Grid1.Grid.HorizontalScrollingOffset;
            if (_Grid1.Grid.FirstDisplayedScrollingRowIndex != -1)
                _Grid2.Grid.FirstDisplayedScrollingRowIndex = _Grid1.Grid.FirstDisplayedScrollingRowIndex;
        }

        #endregion

    }

} //end of root namespace