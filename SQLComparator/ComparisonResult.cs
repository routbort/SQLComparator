using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SQLComparator
{
	public class ComparisonResult
	{
		public bool Equal;
		public string Message;

		public ComparisonResult(bool Equal, string Message)
		{
			this.Equal = Equal;
			this.Message = Message;
		}
	}

} //end of root namespace