using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SQLComparator
{
	public partial class Main : PrintableForm
	{

		//Form overrides dispose to clean up the component list.
		internal Main()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.pnlQueryHolder = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.queryInfoControl1 = new SQLComparator.QueryInfoControl();
            this.queryInfoControl2 = new SQLComparator.QueryInfoControl();
            this.comparisonControl1 = new SQLComparator.ComparisonControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.printScreenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeComparisonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.buildConnectionString1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildConnectionString2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildBothConnectionStringsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.useAdvancedCompareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.horizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.showCommentsFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkUseAdvanceCompare = new System.Windows.Forms.CheckBox();
            this.txtMaxAllowedMismatches = new System.Windows.Forms.TextBox();
            this.lblMaxAllowedMismatches = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pnlComments = new System.Windows.Forms.GroupBox();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.saveOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlQueryHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlComments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlQueryHolder
            // 
            this.pnlQueryHolder.Controls.Add(this.splitContainer1);
            this.pnlQueryHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQueryHolder.Location = new System.Drawing.Point(0, 0);
            this.pnlQueryHolder.Name = "pnlQueryHolder";
            this.pnlQueryHolder.Size = new System.Drawing.Size(536, 214);
            this.pnlQueryHolder.TabIndex = 24;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.queryInfoControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.queryInfoControl2);
            this.splitContainer1.Size = new System.Drawing.Size(536, 214);
            this.splitContainer1.SplitterDistance = 254;
            this.splitContainer1.TabIndex = 0;
            // 
            // queryInfoControl1
            // 
            this.queryInfoControl1.Caption = "Caption";
            this.queryInfoControl1.ConnectionString = "";
            this.queryInfoControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryInfoControl1.Location = new System.Drawing.Point(0, 0);
            this.queryInfoControl1.Name = "queryInfoControl1";
            this.queryInfoControl1.Query = "";
            this.queryInfoControl1.Size = new System.Drawing.Size(254, 214);
            this.queryInfoControl1.TabIndex = 2;
            // 
            // queryInfoControl2
            // 
            this.queryInfoControl2.Caption = "Caption";
            this.queryInfoControl2.ConnectionString = "";
            this.queryInfoControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryInfoControl2.Location = new System.Drawing.Point(0, 0);
            this.queryInfoControl2.Name = "queryInfoControl2";
            this.queryInfoControl2.Query = "";
            this.queryInfoControl2.Size = new System.Drawing.Size(278, 214);
            this.queryInfoControl2.TabIndex = 4;
            // 
            // comparisonControl1
            // 
            this.comparisonControl1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.comparisonControl1.ConnectionString1 = null;
            this.comparisonControl1.ConnectionString2 = null;
            this.comparisonControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comparisonControl1.Location = new System.Drawing.Point(0, 0);
            this.comparisonControl1.Name = "comparisonControl1";
            this.comparisonControl1.NumberOfColumnMismatchesAllowed = 0;
            this.comparisonControl1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.comparisonControl1.Query1 = null;
            this.comparisonControl1.Query2 = null;
            this.comparisonControl1.Size = new System.Drawing.Size(793, 375);
            this.comparisonControl1.TabIndex = 0;
            this.comparisonControl1.UseAdvancedCompare = false;
            this.comparisonControl1.CompareCompleted += new SQLComparator.ComparisonControl.CompareCompletedEventHandler(this.CompareCompleted);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.queryToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(797, 24);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveOutputToolStripMenuItem,
            this.toolStripSeparator2,
            this.printScreenshotToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.loadToolStripMenuItem.Text = "&Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.saveAsToolStripMenuItem.Text = "Save As ...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(186, 6);
            // 
            // printScreenshotToolStripMenuItem
            // 
            this.printScreenshotToolStripMenuItem.Name = "printScreenshotToolStripMenuItem";
            this.printScreenshotToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.printScreenshotToolStripMenuItem.Text = "&Print Screenshot (F10)";
            this.printScreenshotToolStripMenuItem.Click += new System.EventHandler(this.printScreenshotToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(186, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // queryToolStripMenuItem
            // 
            this.queryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeComparisonToolStripMenuItem,
            this.toolStripSeparator3,
            this.buildConnectionString1ToolStripMenuItem,
            this.buildConnectionString2ToolStripMenuItem,
            this.buildBothConnectionStringsToolStripMenuItem,
            this.toolStripSeparator4,
            this.useAdvancedCompareToolStripMenuItem});
            this.queryToolStripMenuItem.Name = "queryToolStripMenuItem";
            this.queryToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.queryToolStripMenuItem.Text = "&Query";
            // 
            // executeComparisonToolStripMenuItem
            // 
            this.executeComparisonToolStripMenuItem.Name = "executeComparisonToolStripMenuItem";
            this.executeComparisonToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.executeComparisonToolStripMenuItem.Text = "&Execute (F5)";
            this.executeComparisonToolStripMenuItem.Click += new System.EventHandler(this.executeComparisonToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(227, 6);
            // 
            // buildConnectionString1ToolStripMenuItem
            // 
            this.buildConnectionString1ToolStripMenuItem.Name = "buildConnectionString1ToolStripMenuItem";
            this.buildConnectionString1ToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.buildConnectionString1ToolStripMenuItem.Text = "Build connection string 1";
            this.buildConnectionString1ToolStripMenuItem.Click += new System.EventHandler(this.buildConnectionString1ToolStripMenuItem_Click);
            // 
            // buildConnectionString2ToolStripMenuItem
            // 
            this.buildConnectionString2ToolStripMenuItem.Name = "buildConnectionString2ToolStripMenuItem";
            this.buildConnectionString2ToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.buildConnectionString2ToolStripMenuItem.Text = "Build connection string 2";
            this.buildConnectionString2ToolStripMenuItem.Click += new System.EventHandler(this.buildConnectionString2ToolStripMenuItem_Click);
            // 
            // buildBothConnectionStringsToolStripMenuItem
            // 
            this.buildBothConnectionStringsToolStripMenuItem.Name = "buildBothConnectionStringsToolStripMenuItem";
            this.buildBothConnectionStringsToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.buildBothConnectionStringsToolStripMenuItem.Text = "Build both connection strings";
            this.buildBothConnectionStringsToolStripMenuItem.Click += new System.EventHandler(this.buildBothConnectionStringsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(227, 6);
            // 
            // useAdvancedCompareToolStripMenuItem
            // 
            this.useAdvancedCompareToolStripMenuItem.Name = "useAdvancedCompareToolStripMenuItem";
            this.useAdvancedCompareToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.useAdvancedCompareToolStripMenuItem.Text = "Use advanced compare";
            this.useAdvancedCompareToolStripMenuItem.Click += new System.EventHandler(this.useAdvancedCompareToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.horizontalToolStripMenuItem,
            this.verticalToolStripMenuItem,
            this.toolStripSeparator5,
            this.showCommentsFieldToolStripMenuItem,
            this.toolStripSeparator6,
            this.helpToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // horizontalToolStripMenuItem
            // 
            this.horizontalToolStripMenuItem.Name = "horizontalToolStripMenuItem";
            this.horizontalToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.horizontalToolStripMenuItem.Text = "Horizontal";
            this.horizontalToolStripMenuItem.Click += new System.EventHandler(this.horizontalToolStripMenuItem_Click);
            // 
            // verticalToolStripMenuItem
            // 
            this.verticalToolStripMenuItem.Name = "verticalToolStripMenuItem";
            this.verticalToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.verticalToolStripMenuItem.Text = "Vertical";
            this.verticalToolStripMenuItem.Click += new System.EventHandler(this.verticalToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(205, 6);
            // 
            // showCommentsFieldToolStripMenuItem
            // 
            this.showCommentsFieldToolStripMenuItem.Name = "showCommentsFieldToolStripMenuItem";
            this.showCommentsFieldToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.showCommentsFieldToolStripMenuItem.Text = "Show comments field";
            this.showCommentsFieldToolStripMenuItem.Click += new System.EventHandler(this.showCommentsFieldToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(205, 6);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.helpToolStripMenuItem.Text = "About Database Validator";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.comparisonControl1);
            this.splitContainer2.Size = new System.Drawing.Size(797, 601);
            this.splitContainer2.SplitterDistance = 218;
            this.splitContainer2.TabIndex = 16;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.pnlQueryHolder);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer3.Panel2.Controls.Add(this.txtResult);
            this.splitContainer3.Panel2.Controls.Add(this.Label5);
            this.splitContainer3.Size = new System.Drawing.Size(797, 218);
            this.splitContainer3.SplitterDistance = 540;
            this.splitContainer3.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkUseAdvanceCompare);
            this.groupBox1.Controls.Add(this.txtMaxAllowedMismatches);
            this.groupBox1.Controls.Add(this.lblMaxAllowedMismatches);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(249, 78);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // chkUseAdvanceCompare
            // 
            this.chkUseAdvanceCompare.AutoSize = true;
            this.chkUseAdvanceCompare.Location = new System.Drawing.Point(9, 23);
            this.chkUseAdvanceCompare.Name = "chkUseAdvanceCompare";
            this.chkUseAdvanceCompare.Size = new System.Drawing.Size(153, 17);
            this.chkUseAdvanceCompare.TabIndex = 26;
            this.chkUseAdvanceCompare.Text = "Use advanced comparison";
            this.chkUseAdvanceCompare.UseVisualStyleBackColor = true;
            this.chkUseAdvanceCompare.CheckedChanged += new System.EventHandler(this.chkUseAdvanceCompare_CheckedChanged);
            // 
            // txtMaxAllowedMismatches
            // 
            this.txtMaxAllowedMismatches.Location = new System.Drawing.Point(69, 46);
            this.txtMaxAllowedMismatches.Name = "txtMaxAllowedMismatches";
            this.txtMaxAllowedMismatches.Size = new System.Drawing.Size(32, 20);
            this.txtMaxAllowedMismatches.TabIndex = 27;
            // 
            // lblMaxAllowedMismatches
            // 
            this.lblMaxAllowedMismatches.AutoSize = true;
            this.lblMaxAllowedMismatches.Location = new System.Drawing.Point(6, 50);
            this.lblMaxAllowedMismatches.Name = "lblMaxAllowedMismatches";
            this.lblMaxAllowedMismatches.Size = new System.Drawing.Size(159, 13);
            this.lblMaxAllowedMismatches.TabIndex = 28;
            this.lblMaxAllowedMismatches.Text = "Allow up to               mismatches";
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtResult.Location = new System.Drawing.Point(6, 23);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(237, 107);
            this.txtResult.TabIndex = 25;
            // 
            // Label5
            // 
            this.Label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.Label5.Location = new System.Drawing.Point(0, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(249, 23);
            this.Label5.TabIndex = 24;
            this.Label5.Text = "Comparison results";
            this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "SQL Comparator Files|*.sqc";
            // 
            // pnlComments
            // 
            this.pnlComments.Controls.Add(this.txtComments);
            this.pnlComments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlComments.Location = new System.Drawing.Point(0, 0);
            this.pnlComments.Name = "pnlComments";
            this.pnlComments.Size = new System.Drawing.Size(150, 59);
            this.pnlComments.TabIndex = 18;
            this.pnlComments.TabStop = false;
            this.pnlComments.Text = "Validation comments";
            // 
            // txtComments
            // 
            this.txtComments.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.txtComments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtComments.Location = new System.Drawing.Point(3, 16);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(144, 40);
            this.txtComments.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 24);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.pnlComments);
            this.splitContainer4.Panel1Collapsed = true;
            this.splitContainer4.Panel1MinSize = 0;
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer4.Size = new System.Drawing.Size(797, 601);
            this.splitContainer4.SplitterDistance = 59;
            this.splitContainer4.TabIndex = 19;
            // 
            // saveOutputToolStripMenuItem
            // 
            this.saveOutputToolStripMenuItem.Name = "saveOutputToolStripMenuItem";
            this.saveOutputToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.saveOutputToolStripMenuItem.Text = "Save Output";
            this.saveOutputToolStripMenuItem.Click += new System.EventHandler(this.saveOutputToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 625);
            this.Controls.Add(this.splitContainer4);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "SQL Comparator";
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.pnlQueryHolder.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlComments.ResumeLayout(false);
            this.pnlComments.PerformLayout();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.Panel pnlQueryHolder;
        private ComparisonControl comparisonControl1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem printScreenshotToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem verticalToolStripMenuItem;
        private ToolStripMenuItem horizontalToolStripMenuItem;
        private ToolStripMenuItem queryToolStripMenuItem;
        private ToolStripMenuItem buildConnectionString1ToolStripMenuItem;
        private ToolStripMenuItem buildConnectionString2ToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem buildBothConnectionStringsToolStripMenuItem;
        private ToolStripMenuItem executeComparisonToolStripMenuItem;
        private SplitContainer splitContainer1;
        private QueryInfoControl queryInfoControl1;
        private QueryInfoControl queryInfoControl2;
        private SplitContainer splitContainer2;
        private SplitContainer splitContainer3;
        internal TextBox txtResult;
        internal Label Label5;
        private Label lblMaxAllowedMismatches;
        private TextBox txtMaxAllowedMismatches;
        private CheckBox chkUseAdvanceCompare;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem useAdvancedCompareToolStripMenuItem;
        private GroupBox groupBox1;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem helpToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem showCommentsFieldToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private GroupBox pnlComments;
        private TextBox txtComments;
        private SplitContainer splitContainer4;
        private ToolStripMenuItem saveOutputToolStripMenuItem;
	}

} //end of root namespace