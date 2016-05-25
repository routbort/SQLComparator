using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SQLComparator
{
	public partial class Info
	{

		private void Info_Load(object sender, System.EventArgs e)
		{
			string ns = typeof(Info).Namespace;
			System.IO.Stream rs = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(ns + "." + "Info.rtf");
			this.RichTextBox1.Rtf = new System.IO.StreamReader(rs).ReadToEnd();

		}
	}
} //end of root namespace