using System;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.OracleClient;
namespace FIADB.Oracle
{
	/// <summary>
	/// Summary description for Oracle.
	/// </summary>
	public class DataMgr
	{
		public System.Data.DataSet m_DataSet;
		public System.Data.OracleClient.OracleDataAdapter m_DataAdapter;
		public System.Data.OracleClient.OracleCommand m_Command;
		public System.Data.OracleClient.OracleConnection m_Connection;
		public System.Data.OracleClient.OracleDataReader m_DataReader;
		public System.Data.DataTable m_DataTable;
		public System.Data.OracleClient.OracleTransaction m_Transaction;
		public string m_strError;
		public int m_intError;
		public string m_strSQL;
		public string m_strTable;
		private bool _bDisplayErrors=true;
		private string _strMsgBoxTitle="Boundary Viewer";


		public DataMgr()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		~DataMgr()
		{

		}
		public void OpenConnection(string strConn)
		{
			System.Data.OracleClient.OracleConnection p_Connection = new System.Data.OracleClient.OracleConnection();
			p_Connection.ConnectionString = strConn;
			try
			{
				p_Connection.Open();
			}
			catch (Exception caught)
			{
				this.m_strError = caught.Message;
				if (m_strError.IndexOf("ORA-03134",0) >=0 || 
					m_strError.IndexOf("ORA-28273",0) >=0) m_intError=-2;
				else this.m_intError = -1;
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:OpenConnection  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				return;
			}
			this.m_Connection = p_Connection;
		}
		public void OpenConnection(string strConn, ref System.Data.OracleClient.OracleConnection p_Connection)
		{
            this.m_intError=0;
			this.m_strError="";
			try
			{
			    p_Connection.ConnectionString = strConn;
			
			
				p_Connection.Open();
				
			}
			catch (Exception caught)
			{
				this.m_strError = caught.Message;
				this.m_intError = -1;
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:OpenConnection  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
			}
		}

		public void SqlNonQuery(string strConn, string strSQL)
		{
            System.Data.OracleClient.OracleConnection p_Connection = new System.Data.OracleClient.OracleConnection();
		    this.OpenConnection(strConn, ref p_Connection);
			if (this.m_intError == 0)
			{
                System.Data.OracleClient.OracleCommand p_Command = new System.Data.OracleClient.OracleCommand();
				p_Command.Connection = p_Connection;
				p_Command.CommandText = strSQL;
				try
				{
					p_Command.ExecuteNonQuery();
				}
				catch (System.Threading.ThreadInterruptedException err)
				{
				
				}
				catch (System.Threading.ThreadAbortException err)
				{
				}
				catch (Exception caught)
				{
					this.m_strError = caught.Message + " The SQL command " + strSQL + " Failed";;
					this.m_intError = -1;
					if (_bDisplayErrors)
					MessageBox.Show("!!Error!! \n" + 
						"Module - Oracle:SqlNonQuery  \n" + 
						"Err Msg - " + this.m_strError,
						"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
						System.Windows.Forms.MessageBoxIcon.Exclamation);
    			}
				p_Connection.Close();
				p_Connection = null;
				p_Command = null;

			}
		}
		public void SqlNonQuery(System.Data.OracleClient.OracleConnection p_Connection, string strSQL)
		{
				System.Data.OracleClient.OracleCommand p_Command = new System.Data.OracleClient.OracleCommand();
			    p_Command.Connection = p_Connection;
				p_Command.CommandText = strSQL;
			    
				try
				{
					p_Command.ExecuteNonQuery();
				}
				catch (System.Threading.ThreadInterruptedException err)
				{
				
				}
				catch (System.Threading.ThreadAbortException err)
				{
				}
				catch (Exception caught)
				{
					this.m_strError = caught.Message + " The SQL command " + strSQL + " Failed";;
					this.m_intError = -1;
					if (_bDisplayErrors)
					MessageBox.Show("!!Error!! \n" + 
						"Module - Oracle:SqlNonQuery  \n" + 
						"Err Msg - " + this.m_strError,
						"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
						System.Windows.Forms.MessageBoxIcon.Exclamation);
				}
				p_Command = null;

			
		}

		public void SqlQueryReader(string strConn,string strSql)
		{
			this.m_intError=0;
			this.m_strError="";
            this.OpenConnection(strConn);
			if (this.m_intError==0)
			{
			    this.m_Command = this.m_Connection.CreateCommand();
				this.m_Command.CommandText = strSql;
				try
				{
					this.m_DataReader = this.m_Command.ExecuteReader();
				}
				catch (System.Threading.ThreadInterruptedException err)
				{
				
				}
				catch (System.Threading.ThreadAbortException err)
				{
				}
				catch (Exception caught)
				{
					this.m_intError = -1;
					this.m_strError = caught.Message + " The Query Command " + this.m_Command.CommandText.ToString() + " Failed";
					if (_bDisplayErrors)
					MessageBox.Show("!!Error!! \n" + 
						"Module - Oracle:SqlQueryReader  \n" + 
						"Err Msg - " + this.m_strError,
						"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
						System.Windows.Forms.MessageBoxIcon.Exclamation);
					this.m_DataReader = null;
                    this.m_Command = null;
					this.m_Connection.Close();
					this.m_Connection = null;
					return;
				}
			}
		}
		public void SqlQueryReader(System.Data.OracleClient.OracleConnection p_Connection,string strSql)
		{
			this.m_intError=0;
			this.m_strError="";
			
				this.m_Command = p_Connection.CreateCommand();
				this.m_Command.CommandText = strSql;
				try
				{
					this.m_DataReader = this.m_Command.ExecuteReader();
				}
				catch (System.Threading.ThreadInterruptedException err)
				{
				
				}
				catch (System.Threading.ThreadAbortException err)
				{
				}
				catch (Exception caught)
				{
					this.m_intError = -1;
					this.m_strError = caught.Message + " The Query Command " + this.m_Command.CommandText.ToString() + " Failed";
					if (_bDisplayErrors)
					MessageBox.Show("!!Error!! \n" + 
						"Module - Oracle:SqlQueryReader  \n" + 
						"Err Msg - " + this.m_strError,
						"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
						System.Windows.Forms.MessageBoxIcon.Exclamation);
					this.m_DataReader = null;
					this.m_Command = null;
					return;
				}
			    
			
		}
		public void SqlQueryReader(System.Data.OracleClient.OracleConnection p_Connection,System.Data.OracleClient.OracleTransaction p_trans,string strSql)
		{
			this.m_intError=0;
			this.m_strError="";
			
			this.m_Command = p_Connection.CreateCommand();
			this.m_Command.CommandText = strSql;
            this.m_Command.Transaction = p_trans;
			try
			{
				this.m_DataReader = this.m_Command.ExecuteReader();
			}
			catch (System.Threading.ThreadInterruptedException err)
			{
				
			}
			catch (System.Threading.ThreadAbortException err)
			{
			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + " The Query Command " + this.m_Command.CommandText.ToString() + " Failed";
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:SqlQueryReader  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				this.m_DataReader = null;
				this.m_Command = null;
				return;
			}
		}
		public bool FieldExist(System.Data.OracleClient.OracleConnection p_oConn, string p_strSql, string p_strField)
		{
			string strDelimiter=",";
			string strList = getFieldNames(p_oConn,p_strSql);
			string[] strArray = strList.Split(strDelimiter.ToCharArray());
			for (int x=0;x<=strArray.Length-1;x++)
			{
				if (strArray[x] != null && strArray[x].Trim().Length > 0)
				{
					if (strArray[x].Trim().ToUpper()==p_strField.Trim().ToUpper()) return true;
				}
			}
			return false;

		}
		public bool FieldExist(string p_strConn, string p_strSql, string p_strField)
		{
			string strDelimiter=",";
			this.OpenConnection(p_strConn);
			if (this.m_intError==0)
			{
				string strList = getFieldNames(this.m_Connection,p_strSql);
				string[] strArray = strList.Split(strDelimiter.ToCharArray());
				for (int x=0;x<=strArray.Length-1;x++)
				{
					if (strArray[x] != null && strArray[x].Trim().Length > 0)
					{
						if (strArray[x].Trim().ToUpper()==p_strField.Trim().ToUpper())
						{
							this.m_Connection.Close();
							return true;
						}
					}
				}
				this.m_Connection.Close();

			}
			return false;

		}

