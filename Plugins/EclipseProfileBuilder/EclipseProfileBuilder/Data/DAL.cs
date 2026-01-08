using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Reflection;
using System.Windows.Forms;
using Eclipse.WoWDatabase;
namespace ArachnidCreations
{
    /* 
     * WARNING
     * THIS IS A MODIFIED DAL EVERYTHING IS STATIC AND SHOULD NOT BE USED FOR PRODUCTION OR WEB ENVIROMENTS
    */
    public class DAL
    {
        private static SQLiteConnection dbConn;
        private static SQLiteDataAdapter DB;
        public  static string DBFile;
        public static void ExecuteSL3Query(string txtQuery, bool suppressErrors = false)
        {
            try
            {
                //if (FileName == null) 
                SetSL3Connection();
                //else SetSL3Connection(FileName);
                dbConn.Open();
                SQLiteCommand sqlCommand = new SQLiteCommand();
                sqlCommand = dbConn.CreateCommand();
                sqlCommand.CommandText = txtQuery;
                sqlCommand.ExecuteNonQuery();
                dbConn.Close();
            }
           catch (Exception e) { 
               if (suppressErrors == false) System.Windows.Forms.MessageBox.Show(e.ToString() + "\r\n " + txtQuery); 
           }
        }
        private static void SetSL3Connection(string curFile = null)
        {
            if (curFile != null)
            {
                if (!File.Exists(curFile)) curFile = Path.Combine(Application.StartupPath, curFile);
                if (!File.Exists(curFile))
                {
                    //dbConn = new SQLiteConnection(@"Data Source=" + curFile + ";Version=3;New=True;Compress=True;");
                }
                else { dbConn = new SQLiteConnection(@"Data Source=" + curFile + ";Version=3;New=False;Compress=True;"); }
            }
            else
            {
                if (!File.Exists(DBFile)) DBFile = Path.Combine(Application.StartupPath, "EclipseWoWDB.edb");
                if (File.Exists(DBFile))
                {
                    dbConn = new SQLiteConnection(@"Data Source=" + DBFile + ";Version=3;New=False;Compress=True;");
                }
                else dbConn = new SQLiteConnection(@"Data Source=" + DBFile + ";Version=3;New=True;Compress=True;");
            }
        }
        public static DataTable LoadSL3Data(string Commandtext, string FileName = null)
        {
            DataSet DS = new DataSet();
            DataTable DT = new DataTable();
            try
            {
                if (FileName == null) SetSL3Connection();
                else SetSL3Connection(FileName);
                dbConn.Open();
                SQLiteCommand sqlCommand = new SQLiteCommand();
                sqlCommand = dbConn.CreateCommand();
                DB = new SQLiteDataAdapter(Commandtext, dbConn);
                DS.Reset();
                DB.Fill(DS);
                DT = DS.Tables[0];
                dbConn.Close();
                return DT;
            }
            catch (Exception e) { System.Windows.Forms.MessageBox.Show(e.ToString() + "\r\n " + Commandtext); return null; }
        }
        public static string generateCreateSQL(object userClass, string tablename, string fieldprefix = "")
        {
            object foo = userClass;
            string returnSQL = string.Empty;
            returnSQL += (string.Format("CREATE TABLE {0} ( ", tablename));
            int icount = 0;
            string comma = "";
            foreach (var prop in userClass.GetType().GetProperties())
            {
                icount++;
                //tbOutput.AppendText(string.Format("name:{0} value:{1} type: {2}", prop.Name, prop.GetValue(foo, null), prop.GetType().Name));
                //tbOutput.AppendText(prop.GetType().ToString());
                //propertyGrid1.SelectedObject = prop;
                if (icount < foo.GetType().GetProperties().Length) comma = ",\r\n";
                else comma = "";
                if (prop.PropertyType.ToString() == "System.Char")
                {
                    string sqlvartype = "[Varchar] (250)";
                    returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                }
                else if (prop.PropertyType.ToString() == "System.String")
                {
                    string sqlvartype = "[text]";
                    returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                }
                else if (prop.PropertyType.ToString() == "System.Int64")
                {
                    string sqlvartype = "[int]";
                    returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                }
                else if (prop.PropertyType.ToString() == "System.UInt32")
                {
                    string sqlvartype = "[int]";
                    returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                }
                else if (prop.PropertyType.ToString() == "System.Int32")
                {
                    string sqlvartype = "[int]";
                    returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                }
                else if (prop.PropertyType.ToString() == "System.Float")
                {
                    string sqlvartype = "[float]";
                    returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                }
                else if (prop.PropertyType.ToString() == "System.Single")//float
                {
                    string sqlvartype = "[float]";
                    returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                }
                else if (prop.PropertyType.ToString() == "System.Boolean")//float
                {
                    string sqlvartype = "[bit]";
                    returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                }
                else if (prop.PropertyType.ToString() == "System.Drawing.Image")//float
                {
                    string sqlvartype = "[varchar] (250)";
                    returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                }
                //listBox1.Items.Add(prop);

            }
            //hacky dumb filter for the last comma if present
            var temp = returnSQL.Substring(returnSQL.Length-3, 1);
            if (temp == ","){
                returnSQL = returnSQL.Substring(0, returnSQL.Length - 3);
            }
            returnSQL += (string.Format(");", tablename));
            return returnSQL;
        }

