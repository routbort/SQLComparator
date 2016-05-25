using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SQLComparator
{
	public class SQLConnectionInfo
	{

		//Class to facilitate connection string information sharing

		public string _Server;
		public string _Database;
		public string _Username = "";
		public string _Password = "";
		private string _ConnectionString;
		private bool _IsTrusted;

		public SQLConnectionInfo(string Server, string Database)
		{
			this._Server = Server;
			this._Database = Database;
			_ConnectionString = "Server=" + Server + ";Database=" + Database + ";Trusted_Connection=True;";
			_IsTrusted = true;
		}

		public SQLConnectionInfo(string Server, string Database, string Username, string Password)
		{

			this._Server = Server;
			this._Database = Database;
			this._Username = Username;
			this._Password = Password;
			_ConnectionString = "Server=" + Server + ";Database=" + Database + ";UID=" + Username + ";Password=" + Password;
			_IsTrusted = false;

		}

		public bool TrustedConnection
		{
			get
			{
				return _IsTrusted;
			}
		}

		public string ConnectionString
		{
			get
			{
				return _ConnectionString;
			}
		}

	}

} //end of root namespace