		public string getFieldNames(System.Data.OracleClient.OracleConnection p_oConn,string p_strSql)
		{
			this.m_intError=0;
			System.Data.DataTable oTableSchema = this.getTableSchema(p_oConn,p_strSql);
			if (this.m_intError !=0) return "";
			string strFields="";
			
			for (int x=0; x<=oTableSchema.Rows.Count-1;x++)
			{
				strFields = strFields + oTableSchema.Rows[x]["columnname"].ToString().Trim() + ",";
			}
			if (strFields.Trim().Length > 0) strFields=strFields.Substring(0,strFields.Trim().Length -1);

			return strFields;
			
		}
		public string getFieldNames(string p_strConn,string p_strSql)
		{
			string strFields="";
			this.m_intError=0;
			this.OpenConnection(p_strConn);
			if (this.m_intError==0)
			{
				System.Data.DataTable oTableSchema = this.getTableSchema(this.m_Connection,p_strSql);
				if (this.m_intError !=0) return "";
				
			
				for (int x=0; x<=oTableSchema.Rows.Count-1;x++)
				{
					strFields = strFields + oTableSchema.Rows[x]["columnname"].ToString().Trim() + ",";
				}
				if (strFields.Trim().Length > 0) strFields=strFields.Substring(0,strFields.Trim().Length -1);

				
			}
			return strFields;
			
		}

		public System.Data.DataTable getTableSchema(System.Data.OracleClient.OracleConnection p_Connection,string strSql)
		{
			System.Data.DataTable p_dt;
			this.m_intError=0;
			this.m_strError="";
			
			this.m_Command = p_Connection.CreateCommand();
			this.m_Command.CommandText = strSql;
			try
			{
				this.m_DataReader = this.m_Command.ExecuteReader(CommandBehavior.KeyInfo);
				p_dt = this.m_DataReader.GetSchemaTable();
			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + " The Query Command " + this.m_Command.CommandText.ToString() + " Failed";
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:getTableSchema  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				this.m_DataReader = null;
				this.m_Command = null;
				return null;
			}
			this.m_DataReader.Close();
			return p_dt;
			
		}
		public System.Data.DataTable getTableSchema(System.Data.OracleClient.OracleConnection p_Connection,
			                                        System.Data.OracleClient.OracleTransaction p_trans,
			                                        string strSql)
		{
			System.Data.DataTable p_dt;
			this.m_intError=0;
			this.m_strError="";
			
			this.m_Command = p_Connection.CreateCommand();
			this.m_Command.CommandText = strSql;
			this.m_Command.Transaction = p_trans;
			try
			{
				this.m_DataReader = this.m_Command.ExecuteReader(CommandBehavior.KeyInfo);
				p_dt = this.m_DataReader.GetSchemaTable();
			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + " The Query Command " + this.m_Command.CommandText.ToString() + " Failed";
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:getTableSchema  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				this.m_DataReader = null;
				this.m_Command = null;
				return null;
			}
			this.m_DataReader.Close();
			return p_dt;
			
		}

