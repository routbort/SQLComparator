using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;

namespace SQLComparator
{
    public partial class Main
    {

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetAsyncKeyState", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern short GetAsyncKeyState(Int32 vKey);
        private string _CurrentFilename = null;
        private const string APP_NAME = "SQL Comparator";

        public void CompareCompleted(object sender, GridComparisonResult Result)
        {
            this.txtResult.Text = Result.Message;
        }

        private void Compare()
        {
            ComparisonControl cc = this.comparisonControl1;
            cc.UseAdvancedCompare = this.chkUseAdvanceCompare.Checked;
            if (cc.UseAdvancedCompare)
            {
                int MismatchesAllowed;
                if (Int32.TryParse(this.txtMaxAllowedMismatches.Text, out MismatchesAllowed))
                {
                    this.comparisonControl1.NumberOfColumnMismatchesAllowed = MismatchesAllowed;
                }
                else
                {
                    MessageBox.Show("Non-numeric value invalid in Max Allowed Mismatches field");
                    this.txtMaxAllowedMismatches.Focus();
                    return;
                }
            }

            cc.ConnectionString1 = this.queryInfoControl1.ConnectionString;
            cc.ConnectionString2 = this.queryInfoControl2.ConnectionString;
            cc.Query1 = this.queryInfoControl1.Query;
            cc.Query2 = this.queryInfoControl2.Query;
            SetGUI();
            cc.Compare();
        }

        private void cmdInfo_Click(object sender, System.EventArgs e)
        {
            Info infoForm = new Info();
            infoForm.Show();
        }

        private void SetName()
        {
            string Filename = System.IO.Path.GetFileNameWithoutExtension(this._CurrentFilename);
            this.Text = (Filename == null ? "" : Filename + " - ") + APP_NAME + "   (Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")";
        }

        private void SetGUI()
        {
            ComparisonControl cc = this.comparisonControl1;
            this.queryInfoControl1.Caption = ((cc.Orientation == Orientation.Horizontal) ? "Top" : "Left") + " Query";
            this.queryInfoControl2.Caption = ((cc.Orientation == Orientation.Horizontal) ? "Bottom" : "Right") + " Query";
            this.verticalToolStripMenuItem.Checked = (this.comparisonControl1.Orientation == Orientation.Vertical);
            this.horizontalToolStripMenuItem.Checked = !this.verticalToolStripMenuItem.Checked;
            this.useAdvancedCompareToolStripMenuItem.Checked = cc.UseAdvancedCompare;
            this.chkUseAdvanceCompare.Checked = cc.UseAdvancedCompare;
            this.lblMaxAllowedMismatches.Visible = cc.UseAdvancedCompare;
            this.txtMaxAllowedMismatches.Visible = cc.UseAdvancedCompare;
            this.showCommentsFieldToolStripMenuItem.Checked = !this.splitContainer4.Panel1Collapsed;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            SetGUI();
            SetName();

            if (this.Tag != null)
            {
                string[] Args = (string[])this.Tag;
                this.Tag = null;
                if (Args.Length == 1)
                    LoadSettings(Args[0]);
            }

            this.queryInfoControl1.Splitter.SplitterMoved += new SplitterEventHandler(Splitter_SplitterMoved);
            this.queryInfoControl2.Splitter.SplitterMoved += new SplitterEventHandler(Splitter_SplitterMoved);
            this.queryInfoControl1.Splitter.SplitterMoving += new SplitterCancelEventHandler(Splitter_SplitterMoving);
            this.queryInfoControl2.Splitter.SplitterMoving += new SplitterCancelEventHandler(Splitter_SplitterMoving);
            //    this.comparisonControl1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        void Splitter_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            _QueryInfoControlSplitterMoving = true;
        }

        bool _QueryInfoControlSplitterMoved = false;
        bool _QueryInfoControlSplitterMoving = false;

