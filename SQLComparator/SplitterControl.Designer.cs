using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SQLComparator
{
	public partial class SplitterControl : System.Windows.Forms.UserControl
	{

		//UserControl overrides dispose to clean up the component list.
		internal SplitterControl()
		{
			InitializeComponent();
		}
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
			this.Splitter = new System.Windows.Forms.SplitContainer();
			this.Splitter.SuspendLayout();
			this.SuspendLayout();
			//
			//Splitter
			//
			this.Splitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Splitter.Location = new System.Drawing.Point(0, 0);
			this.Splitter.Name = "Splitter";
			this.Splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			//
			//Splitter.Panel1
			//
			this.Splitter.Panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
			//
			//Splitter.Panel2
			//
			this.Splitter.Panel2.BackColor = System.Drawing.SystemColors.ButtonShadow;
			this.Splitter.Size = new System.Drawing.Size(608, 447);
			this.Splitter.SplitterDistance = 223;
			this.Splitter.TabIndex = 0;
			//
			//SplitterControl
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float)(6.0), (float)(13.0));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.Splitter);
			this.Name = "SplitterControl";
			this.Size = new System.Drawing.Size(608, 447);
			this.Splitter.ResumeLayout(false);
			this.ResumeLayout(false);

			Splitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(SplitterMoved);
			Splitter.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(SplitterMoving);
			this.Resize += new System.EventHandler(ComparisonControl_Resize);

		}
		internal System.Windows.Forms.SplitContainer Splitter;

	}

} //end of root namespace