		public System.Data.DataTable getTableSchema(string strConn,string strSql)
		{
			System.Data.DataTable p_dt;
			this.m_intError=0;
			this.m_strError="";
			
			this.OpenConnection(strConn);
			if (this.m_intError==0)
			{
				this.m_Command = this.m_Connection.CreateCommand();
				this.m_Command.CommandText = strSql;
				try
				{
					this.m_DataReader = this.m_Command.ExecuteReader(CommandBehavior.KeyInfo);
					p_dt = this.m_DataReader.GetSchemaTable();
				}
				catch (Exception caught)
				{
					this.m_intError = -1;
					this.m_strError = caught.Message + " The Query Command " + this.m_Command.CommandText.ToString() + " Failed";
					if (_bDisplayErrors)
					MessageBox.Show("!!Error!! \n" + 
						"Module - Oracle:getTableSchema  \n" + 
						"Err Msg - " + this.m_strError,
						"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
						System.Windows.Forms.MessageBoxIcon.Exclamation);
					this.m_DataReader = null;
					this.m_Command = null;
					return null;
				}
				this.m_DataReader.Close();
				return p_dt;
			}
			return null;
			
			
		}
		public void getTableSchema2(string strConn,string strSql)
		{
			this.m_intError=0;
			this.m_strError="";
			if (this.m_DataTable != null) this.m_DataTable.Clear();
			
			this.OpenConnection(strConn);
			if (this.m_intError==0)
			{
				this.m_Command = this.m_Connection.CreateCommand();
				this.m_Command.CommandText = strSql;
				try
				{
					this.m_DataReader = this.m_Command.ExecuteReader(CommandBehavior.KeyInfo);
					this.m_DataTable = this.m_DataReader.GetSchemaTable();
				}
				catch (Exception caught)
				{
					this.m_intError = -1;
					this.m_strError = caught.Message + " The Query Command " + this.m_Command.CommandText.ToString() + " Failed";
					if (_bDisplayErrors)
						MessageBox.Show("!!Error!! \n" + 
							"Module - Oracle:getTableSchema  \n" + 
							"Err Msg - " + this.m_strError,
							"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
							System.Windows.Forms.MessageBoxIcon.Exclamation);
					this.m_DataReader = null;
					this.m_Command = null;
				}
				this.m_DataReader.Close();
			}
		}
		public void getTableSchema2(System.Data.OracleClient.OracleConnection p_Connection,string strSql)
		{
			this.m_intError=0;
			this.m_strError="";
			if (this.m_DataTable != null) this.m_DataTable.Clear();
			
			if (this.m_intError==0)
			{
				this.m_Command = p_Connection.CreateCommand();
				this.m_Command.CommandText = strSql;
				try
				{
					this.m_DataReader = this.m_Command.ExecuteReader(CommandBehavior.KeyInfo);
					this.m_DataTable = this.m_DataReader.GetSchemaTable();
				}
				catch (Exception caught)
				{
					this.m_intError = -1;
					this.m_strError = caught.Message + " The Query Command " + this.m_Command.CommandText.ToString() + " Failed";
					if (_bDisplayErrors)
						MessageBox.Show("!!Error!! \n" + 
							"Module - Oracle:getTableSchema  \n" + 
							"Err Msg - " + this.m_strError,
							"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
							System.Windows.Forms.MessageBoxIcon.Exclamation);
					this.m_DataReader = null;
					this.m_Command = null;
				}
				this.m_DataReader.Close();
			}
		}
		/****
		 **** format strings to be used in an sql statement
		 ****/
		public string FixString(string SourceString , string StringToReplace, string StringReplacement)
		{
			SourceString = SourceString.Replace(StringToReplace, StringReplacement);
			return(SourceString);
		}
		//returns Y or N for whether the field is a string or not
		public string getIsTheFieldAStringDataType(string strFieldType)
		{
			switch (strFieldType.Trim())
			{
				case "System.Single":
					return "N";
				case "System.Double":
					return "N";
				case "System.Decimal":
					return "N";
				case "System.String":
					return "Y";
				case "System.Int16":
					return "N";
				case "System.Char":
					return "Y";
				case "System.Int32":
					return "N";
				case "System.DateTime":
					return "Y";
				case "System.DayOfWeek":
					return "Y";
				case "System.Int64":
					return "N";
				case "System.Byte":
					return "N";
				case "System.Boolean":
					return "N";
				default:
					//return "N";
					MessageBox.Show(strFieldType + " is undefined");
					return "N";
			}


		}
        public string FormatCreateTableSqlFieldItemForMSAccess(System.Data.DataRow p_oRow)
        {
            string strLine;
            string strColumn = p_oRow["ColumnName"].ToString().Trim();
            string strDataType = p_oRow["DataType"].ToString().Trim();
            string strPrecision = "";
            string strScale = "";
            string strSize = "";

            if (p_oRow["ColumnSize"] != null)
               strSize = Convert.ToString(p_oRow["ColumnSize"]);

            if (p_oRow["NumericPrecision"] != null)
                strPrecision = Convert.ToString(p_oRow["NumericPrecision"]);

            if (p_oRow["NumericScale"] != null)
                strScale = Convert.ToString(p_oRow["NumericScale"]);

            if (strColumn.Trim().ToUpper() == "VALUE" ||
                strColumn.Trim().ToUpper() == "USE" ||
                strColumn.Trim().ToUpper() == "YEAR")
                strColumn = "`" + strColumn.Trim() + "`";

            
            switch (strDataType)
            {

                case "System.Single":
                    strDataType = "single";
                    break;
                case "System.Double":
                    strDataType = "double";
                    break;
                case "System.Decimal":
                    strDataType = "decimal";
                    break;
                case "System.String":
                    strDataType = "text";
                    break;
                case "System.Int16":
                    strDataType = "short";
                    break;
                case "System.Char":
                    strDataType = "text";
                    break;
                case "System.Int32":
                    strDataType = "integer";
                    break;
                case "System.DateTime":
                    strDataType = "datetime";
                    break;
                case "System.DayOfWeek":
                    break;
                case "System.Int64":
                    break;
                case "System.Byte":
                    strDataType="byte";
                    break;
                case "System.Boolean":
                    break;



            }

            strLine = strColumn + " " + strDataType;

            if (strSize.Trim().Length > 0 && strDataType == "text")
                if (Convert.ToInt32(strSize) < 256)
                    strLine = strLine + " (" + strSize + ")";
                else
                {
                    strLine = strColumn + " memo";
                }
            else
            {
                if (strDataType == "decimal")
                {
                    if (strPrecision.Trim() == "0")
                        strLine = strColumn + " double";
                    else 
                        strLine = strLine + " (" + strPrecision + "," + strScale + ")";
                }
                
                    
            }
            return strLine;

        }