        public static string Insert(object userClass, string tablename, string fieldprefix = "", bool returnId = false, DataTable dt = null)
        {
            //Compile a List<T> of colmumn names (with datatypes) from the target table


            //List<TableStructure> TS_cols = new List<TableStructure>(); <== //We are not using this...
            List<string> cols = new List<string>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    //TS_cols.Add((TableStructure)convertDataRowtoObject(new TableStructure(), row, ""));
                    cols.Add(row["Name"].ToString()); //sqlite tablenames are just name
                }
            }
            List<PropertyInfo> matchedProps = new List<PropertyInfo>();
            StringBuilder sql = new StringBuilder();
            //Select only the properties that are system types (they will match sql data types mostly) as a type of "User" wont match any sql field.
            List<PropertyInfo> props = userClass.GetType().GetProperties().Where(p => p.PropertyType.ToString().ToLower().Contains("system.")).ToList();
            foreach (var col in cols.Distinct())
            {
                //WE only want to create sql code for the properties that are in our object that MATCH the sql fields. 
                //otherwise we will create empty insert values that wont match the table.
                var propMatch = props.Where(p => p.Name.ToLower() == col.ToLower()).FirstOrDefault();
                if (propMatch != null)
                {
                    matchedProps.Add(propMatch);
                }

            }
            //Console.Write(matchedProps);
            sql.Append(string.Format("insert into {0} (", tablename));
            int propcount = 0;
            foreach (PropertyInfo prop in matchedProps)
            {
                propcount++;
                sql.Append(prop.Name);
                if (propcount != matchedProps.Count) sql.Append(",");
            }
            sql.Append(") values (");
            propcount = 0; //starting at one because we are going to skip a property called id 
            foreach (PropertyInfo prop in matchedProps)
            {
                propcount++;
                //ToDo: make sure the property is not the primary key before inserting it
                if (cols.Where(c => c.ToLower() == prop.Name.ToLower()).Count() > 0) // we want to skip ID since it usually cant be inserted... and make sure the property exists in the column names
                {
                    if (prop.PropertyType.ToString().ToLower().Contains("datetime"))
                    {
                        sql.Append(string.Format("'{0:s}'", prop.GetValue(userClass)));
                    }
                    else if (prop.PropertyType.ToString().ToLower().Contains("bool"))
                    {
                        string insert = "";
                        if (prop.GetValue(userClass) != null)
                        {
                            if (prop.GetValue(userClass).ToString().ToLower() == "true") insert = "1";
                            else insert = "0";
                        }
                        sql.Append(string.Format("'{0}'", insert));
                    }
                    else if (prop.PropertyType.ToString().ToLower().Contains("int32") || prop.PropertyType.ToString().ToLower().Contains("uint32"))
                    {
                        if (prop.GetValue(userClass) != null) sql.Append(string.Format("'{0}'", prop.GetValue(userClass).ToString()));
                    }
                    else
                    {
                        try
                        {
                            if (prop.GetValue(userClass) != null) sql.Append(string.Format("'{0}'", prop.GetValue(userClass).ToString().Replace("'", "''")));
                            else sql.Append(string.Format("'{0}'", ""));
                        }
                        catch (Exception err)
                        {
                            
                            // This property is not serializable into sql
                            //Eclipse.WoWDatabase.Core.iLog(string.Format("Cannot serialize property {0} - {1}", prop, err.Message));
                            sql.Append(string.Format("'{0}'", ""));
                        }

                    }
                    if (propcount != matchedProps.Count) sql.Append(",");
                }
            }
            sql.Append(");");
            if (returnId == true) sql.Append(" select SCOPE_IDENTITY();");
            return sql.ToString();
        }
        public static DataTable getTableStructure(string tablename)
        {
            
            string sql = string.Format(@"PRAGMA table_info( '{0}' );", tablename);
            return LoadSL3Data(sql);
        }
    }
}