        void Splitter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!_QueryInfoControlSplitterMoving || _QueryInfoControlSplitterMoved)
                return;
            _QueryInfoControlSplitterMoving = false;
            _QueryInfoControlSplitterMoved = true;
            SplitContainer splitterPrimary = (sender == this.queryInfoControl1.Splitter ? this.queryInfoControl1.Splitter : this.queryInfoControl2.Splitter);
            SplitContainer splitterSecondary = (sender == this.queryInfoControl1.Splitter ? this.queryInfoControl2.Splitter : this.queryInfoControl1.Splitter);
            splitterSecondary.SplitterDistance = splitterPrimary.SplitterDistance;
            _QueryInfoControlSplitterMoved = false;
        }


        private void cmdPrint_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        private string GetConnectionString()
        {

            try
            {

                MSDASC.DataLinksClass DataLink = new MSDASC.DataLinksClass();
                object connection = new ADODB.ConnectionClass();
                if (DataLink.PromptEdit(ref connection))
                {
                    return (connection as ADODB.ConnectionClass).ConnectionString;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error attempting to load Windows data link wizard - possibly you do not have the correct version of MDAC installed." + System.Environment.NewLine + ex.Message);
            }
            return null;

        }

        private void SaveSettings(string filename)
        {
            ComparisonInfo ci = new ComparisonInfo();
            ci.Query1 = this.queryInfoControl1.Query;
            ci.Query2 = this.queryInfoControl2.Query;
            ci.ConnectionString1 = this.queryInfoControl1.ConnectionString;
            ci.ConnectionString2 = this.queryInfoControl2.ConnectionString;
            ci.UseAdvancedCompare = this.chkUseAdvanceCompare.Checked;
            if (this.comparisonControl1.UseAdvancedCompare)
            {
                int MismatchesAllowed;
                if (Int32.TryParse(this.txtMaxAllowedMismatches.Text, out MismatchesAllowed))
                {
                    ci.MaxAllowedMismatches = MismatchesAllowed;
                }
                else
                {
                    MessageBox.Show("Non-numeric value invalid in Max Allowed Mismatches field. Save failed.");
                    return;
                }
            }

            FileStream fs = new FileStream(filename, FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();


            try
            {
                formatter.Serialize(fs, ci);
                this._CurrentFilename = filename;
                SetName();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save. Reason: " + ex.Message);
            }
            finally
            {
                fs.Close();
            }



            XmlSerializer SerializerObj = new XmlSerializer(typeof(ComparisonInfo));

            // Create a new file stream to write the serialized object to a file
            TextWriter WriteFileStream = new StreamWriter(filename + ".xml");
            SerializerObj.Serialize(WriteFileStream, ci);









        }



        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_CurrentFilename != null)
                SaveSettings(_CurrentFilename);
            else
                SaveAs();
        }

        private void SaveAs()
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                SaveSettings(filename);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }




        internal void LoadSettings(string filename)
        {
            this.comparisonControl1.Abort();
            ComparisonInfo ci;
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                ci = (ComparisonInfo)formatter.Deserialize(fs);
                this.queryInfoControl1.Query = ci.Query1;
                this.queryInfoControl2.Query = ci.Query2;
                this.queryInfoControl1.ConnectionString = ci.ConnectionString1;
                this.queryInfoControl2.ConnectionString = ci.ConnectionString2;
                this.chkUseAdvanceCompare.Checked = ci.UseAdvancedCompare;
                if (ci.UseAdvancedCompare)
                    this.txtMaxAllowedMismatches.Text = ci.MaxAllowedMismatches.ToString();
                this._CurrentFilename = filename;
                SetName();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open file " + filename + System.Environment.NewLine + "Reason: " + ex.Message);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

        }



        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";


            openFileDialog1.Filter = "SQL Comparator files|*.sqc";

            //         openFileDialog1.Filter = "SQL Comparator files|*.sqc,*.xml";

            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                LoadSettings(filename);
            }
        }

        private void printScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.comparisonControl1.Orientation = Orientation.Vertical;
            SetGUI();
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.comparisonControl1.Orientation = Orientation.Horizontal;
            SetGUI();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buildConnectionString1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ConnectionString = GetConnectionString();
            if (ConnectionString != null)
                this.queryInfoControl1.ConnectionString = ConnectionString;
        }

        private void buildConnectionString2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ConnectionString = GetConnectionString();
            if (ConnectionString != null)
                this.queryInfoControl2.ConnectionString = ConnectionString;
        }

        private void buildBothConnectionStringsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ConnectionString = GetConnectionString();
            if (ConnectionString != null)
            {
                this.queryInfoControl1.ConnectionString = ConnectionString;
                this.queryInfoControl2.ConnectionString = ConnectionString;
            }
        }

        private void executeComparisonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Compare();
        }

        private void chkUseAdvanceCompare_CheckedChanged(object sender, EventArgs e)
        {
            this.comparisonControl1.UseAdvancedCompare = this.chkUseAdvanceCompare.Checked;
            SetGUI();
        }

        private void useAdvancedCompareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.comparisonControl1.UseAdvancedCompare = !this.useAdvancedCompareToolStripMenuItem.Checked;
            SetGUI();
        }

        Info _InfoForm = null;

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_InfoForm == null || _InfoForm.IsDisposed)
            {
                _InfoForm = new Info();
                _InfoForm.StartPosition = FormStartPosition.CenterScreen;
            }
            _InfoForm.Show();

        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                Compare();
                e.Handled = true;
            }

            if (e.KeyCode == Keys.F10)
            {
                this.Print();
            }
        }

        private void showCommentsFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer4.Panel1Collapsed = (!this.splitContainer4.Panel1Collapsed);
            // this.pnlComments.Visible = (!this.pnlComments.Visible);
            SetGUI();
        }

        private void saveOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".xls"; // Default file extension
            dlg.Filter = "Tab separated file (.xls)|*.xls"; // Filter files by extension

            // Show save file dialog box
            DialogResult result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == System.Windows.Forms.DialogResult.OK)
            {

                string filename = dlg.FileName;
                File.WriteAllText(filename, this.comparisonControl1.GetTSV());
            }

        }




    }
} //end of root namespace