        public string FormatSelectSqlFieldItemForMSAccess(System.Data.DataRow p_oRow)
        {
            string strLine="";
            string strColumn = p_oRow["ColumnName"].ToString().Trim();
            string strDataType = p_oRow["DataType"].ToString().Trim();
            string strPrecision = "";
            string strScale = "";
            string strSize = "";

            if (p_oRow["ColumnSize"] != null)
                strSize = Convert.ToString(p_oRow["ColumnSize"]);

            if (p_oRow["NumericPrecision"] != null)
                strPrecision = Convert.ToString(p_oRow["NumericPrecision"]);

            if (p_oRow["NumericScale"] != null)
                strScale = Convert.ToString(p_oRow["NumericScale"]);

            //if (strColumn.Trim().ToUpper() == "VALUE" ||
            //    strColumn.Trim().ToUpper() == "USE")
            //    strColumn = "`" + strColumn.Trim() + "`";




            switch (strDataType)
            {

                case "System.Single":
                    strDataType = "single";
                    break;
                case "System.Double":
                    strDataType = "double";
                    break;
                case "System.Decimal":
                    strDataType = "decimal";
                    break;
                case "System.String":
                    strDataType = "text";
                    break;
                case "System.Int16":
                    strDataType = "short";
                    break;
                case "System.Char":
                    strDataType = "text";
                    break;
                case "System.Int32":
                    strDataType = "integer";
                    break;
                case "System.DateTime":
                    strDataType = "datetime";
                    break;
                case "System.DayOfWeek":
                    break;
                case "System.Int64":
                    break;
                case "System.Byte":
                    strDataType="byte";
                    break;
                case "System.Boolean":
                    break;



            }

            strLine = strColumn;

            if (strDataType == "decimal")
            {
                if (strPrecision.Trim() == "0")
                    strLine = "ROUND(" + strColumn + ",14) AS " + strColumn;
            }
            else if (strDataType == "double")
            {
                strLine = "ROUND(" + strColumn + ",14) AS " + strColumn;
            }
          

            return strLine;

        }

            
		public void CreateDataSet(string strConn,
			string strSQL,string strTableName)
		{
			this.m_intError=0;
			this.m_strError="";
			try
			{
				this.OpenConnection(strConn);
				if (this.m_intError == 0)
				{
					this.m_DataAdapter = new System.Data.OracleClient.OracleDataAdapter(strSQL, this.m_Connection);
					this.m_DataSet = new DataSet();
					this.m_DataAdapter.Fill(this.m_DataSet,strTableName);
					this.m_Connection.Close();
				}

			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + " : SQL query command " + strSQL + " failed" ;
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:CreateDataSet  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				this.m_Connection.Close();
				this.m_DataAdapter = null;
				this.m_DataSet  = null;
				return;
			}
			
		}

		public void CreateDataSet(System.Data.OracleClient.OracleConnection p_conn,
			string strSQL,string strTableName)
		{
			this.m_intError=0;
			this.m_strError="";
			try
			{
					this.m_DataAdapter = new System.Data.OracleClient.OracleDataAdapter(strSQL, p_conn);
					this.m_DataSet = new DataSet();
					this.m_DataAdapter.Fill(this.m_DataSet,strTableName);
					//this.m_Connection.Close();
			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + " : SQL query command " + strSQL + " failed" ;
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:CreateDataSet  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				//this.m_Connection.Close();
				this.m_DataAdapter = null;
				this.m_DataSet  = null;
				return;
			}
			
		}

		public void AddSQLQueryToDataSet(System.Data.OracleClient.OracleConnection p_conn,
			ref System.Data.OracleClient.OracleDataAdapter p_da,
			ref System.Data.DataSet p_ds,
			string strSQL, 
			string strTableName)
		{
			this.m_intError=0;
			this.m_strError="";
			System.Data.OracleClient.OracleCommand p_Command;
			try
			{
				p_Command = p_conn.CreateCommand();
				p_Command.CommandText = strSQL;
				p_da.SelectCommand = p_Command;
				p_da.Fill(p_ds,strTableName);
			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + " : SQL query command " + strSQL + " failed" ;
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:AddSQLQueryToDataSet  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				if (_bDisplayErrors)
				MessageBox.Show(this.m_strError);
			}

		}
		public void AddSQLQueryToDataSet(System.Data.OracleClient.OracleConnection p_conn,
			ref System.Data.OracleClient.OracleDataAdapter p_da,
			ref System.Data.DataSet p_ds,
			ref System.Data.OracleClient.OracleTransaction p_trans,
			string strSQL, 
			string strTableName)
		{
			this.m_intError=0;
			this.m_strError="";
			System.Data.OracleClient.OracleCommand p_Command;
			try
			{
				p_Command = p_conn.CreateCommand();
				p_Command.CommandText = strSQL;
				p_Command.Transaction = p_trans;
				p_da.SelectCommand = p_Command;
				p_da.Fill(p_ds,strTableName);
			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + " : SQL query command " + strSQL + " failed" ;
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:AddSQLQueryToDataSet  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				if (_bDisplayErrors)
				MessageBox.Show(this.m_strError);
			}

		}

		public void DatasetSQLInsertCommand(System.Data.OracleClient.OracleConnection p_conn,
			ref System.Data.OracleClient.OracleDataAdapter p_da,
			ref System.Data.DataSet p_ds,
			string strSQL, 
			string strTableName)
		{

		}

		public long getRecordCount(string strConn,
			string strSQL,string strTableName)
		{
			System.Data.OracleClient.OracleConnection p_Conn;
			System.Data.OracleClient.OracleCommand p_Command;
			long intRecTtl=0;
			this.m_intError=0;
			this.m_strError="";
			p_Conn = new System.Data.OracleClient.OracleConnection();
			try
			{
				this.OpenConnection(strConn, ref p_Conn);
				if (this.m_intError == 0)
				{
					p_Command = p_Conn.CreateCommand();
					p_Command.CommandText = strSQL;
					intRecTtl = Convert.ToInt32(p_Command.ExecuteScalar());
				}

			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + "  SQL query command: " + strSQL + " failed" ;
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:getRecordCount  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				p_Conn.Close();
				if (_bDisplayErrors) MessageBox.Show(this.m_strError);
			}
			try
			{
				p_Conn.Close();
			}
			catch 
			{
			}
			p_Conn = null;
			p_Command = null;
			return intRecTtl;
			
		}

		public long getRecordCount(System.Data.OracleClient.OracleConnection p_conn,
			string strSQL,string strTableName)
		{
			System.Data.OracleClient.OracleCommand p_Command;
			long intRecTtl=0;
			this.m_intError=0;
			this.m_strError="";
			try
			{
				
					p_Command = p_conn.CreateCommand();
					p_Command.CommandText = strSQL;
					intRecTtl = Convert.ToInt32(p_Command.ExecuteScalar());
				

			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + "  SQL query command: " + strSQL + " failed" ;
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:getRecordCount  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
			}
			p_Command = null;
			return intRecTtl;
			
		}
		public long getRecordCount(System.Data.OracleClient.OracleConnection p_conn, 
			System.Data.OracleClient.OracleTransaction p_trans,
			string strSQL,string strTableName)
		{
			System.Data.OracleClient.OracleCommand p_Command;
			long intRecTtl=0;
			this.m_intError=0;
			this.m_strError="";
			try
			{
				
				p_Command = p_conn.CreateCommand();
				p_Command.CommandText = strSQL;
				p_Command.Transaction = p_trans;
				intRecTtl = Convert.ToInt32(p_Command.ExecuteScalar());
				

			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + "  SQL query command: " + strSQL + " failed" ;
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:getRecordCount  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				
			}
			p_Command = null;
			return intRecTtl;
			
		}
		public string getOracleConnString(string strDataSource,string strUserId,string strPW)
		{
			return "Data Source=" + strDataSource.Trim() + ";User Id=" + strUserId.Trim() + ";Password=" + strPW.Trim() + ";";
			
		}
	
