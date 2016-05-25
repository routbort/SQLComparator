using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SQLComparator
{
    public class CompareDataGridView : DataGridView
    {

        public void SetBackColor(Color BackColor)
        {

            foreach (DataGridViewRow row in base.Rows)
            {
                for (int colindex = 0; colindex < base.ColumnCount; colindex++)
                {
                    row.Cells[colindex].Style.BackColor = BackColor;
                }
            }
            for (int colindex = 0; colindex < base.ColumnCount; colindex++)
            {
                base.Columns[colindex].HeaderCell.Style.BackColor = BackColor;
            }
        }

        public CompareDataGridView()
            : base()
        {

            //Change defaults
            base.AllowUserToAddRows = false;
            base.AllowUserToDeleteRows = true;
            base.AllowUserToResizeRows = false;
            base.RowHeadersVisible = false;
            base.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }


    }

} //end of root namespace