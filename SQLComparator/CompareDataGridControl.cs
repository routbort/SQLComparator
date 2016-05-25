using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SQLComparator
{
    public partial class CompareDataGridControl
    {

        #region Instance variables

        private string _ErrorMessage = "";
        private DataTable _DataTable;
        private string _Query;
        private string _ConnectionString;
        private bool _InProgress = false;
        private List<string> _Columns = new List<string>();

        #endregion Instance variables

        #region Implementation

        private delegate void MessageSetter(string value);

        private void DataGridView1_ColumnWidthChanged(object sender, System.Windows.Forms.DataGridViewColumnEventArgs e)
        {
            if (ColumnWidthChanged != null)
                ColumnWidthChanged(sender, e);
        }

        #endregion

        #region Public methods

        public void SetMessage(string value)
        {
            if (this.InvokeRequired)
            {
                MessageSetter ms = new MessageSetter(SetMessage);
                this.Invoke(ms, new object[] { value });
            }
            else
            {
                this.lblMessage.Text = value;
                this.lblMessage.Visible = true;
                this.DataGridView1.Visible = false;
            }
        }

        public void BindDataTable()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(BindDataTable));
            }
            else
            {
                this.lblMessage.Visible = false;
                this.DataGridView1.Visible = true;
                this.DataGridView1.DataSource = _DataTable;
                _Columns.Clear();
                foreach (DataColumn col in _DataTable.Columns)
                    _Columns.Add(col.ColumnName);
                this.listBox1.DataSource = _Columns;
            }

        }

        public void SetBackColor(Color BackColor)
        {
            foreach (DataGridViewRow row in DataGridView1.Rows)
            {
                for (int colindex = 0; colindex < DataGridView1.Columns.Count; colindex++)
                {
                    row.Cells[colindex].Style.BackColor = BackColor;
                }
            }
            for (int colindex = 0; colindex < DataGridView1.Columns.Count; colindex++)
            {
                DataGridView1.Columns[colindex].HeaderCell.Style.BackColor = BackColor;
            }

        }
 
        #endregion

        #region Public properties

        private string _PendingErrorMessage = "";

        public string PendingErrorMessage
        {
            get { return _PendingErrorMessage; }
            set { _PendingErrorMessage = value; }
        }



        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                _ErrorMessage = value;
                if (_ErrorMessage == "")
                {
                    _PendingErrorMessage = "";
                }
                else
                {
                    this.InProgress = false;
                    SetMessage(value);
                }
            }
        }

        public bool IsValid { get { return (_ErrorMessage == ""); } }

        public DataTable DataTable
        {
            get
            {
                return _DataTable;
            }
            set
            {
                _DataTable = value;
            }
        }

        public List<string> ColumnNames { get { return _Columns; } }


        public string Query
        {
            get
            {
                return _Query;
            }
            set
            {
                _Query = value;
            }
        }

        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }

        public DataGridView Grid
        {
            get
            {
                return this.DataGridView1;
            }
        }

        public bool InProgress
        {
            get
            {
                return _InProgress;
            }
            set
            {
                _InProgress = value;
            }
        }

        #endregion

        #region Events

        public delegate void ColumnWidthChangedEventHandler(object sender, System.Windows.Forms.DataGridViewColumnEventArgs e);
        public event ColumnWidthChangedEventHandler ColumnWidthChanged;
        
#endregion

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }

} //end of root namespace