		public string CreateSQLNOTINString(System.Data.OracleClient.OracleConnection p_conn,string strSQL)
		{
			string str = "";
			this.SqlQueryReader(p_conn, strSQL);
			if (this.m_intError == 0)
			{
				if (this.m_DataReader.HasRows)
				{
					while (this.m_DataReader.Read())
					{
						if (str.Trim().Length == 0)
						{
							str = this.m_DataReader[0].ToString().Trim();
						}
						else
						{
						    str += "," + this.m_DataReader[0].ToString().Trim();
						}
					}
				}
				this.m_DataReader.Close();
			}
			return str;
			
			
		}
		public string getSingleStringValueFromSQLQuery(System.Data.OracleClient.OracleConnection p_conn,
			string strSQL,string strTableName)
		{
			System.Data.OracleClient.OracleCommand p_Command;
			string strValue="";
			this.m_intError=0;
			this.m_strError="";
			try
			{
				
				p_Command = p_conn.CreateCommand();
				p_Command.CommandText = strSQL;
				strValue = Convert.ToString(p_Command.ExecuteScalar());
				

			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + "  SQL query command: " + strSQL + " failed" ;
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:getSingleStringValueFromSQLQuery  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				
			}
			p_Command = null;
			return strValue;

		}
		public string getSingleStringValueFromSQLQuery(System.Data.OracleClient.OracleConnection p_conn,
			System.Data.OracleClient.OracleTransaction p_trans, string strSQL,string strTableName)
		{
			System.Data.OracleClient.OracleCommand p_Command;
			string strValue="";
			this.m_intError=0;
			this.m_strError="";
			try
			{
				
				p_Command = p_conn.CreateCommand();
				p_Command.CommandText = strSQL;
				p_Command.Transaction = p_trans;
				strValue = Convert.ToString(p_Command.ExecuteScalar());
				

			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + "  SQL query command: " + strSQL + " failed" ;
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:getSingleStringValueFromSQLQuery  \n" + 
					"Err Msg - " + this.m_strError,
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				
				
				
			}
			p_Command = null;
			return strValue;

		}
		/// <summary>
		/// Execute a query that returns a single string value
		/// </summary>
		/// <param name="strConn">Access connection string</param>
		/// <param name="strSQL">SQL that returns a single string value</param>
		/// <param name="strTableName">table name</param>
		/// <returns></returns>
		public string getSingleStringValueFromSQLQuery(string strConn,
			string strSQL,string strTableName)
		{
			System.Data.OracleClient.OracleConnection oOleDbConn;
			System.Data.OracleClient.OracleCommand oOleDbCommand;
			string strValue="";
			this.m_intError=0;
			this.m_strError="";
			try
			{
				oOleDbConn = new System.Data.OracleClient.OracleConnection();
				this.OpenConnection(strConn, ref oOleDbConn);
				if (m_intError==0)
				{
					oOleDbCommand = oOleDbConn.CreateCommand();
					oOleDbCommand.CommandText = strSQL;
					strValue = Convert.ToString(oOleDbCommand.ExecuteScalar());
					oOleDbConn.Close();
				}
				

			}
			catch (Exception caught)
			{
				this.m_intError = -1;
				this.m_strError = caught.Message + "  SQL query command: " + strSQL + " failed" ;
				if (_bDisplayErrors)
					MessageBox.Show("!!Error!! \n" + 
						"Module - Oracle:getSingleStringValueFromSQLQuery  \n" + 
						"Err Msg - " + this.m_strError,
						"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
						System.Windows.Forms.MessageBoxIcon.Exclamation);
				
			}
			oOleDbCommand = null;
			oOleDbConn=null;
			return strValue;

		}
		public System.Data.DataTable ConvertDataViewToDataTable(
			System.Data.DataView p_dv)
		{
			int x=0;
			System.Data.DataTable p_dtNew;
			//copy exact structure from the view to the new table
			p_dtNew = p_dv.Table.Clone();
			int idx = 0;
			//create an array containing all the column names in the new data table
			string[] strColNames = new string[p_dtNew.Columns.Count];
			for (x=0;x<=p_dtNew.Columns.Count-1;x++)
			{
				strColNames[idx++] = p_dtNew.Columns[x].ColumnName;
			}
			//append each row in the dataview to the new table
			System.Collections.IEnumerator viewEnumerator = p_dv.GetEnumerator();
			
			while (viewEnumerator.MoveNext())
			{
				DataRowView drv = (DataRowView)viewEnumerator.Current;
				DataRow dr = p_dtNew.NewRow();
				try
				{
					foreach (string strName in strColNames)
					{
						//value in data table row and column equal to value in 
						//dataview row and column value
						dr[strName] = drv[strName];
						
					}
				}
				catch (System.Threading.ThreadInterruptedException err)
				{
				
				}
				catch (System.Threading.ThreadAbortException err)
				{
				}
				catch (Exception ex)
				{
				if (_bDisplayErrors)	
					MessageBox.Show("!!Error!! \n" + 
						"Module - Oracle:ConvertDataViewToDataTable  \n" + 
						"Err Msg - " + ex.Message,
						"QATools",System.Windows.Forms.MessageBoxButtons.OK,
						System.Windows.Forms.MessageBoxIcon.Exclamation);
				
				}
				//append the new row to the data table
				p_dtNew.Rows.Add(dr);
			}
			return p_dtNew;
		}
		/// <summary>
		/// Converts a given delimited file into a dataset. 
		/// Assumes that the first line    
		/// of the text file contains the column names.
		/// </summary>
		/// <param name="File">The name of the file to open</param>    
		/// <param name="TableName">The name of the 
		/// Table to be made within the DataSet returned</param>
		/// <param name="delimiter">The string to delimit by</param>
		/// <returns></returns>  
		public void ConvertDelimitedTextToDataTable(System.Data.DataSet p_ds, 
			                                            string p_strFile, 
                                             			string p_strTableName, string p_strDelimiter)
		{   
            this.m_intError=0;
			try
			{
				//Open the file in a stream reader.
				StreamReader s = new StreamReader(p_strFile);
        
				//Split the first line into the columns       
				string[] columns = s.ReadLine().Split(p_strDelimiter.ToCharArray());
  
				//Add the new DataTable to the RecordSet
				p_ds.Tables.Add(p_strTableName);
    
				//Cycle the colums, adding those that don't exist yet 
				//and sequencing the one that do.
				foreach(string col in columns)
				{
					bool added = false;
					string next = "";
					int i = 0;
					while(!added)        
					{
						//Build the column name and remove any unwanted characters.
						string columnname = col + next;
						columnname = columnname.Replace("#","");
						columnname = columnname.Replace("'","");
						columnname = columnname.Replace("&","");
						columnname = columnname.Replace("\"","");
        
						//See if the column already exists
						if(!p_ds.Tables[p_strTableName].Columns.Contains(columnname))
						{
							//if it doesn't then we add it here and mark it as added
							p_ds.Tables[p_strTableName].Columns.Add(columnname);
							added = true;
						}
						else
						{
							//if it did exist then we increment the sequencer and try again.
							i++;  
							next = "_" + i.ToString();
						}         
					}
				}
    
				//Read the rest of the data in the file.        
				string AllData = s.ReadToEnd();
    
				//Split off each row at the Carriage Return/Line Feed
				//Default line ending in most <A class=iAs style="FONT-WEIGHT: normal; FONT-SIZE: 100%; PADDING-BOTTOM: 1px; COLOR: darkgreen; BORDER-BOTTOM: darkgreen 0.07em solid; BACKGROUND-COLOR: transparent; TEXT-DECORATION: underline" href="#" target=_blank itxtdid="2592535">windows</A> exports.  
				//You may have to edit this to match your particular file.
				//This will work for Excel, Access, etc. default exports.
				string[] rows = AllData.Split("\r\n".ToCharArray());
 
				//Now add each row to the DataSet        
				foreach(string r in rows)
				{
					//Split the row at the delimiter.
					string[] items = r.Split(p_strDelimiter.ToCharArray());
      
					//Add the item
					p_ds.Tables[p_strTableName].Rows.Add(items);  
				}
			}
			catch (Exception caught)
			{

				this.m_intError=-1;
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:ConvertDelimitedTextToDataTable  \n" + 
					"Err Msg - " + caught.Message.ToString().Trim(),
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				this.m_intError=-1;
			}
    
			//Return the imported data.        

		}
		/// <summary>
		/// Create an oledb data adapter insert command. The select sql statement
		/// is used to get the data types of the fields used in the insert.
		/// </summary>
		/// <param name="p_conn">the oledb database connection</param>
		/// <param name="p_da">the data adapter</param>
		/// <param name="p_trans">oledb transaction object</param>
		/// <param name="p_strSQL">select sql statement containing fields in the insert command</param>
		/// <param name="p_strTable">table name that records are inserted</param>
		public void ConfigureDataAdapterInsertCommand(System.Data.OracleClient.OracleConnection p_conn, 
			                                          System.Data.OracleClient.OracleDataAdapter p_da,
													   System.Data.OracleClient.OracleTransaction p_trans,
			                                           string p_strSQL,string p_strTable)
		{

			this.m_intError=0;
			System.Data.DataTable p_dtTableSchema = this.getTableSchema(p_conn,p_trans, p_strSQL);
			if (this.m_intError !=0) return;
			string strFields = "";
			string strValues = "";
			int x;
			try
			{
				//Build the plot insert sql
				for (x=0; x<=p_dtTableSchema.Rows.Count-1;x++)
				{
					if (strFields.Trim().Length == 0)
					{
						strFields = "(";
					}
					else
					{	
						strFields = strFields + "," ;
					}
					strFields = strFields + p_dtTableSchema.Rows[x]["columnname"].ToString().Trim();
					if (strValues.Trim().Length == 0)
					{
						strValues = "(";
					}
					else
					{	
						strValues = strValues + ",";
					}
					strValues = strValues + "?";

				}
				strFields = strFields + ")";
				strValues = strValues + ");";
				//create an insert command 
				p_da.InsertCommand = p_conn.CreateCommand();
				//bind the transaction object to the insert command
				p_da.InsertCommand.Transaction = p_trans;
				p_da.InsertCommand.CommandText = 
					"INSERT INTO " + p_strTable + " "  + strFields + " VALUES " + strValues;
				//define field datatypes for the data adapter
				for (x=0; x<=p_dtTableSchema.Rows.Count-1;x++)
				{
					strFields=p_dtTableSchema.Rows[x]["columnname"].ToString().Trim();
					switch (p_dtTableSchema.Rows[x]["datatype"].ToString().Trim())
					{
						case "System.String" :
							p_da.InsertCommand.Parameters.Add
								(strFields, 
								System.Data.OracleClient.OracleType.VarChar,
								0,
								strFields);
							break;
						case "System.Double":
							p_da.InsertCommand.Parameters.Add
								(strFields, 
								System.Data.OracleClient.OracleType.Double,
								0,
								strFields);
							break;
						//case "System.Boolean":
						//	p_da.InsertCommand.Parameters.Add
						//		(strFields, 
						//		System.Data.OracleClient.OracleType.Boolean,
						//		0,
						//		strFields);
						//	break;
						case "System.DateTime":
							p_da.InsertCommand.Parameters.Add
								(strFields, 
								System.Data.OracleClient.OracleType.DateTime,
								0,
								strFields);
							break;
						case "System.Decimal":
							p_da.InsertCommand.Parameters.Add
								(strFields, 
								System.Data.OracleClient.OracleType.Double,
								0,
								strFields);
							break;
						case "System.Int16":
							p_da.InsertCommand.Parameters.Add
								(strFields, 
								System.Data.OracleClient.OracleType.Int16,
								0,
								strFields);
							break;
						case "System.Int32":
							p_da.InsertCommand.Parameters.Add
								(strFields, 
								System.Data.OracleClient.OracleType.Int32,
								0,
								strFields);
							break;
						case "System.Int64":
							p_da.InsertCommand.Parameters.Add
								(strFields, 
								System.Data.OracleClient.OracleType.Number,
								0,
								strFields);
							break;
						case "System.SByte":
							p_da.InsertCommand.Parameters.Add
								(strFields, 
								System.Data.OracleClient.OracleType.SByte,
								0,
								strFields);
							break;
						case "System.Byte":
							p_da.InsertCommand.Parameters.Add
								(strFields, 
								System.Data.OracleClient.OracleType.Byte,
								0,
								strFields);
							break;
						case "System.Single":
							p_da.InsertCommand.Parameters.Add
								(strFields, 
								System.Data.OracleClient.OracleType.Float,
								0,
								strFields);
							break;
						default:
							MessageBox.Show("Could Not Set Data Adapter Parameter For DataType " + p_dtTableSchema.Rows[x]["datatype"].ToString().Trim());
							break;
					}
									
				}
			}
			catch (Exception e)
			{
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:ConfigureDataAdapterInsertCommand  \n" + 
					"Err Msg - " + e.Message.ToString().Trim(),
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				this.m_intError=-1;
			}

		}
/// <summary>
/// create the update command for the data adapter. 
/// </summary>
/// <param name="p_conn">oracle connection object</param>
/// <param name="p_da">oracle dataadapter object</param>
/// <param name="p_trans">oracle transaction object</param>
/// <param name="p_strSQL">select sql statement to get the update field data types</param>
/// <param name="p_strSQLUniqueRecordFields">select SQL statement listing fields used for a records unique id are queried and their field types obtained and added to the dataadapter updates parameters list</param>
/// <param name="p_strTable">table name to be updated</param>
		public void ConfigureDataAdapterUpdateCommand(System.Data.OracleClient.OracleConnection p_conn, 
			System.Data.OracleClient.OracleDataAdapter p_da,
			System.Data.OracleClient.OracleTransaction p_trans,
			string p_strSQL,string p_strSQLUniqueRecordFields,string p_strTable)
		{

			this.m_intError=0;
			System.Data.DataTable p_dtTableSchema = this.getTableSchema(p_conn,p_trans, p_strSQL);
			System.Data.DataTable p_dtTableSchema2 = new DataTable();
			if (this.m_intError !=0) return;
			string strField = "";
			string strValue = "";
			string strSQL="";
			int x;
			try
			{
				//Build the plot update sql
				for (x=0; x<=p_dtTableSchema.Rows.Count-1;x++)
				{
					strField = p_dtTableSchema.Rows[x]["columnname"].ToString().Trim();
					if (strValue.Trim().Length == 0)
					{
						strValue = strField + "=?";
					}
					else
					{	
						strValue += "," + strField + "=?";
					}
				}
				
				strSQL = 
					"UPDATE " + p_strTable + " SET "  +  strValue;

				//get the unique record id
				if (p_strSQLUniqueRecordFields.Trim().Length > 0)
				{
					strValue="";
					p_dtTableSchema2 = this.getTableSchema(p_conn,p_trans,p_strSQLUniqueRecordFields);
					if (this.m_intError !=0) return;
					//build the where condition
					for (x=0; x<=p_dtTableSchema2.Rows.Count-1;x++)
					{
						strField = p_dtTableSchema2.Rows[x]["columnname"].ToString().Trim();
						if (strValue.Trim().Length == 0)
						{
							strValue = strField + "=?";
						}
						else
						{	
							strValue += " AND " + strField + "=?";
						}
					}
					strSQL += " WHERE " + strValue;
				}


				//create an insert command 
				p_da.UpdateCommand = p_conn.CreateCommand();
				//bind the transaction object to the insert command
				p_da.UpdateCommand.Transaction = p_trans;
				p_da.UpdateCommand.CommandText = strSQL;

				//copy the table schema records containing update fields info to a new table
                System.Data.DataTable p_dt = p_dtTableSchema.Copy();

				//define field datatypes for the data adapter
				for (;;)
				{
					for (x=0; x<=p_dt.Rows.Count-1;x++)
					{
						strField=p_dt.Rows[x]["columnname"].ToString().Trim();
						switch (p_dt.Rows[x]["datatype"].ToString().Trim())
						{
							case "System.String" :
								p_da.UpdateCommand.Parameters.Add
									(strField, 
									System.Data.OracleClient.OracleType.VarChar,
									0,
									strField);
								break;
							case "System.Double":
								p_da.UpdateCommand.Parameters.Add
									(strField, 
									System.Data.OracleClient.OracleType.Double,
									0,
									strField);
								break;
							//case "System.Boolean":
							//	p_da.UpdateCommand.Parameters.Add
							//		(strField, 
							//		System.Data.OracleClient.OracleType.,
							//		0,
							//		strField);
							//	break;
							case "System.DateTime":
								p_da.UpdateCommand.Parameters.Add
									(strField, 
									System.Data.OracleClient.OracleType.DateTime,
									0,
									strField);
								break;
							case "System.Decimal":
								p_da.UpdateCommand.Parameters.Add
									(strField, 
									System.Data.OracleClient.OracleType.Number,
									0,
									strField);
								break;
							case "System.Int16":
								p_da.UpdateCommand.Parameters.Add
									(strField, 
									System.Data.OracleClient.OracleType.Int16,
									0,
									strField);
								break;
							case "System.Int32":
								p_da.UpdateCommand.Parameters.Add
									(strField, 
									System.Data.OracleClient.OracleType.Int32,
									0,
									strField);
								break;
							case "System.Int64":
								p_da.UpdateCommand.Parameters.Add
									(strField, 
									System.Data.OracleClient.OracleType.Number,
									0,
									strField);
								break;
							case "System.SByte":
								p_da.UpdateCommand.Parameters.Add
									(strField, 
									System.Data.OracleClient.OracleType.SByte,
									0,
									strField);
								break;
							case "System.Byte":
								p_da.UpdateCommand.Parameters.Add
									(strField, 
									System.Data.OracleClient.OracleType.Byte,
									0,
									strField);
								break;
							case "System.Single":
								p_da.UpdateCommand.Parameters.Add
									(strField, 
									System.Data.OracleClient.OracleType.Float,
									0,
									strField);
								break;
							default:
								MessageBox.Show("Could Not Set Data Adapter Parameter For DataType " + p_dt.Rows[x]["datatype"].ToString().Trim());
								break;
						}
									
					}
					if (p_strSQLUniqueRecordFields.Trim().Length > 0)
					{
						//clear the data table of all its records
						p_dt.Clear();
						//copy the table schema records containing where clause fields info to a new table
						p_dt = p_dtTableSchema2.Copy();
						p_strSQLUniqueRecordFields = "";
					}
					else
					{
						break;
					}
				}
			}
			catch (Exception e)
			{
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:ConfigureDataAdapterUpdateCommand  \n" + 
					"Err Msg - " + e.Message.ToString().Trim(),
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				this.m_intError=-1;
			}
		}

