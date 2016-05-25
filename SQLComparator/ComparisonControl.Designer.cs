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

namespace SQLComparator
{
	public partial class ComparisonControl : System.Windows.Forms.UserControl
	{

		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;

		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.SplitterControl1 = new SQLComparator.SplitterControl();
			this.SuspendLayout();
			//
			//SplitterControl1
			//
			this.SplitterControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SplitterControl1.Location = new System.Drawing.Point(0, 0);
			this.SplitterControl1.Name = "SplitterControl1";
			this.SplitterControl1.Size = new System.Drawing.Size(356, 295);
			this.SplitterControl1.TabIndex = 0;
			//
			//ComparisonControl
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float)(6.0), (float)(13.0));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.SplitterControl1);
			this.Name = "ComparisonControl";
			this.Size = new System.Drawing.Size(356, 295);
			this.ResumeLayout(false);

			SplitterControl1.SplitMoved += new SplitterControl.SplitMovedEventHandler(SplitterControl1_SplitMoved);

		}
		internal SQLComparator.SplitterControl SplitterControl1;

	}

} //end of root namespace