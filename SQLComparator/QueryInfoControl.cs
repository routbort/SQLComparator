using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SQLComparator
{
    public partial class QueryInfoControl : UserControl
    {


        public SplitContainer Splitter { get { return this.splitContainer1; } }

        public QueryInfoControl()
        {
            InitializeComponent();
        }

        public string Query
        {
            get { return this.txtQuery.Text; }
            set { this.txtQuery.Text = value; }
        }

        public string ConnectionString
        {
            get { return this.txtConnection.Text; }
            set { this.txtConnection.Text = value; }
        }

        public string Caption
        {
            get { return this.label1.Text; }
            set { this.label1.Text = value; }
        }

    }
}