		/// <summary>
		/// create the delete command for the data adapter. 
		/// </summary>
		/// <param name="p_conn">oledb connection object</param>
		/// <param name="p_da">oledb dataadapter object</param>
		/// <param name="p_trans">oledb transaction object</param>
		/// <param name="p_strSQLUniqueRecordFields">select SQL statement listing fields used for a records unique id are queried and their field types obtained and added to the dataadapter delete command parameters list</param>
		/// <param name="p_strTable">table name to be updated</param>
		public void ConfigureDataAdapterDeleteCommand(System.Data.OracleClient.OracleConnection p_conn, 
			System.Data.OracleClient.OracleDataAdapter p_da,
			System.Data.OracleClient.OracleTransaction p_trans,
			string p_strSQLUniqueRecordFields,string p_strTable)
		{

			this.m_intError=0;
			System.Data.DataTable p_dt = this.getTableSchema(p_conn,p_trans, p_strSQLUniqueRecordFields);
			if (this.m_intError !=0) return;
			string strField = "";
			string strValue = "";
			string strSQL="";
			int x;
			try
			{
				strSQL = "DELETE FROM " + p_strTable + " ";
				//build the where condition
				for (x=0; x<=p_dt.Rows.Count-1;x++)
				{
					strField = p_dt.Rows[x]["columnname"].ToString().Trim();
					if (strValue.Trim().Length == 0)
					{
						strValue = strField + "=?";
					}
					else
					{	
						strValue += " AND " + strField + "=?";
					}
				}
				strSQL += " WHERE " + strValue;
				


				//create an insert command 
				p_da.DeleteCommand = p_conn.CreateCommand();
				//bind the transaction object to the insert command
				p_da.DeleteCommand.Transaction = p_trans;
				p_da.DeleteCommand.CommandText = strSQL;

				

				//define field datatypes for the data adapter
				
				
				for (x=0; x<=p_dt.Rows.Count-1;x++)
				{
					strField=p_dt.Rows[x]["columnname"].ToString().Trim();
					switch (p_dt.Rows[x]["datatype"].ToString().Trim())
					{
						case "System.String" :
							p_da.DeleteCommand.Parameters.Add
								(strField, 
								System.Data.OracleClient.OracleType.VarChar,
								0,
								strField);
							break;
						case "System.Double":
							p_da.DeleteCommand.Parameters.Add
								(strField, 
								System.Data.OracleClient.OracleType.Double,
								0,
								strField);
							break;
						//case "System.Boolean":
						//	p_da.DeleteCommand.Parameters.Add
						//		(strField, 
						//		System.Data.OracleClient.OracleType.Boolean,
						//		0,
						//		strField);
						//	break;
						case "System.DateTime":
							p_da.DeleteCommand.Parameters.Add
								(strField, 
								System.Data.OracleClient.OracleType.DateTime,
								0,
								strField);
							break;
						case "System.Decimal":
							p_da.DeleteCommand.Parameters.Add
								(strField, 
								System.Data.OracleClient.OracleType.Double,
								0,
								strField);
							break;
						case "System.Int16":
							p_da.DeleteCommand.Parameters.Add
								(strField, 
								System.Data.OracleClient.OracleType.Int16,
								0,
								strField);
							break;
						case "System.Int32":
							p_da.DeleteCommand.Parameters.Add
								(strField, 
								System.Data.OracleClient.OracleType.Int32,
								0,
								strField);
							break;
						case "System.Int64":
							p_da.DeleteCommand.Parameters.Add
								(strField, 
								System.Data.OracleClient.OracleType.Number,
								0,
								strField);
							break;
						case "System.SByte":
							p_da.DeleteCommand.Parameters.Add
								(strField, 
								System.Data.OracleClient.OracleType.SByte,
								0,
								strField);
							break;
						case "System.Byte":
							p_da.DeleteCommand.Parameters.Add
								(strField, 
								System.Data.OracleClient.OracleType.Byte,
								0,
								strField);
							break;
						case "System.Single":
							p_da.DeleteCommand.Parameters.Add
								(strField, 
								System.Data.OracleClient.OracleType.Float,
								0,
								strField);
							break;
						default:
							MessageBox.Show("Could Not Set Data Adapter Parameter For DataType " + p_dt.Rows[x]["datatype"].ToString().Trim());
							break;
					}
									
				}
			
					
				
			}
			catch (Exception e)
			{
				if (_bDisplayErrors)
				MessageBox.Show("!!Error!! \n" + 
					"Module - Oracle:ConfigureDataAdapterUpdateCommand  \n" + 
					"Err Msg - " + e.Message.ToString().Trim(),
					"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Exclamation);
				this.m_intError=-1;
			}
		}
		public void ConfigureSqlForMSAccessUsage(ref string[] p_strColumns, string[] p_strDataTypes)
		{

		}
	

		public bool TableExist(System.Data.OracleClient.OracleConnection p_conn,string p_strOwner,string p_strTable)
		{
			System.Data.OracleClient.OracleDataReader oDataReader; // = new System.Data.OracleClient.OracleDataReader();
			System.Data.OracleClient.OracleCommand oCommand = new System.Data.OracleClient.OracleCommand();
			string strSQL = "SELECT table_name FROM all_tables WHERE TRIM(owner) = '" + p_strOwner.Trim() + "' AND TRIM(table_name) = '" + p_strTable.Trim() + "'";
			oCommand = p_conn.CreateCommand();
			oCommand.CommandText = strSQL;
			try
			{
				oDataReader = oCommand.ExecuteReader();
				if (oDataReader.HasRows)
				{
					oDataReader.Close();
					oDataReader = null;
					oCommand = null;
					return true;
					
				}
				oDataReader.Close();
				
			}
			catch (Exception e)
			{
				if (_bDisplayErrors)
					MessageBox.Show("!!Error!! \n" + 
						"Module - Oracle:TableExist  \n" + 
						"Err Msg - " + e.Message.ToString().Trim(),
						"QA Tools",System.Windows.Forms.MessageBoxButtons.OK,
						System.Windows.Forms.MessageBoxIcon.Exclamation);
				this.m_intError=-1;
			}
			oDataReader = null;
			oCommand = null;
			return false;
			
			
			
		}
		public void CloseConnection(System.Data.OracleClient.OracleConnection p_conn)
		{
			try
			{
				p_conn.Close();
			}
			catch
			{
			}
		}
		public bool DisplayErrors
		{
			get {return _bDisplayErrors;}
			set {_bDisplayErrors = value;}
		}
		public string MessageBoxTitle
		{
			get {return _strMsgBoxTitle;}
			set {_strMsgBoxTitle=value;}
		}
		


	

		


	}



}
