using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data;
using System.Collections;

namespace ArachnidCreations.DevTools
{
    public class ORM
    {
        /// <summary>
        /// Generates an insert statement based off table structure information and an object of your choosing (any object)
        /// this will ignore any properties on your object that do not match a table name.
        /// </summary>
        ///
        /// <param name="userClass"></param>
        /// <param name="tablename"></param>
        /// <param name="fieldprefix"></param>
        /// <param name="returnId"></param>
        /// <param name="dt">Only pass null if your table matches the class EXACTLY. DataTable of structure defined in the getTableStructureData sql statement. Or the TableStructure object.</param>
        /// <returns></returns>
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
                    cols.Add(row["name"].ToString());
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
                    //MessageBox.Show(propMatch + "|" + col.ColumnName.ToLower());
                }

            }
            //Console.Write(matchedProps);
            sql.Append(string.Format("insert into {0} (", tablename));
            int propcount = 0;
            foreach (PropertyInfo prop in matchedProps)
            {
                propcount++;
                //"id" shouldnt be in here so removed the if statement - Will 7/12/2013
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
                    else if (prop.PropertyType.ToString().ToLower().Contains("int32"))
                    {
                        if (prop.GetValue(userClass) != null) sql.Append(string.Format("'{0}'", prop.GetValue(userClass).ToString()));
                    }
                    else if (prop.PropertyType.ToString().ToLower().Contains("float"))
                    {
                        if (prop.GetValue(userClass) != null) sql.Append(string.Format("'{0}'", prop.GetValue(userClass).ToString()));
                    }
                    else
                    {
                        if (prop.GetValue(userClass) != null) sql.Append(string.Format("'{0}'", prop.GetValue(userClass).ToString().Replace("'", "''")));
                        else sql.Append(string.Format("'{0}'", ""));
                    }
                    if (propcount != matchedProps.Count) sql.Append(",");
                }
            }
            sql.Append(");");
            if (returnId == true) sql.Append(" select SCOPE_IDENTITY();");
            return sql.ToString();
        }
        /// <summary>
        /// Generates an insert statement based off table structure information and an object of your choosing (any object)
        /// this will ignore any properties on your object that do not match a table name.
        /// </summary>
        ///
        /// <param name="userClass"></param>
        /// <param name="tablename"></param>
        /// <param name="fieldprefix"></param>
        /// <param name="returnId"></param>
        /// <param name="dt">Only pass null if your table matches the class EXACTLY. DataTable of structure defined in the getTableStructureData sql statement. Or the TableStructure object.</param>
        /// <returns></returns>
        public static string InsertOrUpdate(object userClass, string tablename, string fieldprefix = "", bool returnId = false, DataTable dt = null)
        {
            //Compile a List<T> of colmumn names (with datatypes) from the target table


            //List<TableStructure> TS_cols = new List<TableStructure>(); <== //We are not using this...
            List<string> cols = new List<string>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    //TS_cols.Add((TableStructure)convertDataRowtoObject(new TableStructure(), row, ""));
                    cols.Add(row["name"].ToString());
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
                    //MessageBox.Show(propMatch + "|" + col.ColumnName.ToLower());
                }

            }
            //Console.Write(matchedProps);
            sql.Append(string.Format("INSERT OR IGNORE INTO {0} (", tablename));
            int propcount = 0;
            foreach (PropertyInfo prop in matchedProps)
            {
                propcount++;
                //"id" shouldnt be in here so removed the if statement - Will 7/12/2013
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
                    else if (prop.PropertyType.ToString().ToLower().Contains("int32"))
                    {
                        if (prop.GetValue(userClass) != null) sql.Append(string.Format("'{0}'", prop.GetValue(userClass).ToString()));
                    }
                    else if (prop.PropertyType.ToString().ToLower().Contains("float"))
                    {
                        if (prop.GetValue(userClass) != null) sql.Append(string.Format("'{0}'", prop.GetValue(userClass).ToString()));
                    }
                    else
                    {
                        if (prop.GetValue(userClass) != null) sql.Append(string.Format("'{0}'", prop.GetValue(userClass).ToString().Replace("'", "''")));
                        else sql.Append(string.Format("'{0}'", ""));
                    }
                    if (propcount != matchedProps.Count) sql.Append(",");
                }
            }
            sql.Append(");");
            if (returnId == true) sql.Append(" select SCOPE_IDENTITY();");
            return sql.ToString();
        }
        /// <summary>
        /// Automatically assumes your object and your table has an "id" column/property. If it doesnt. This wont work.
        /// </summary>
        /// <param name="userClass"></param>
        /// <param name="tablename"></param>
        /// <param name="fieldprefix"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Update(object userClass, string tablename, string primarykey = "", DataTable dt = null)
        {
            List<string> cols = new List<string>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    cols.Add(row["name"].ToString());
                }
            }
            List<PropertyInfo> matchedProps = new List<PropertyInfo>();
            StringBuilder sql = new StringBuilder();
            List<PropertyInfo> props = userClass.GetType().GetProperties().Where(p => p.PropertyType.ToString().ToLower().Contains("system.")).ToList();
            foreach (var col in cols.Distinct())
            {
                var propMatch = props.Where(p => p.Name.ToLower() == col.ToLower()).FirstOrDefault();
                if (propMatch != null)
                {
                    matchedProps.Add(propMatch);
                }
            }
            sql.Append(string.Format("update  {0} set ", tablename));
            int propcount = 0;
            foreach (PropertyInfo prop in matchedProps)
            {
                propcount++;
                //ToDo: make sure the property is not the primary key before inserting it
                if (cols.Where(c => c.ToLower() == prop.Name.ToLower()).Count() > 0) // we want to skip ID since it usually cant be inserted... and make sure the property exists in the column names
                {
                    if (prop.PropertyType.ToString().ToLower().Contains("datetime"))
                    {
                        sql.Append(string.Format("{1} ='{0:s}'", prop.GetValue(userClass), prop.Name));
                    }
                    else if (prop.PropertyType.ToString().ToLower().Contains("bool"))
                    {
                        string insert = "";
                        if (prop.GetValue(userClass) != null)
                        {
                            if (prop.GetValue(userClass).ToString().ToLower() == "true") insert = "1";
                            else insert = "0";
                        }
                        sql.Append(string.Format("{1} ='{0}'", insert, prop.Name));
                    }
                    else if (prop.PropertyType.ToString().ToLower().Contains("int32"))
                    {
                        if (prop.GetValue(userClass) != null) sql.Append(string.Format("{1} ='{0}'", prop.GetValue(userClass).ToString().Replace("'", "''"), prop.Name));
                    }
                    else
                    {
                        if (prop.GetValue(userClass) != null) sql.Append(string.Format("{1} ='{0}'", prop.GetValue(userClass).ToString().Replace("'", "''"), prop.Name));
                        else sql.Append(string.Format("{0} =''", prop.Name));
                    }
                    if (propcount != (matchedProps.Count)) sql.Append(",");
                }
            }

            //This is horrible and stupid way to do this - already fixed this in later versions of this class...
            string key = string.Empty;
            PropertyInfo property = null;
            if (primarykey != null && primarykey != string.Empty)
            {
                key = primarykey;
                property = userClass.GetType().GetProperty(key);
            }
            if (primarykey == string.Empty)
            {
                property = userClass.GetType().GetProperty("Entry");
                if (property != null && key == string.Empty) key = "Entry";
                if (property == null)
                {
                    property = userClass.GetType().GetProperty("id");
                    if (property != null && key == string.Empty) key = "id";
                }
                if (property == null)
                {
                    property = userClass.GetType().GetProperty("QuestId");
                    sql.Append(string.Format(" where questid='{0}';", property.GetValue(userClass)));
                }
                else sql.Append(string.Format(" where {1}='{0}';", property.GetValue(userClass), key));
            }
            else sql.Append(string.Format(" where {1}='{0}';", property.GetValue(userClass), key));
            return sql.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userClass"></param>
        /// <param name="tablename"></param>
        /// <param name="id">you need to provide the id of the row that you wish to get.</param>
        /// <returns></returns>
        public static dynamic GetSingle(Type objtype, string tablename, int id)
        {
            DataTable dt = DAL.LoadSL3Data(string.Format("select * from {0} where id = '{1}'", tablename, id));
            int icount = 0;
            Type tType = typeof(object);
            var instance = Activator.CreateInstance(objtype);
            //foreach (DataRow row in dt.Rows)
            {
                icount++;
                DataRow row = dt.Rows[0];
                List<string> ColumnNames = new List<string>();
                foreach (DataColumn item in row.Table.Columns)
                {
                    string columnName = item.ColumnName;
                    ColumnNames.Add(columnName.ToLower());
                }

                PropertyInfo[] props = instance.GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (prop.PropertyType.ToString().ToLower().Contains("string"))
                    {
                        if (ColumnNames.Where(s => s == prop.Name.ToLower()).FirstOrDefault() != null) prop.SetValue(instance, row[prop.Name].ToString());
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("int"))
                    {
                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                        {
                            int i = 0;
                            int.TryParse(row[prop.Name].ToString(), out i);
                            prop.SetValue(instance, i);
                        }
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("float"))
                    {
                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(instance, float.Parse(row[prop.Name].ToString()));
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("datetime"))
                    {
                        DateTime now = DateTime.Now;

                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                        {
                            DateTime.TryParse(row[prop.Name].ToString(), out now);
                            prop.SetValue(instance, now);
                        }
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("double"))
                    {
                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(instance, double.Parse(row[prop.Name].ToString()));
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("decimal"))
                    {
                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                        {
                            decimal i = 0;
                            decimal.TryParse(row[prop.Name].ToString(), out i);
                            prop.SetValue(instance, i);
                        }
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("bool") && ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                    {
                        if (row[prop.Name].ToString() != "" && row[prop.Name].ToString() != null)
                        {
                            int torfint = 0;
                            int.TryParse(row[prop.Name].ToString(), out torfint);
                            bool torf = false;
                            if (torfint == 1) torf = true;
                            if (torfint == 0) torf = false;
                            if (row[prop.Name].ToString().ToLower() == "false") torf = false;
                            if (row[prop.Name].ToString().ToLower() == "true") torf = true;
                            prop.SetValue(instance, torf);
                        }
                        else
                        {
                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(instance, false);
                        }
                    }
                }//
            }
            return instance;
        }
        /// <summary>
        /// This will get an IList<T> of objects from your database - you just have to pass in the type and the tablename thats accessible via the connection string in your app.config/web.config
        /// </summary>
        /// <param name="objtype"></param>
        /// <param name="tablename"></param>
        /// <param name="where">NOT IMPLEMENTED: a dictionary<string, string> of where clauses such as ("uploaddate", "between 2013-01-01 and 2014-01-01")</param>
        /// <returns></returns>
        public static dynamic GetList(Type objtype, string tablename, Dictionary<string, string> where)
        {
            Type customList = typeof(List<>).MakeGenericType(objtype);
            IList objectList = (IList)Activator.CreateInstance(customList);

            string sql = string.Format("select * from {0}", tablename);
            if (where != null)
            {
                if (where.Count > 0)
                {
                    var iCount = 0;
                    foreach (var item in where)
                    {
                        iCount++;
                        if (iCount == 1) sql += string.Format(" where [{0}] = '{1}' ", item.Key, item.Value);
                        else sql += string.Format(" and [{0}] = '{1}' ", item.Key, item.Value);
                    }
                }
            }
            DataTable dt = DAL.LoadSL3Data(sql);
            int icount = 0;
            if (dt == null || dt.Rows.Count == 0) return objectList;
            List<string> ColumnNames = new List<string>();
            foreach (DataColumn item in dt.Rows[0].Table.Columns)
            {
                string columnName = item.ColumnName;
                ColumnNames.Add(columnName.ToLower());
            }

            foreach (DataRow row in dt.Rows)
            {
                icount++;
                var instance = Activator.CreateInstance(objtype);

                PropertyInfo[] props = instance.GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (prop.PropertyType.ToString().ToLower().Contains("string"))
                    {
                        if (ColumnNames.Where(s => s == prop.Name.ToLower()).FirstOrDefault() != null) prop.SetValue(instance, row[prop.Name].ToString());
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("int"))
                    {
                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                        {
                            int i = 0;
                            int.TryParse(row[prop.Name].ToString(), out i);
                            prop.SetValue(instance, i);
                        }
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("float"))
                    {
                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(instance, float.Parse(row[prop.Name].ToString()));
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("datetime"))
                    {
                        DateTime now = DateTime.Now;

                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                        {
                            DateTime.TryParse(row[prop.Name].ToString(), out now);
                            prop.SetValue(instance, now);
                        }
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("double"))
                    {
                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(instance, double.Parse(row[prop.Name].ToString()));
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("decimal"))
                    {
                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                        {
                            decimal i = 0;
                            decimal.TryParse(row[prop.Name].ToString(), out i);
                            prop.SetValue(instance, i);
                        }
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("bool") && ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                    {
                        if (row[prop.Name].ToString() != "" && row[prop.Name].ToString() != null)
                        {
                            int torfint = 0;
                            int.TryParse(row[prop.Name].ToString(), out torfint);
                            bool torf = false;
                            if (torfint == 1) torf = true;
                            if (torfint == 0) torf = false;
                            if (row[prop.Name].ToString().ToLower() == "false") torf = false;
                            if (row[prop.Name].ToString().ToLower() == "true") torf = true;
                            prop.SetValue(instance, torf);
                        }
                        else
                        {
                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(instance, false);
                        }
                    }
                }
                objectList.Add(instance);
            }
            return objectList;
        }
        public static string flexTagsFromObject(Object userClass, string html)
        {
            if (html != null)
            {
                foreach (PropertyInfo prop in userClass.GetType().GetProperties())
                {
                    if (prop.GetValue(userClass) != null)
                    {
                        html = html.Replace("{" + prop.Name.ToLower() + "}", prop.GetValue(userClass).ToString());
                    }
                }
                return html;
            }
            return "";
        }
        public static string flexTagsFromDictionary(Dictionary<string, string> dictionary, string html)
        {
            if (html != null)
            {
                foreach (string key in dictionary.Keys)
                {
                    html = html.Replace("{" + key + "}", dictionary[key]);
                }
                return html;
            }
            return "";
        }
        public static string generateCreateSQL(object userClass, string tablename, string fieldprefix = "")
        {
            object foo = userClass;
            string returnSQL = string.Empty;
            returnSQL += (string.Format("CREATE TABLE {0} (id INTEGER PRIMARY KEY ASC, ", tablename));
            int icount = 0;
            string comma = "";
            foreach (var prop in userClass.GetType().GetProperties())
            {
                icount++;
                //tbOutput.AppendText(string.Format("name:{0} value:{1} type: {2}", prop.Name, prop.GetValue(foo, null), prop.GetType().Name));
                //tbOutput.AppendText(prop.GetType().ToString());
                //propertyGrid1.SelectedObject = prop;

                if (icount < foo.GetType().GetProperties().Count()) comma = ",\r\n";
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
                    if (prop.Name.ToUpper() != "ID") returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                    //else returnSQL += (string.Format("[{1}{2}] INTEGER PRIMARY KEY ASC {0}", comma, fieldprefix, prop.Name, sqlvartype));
                }
                else if (prop.PropertyType.ToString().ToLower().Contains("uint"))
                {
                    string sqlvartype = "[int]";
                    if (prop.Name.ToUpper() != "ID") returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                    else returnSQL += (string.Format("[{1}{2}] INTEGER PRIMARY KEY ASC {0}", comma, fieldprefix, prop.Name, sqlvartype));
                }
                else if (prop.PropertyType.ToString() == "System.Int32")
                {
                    string sqlvartype = "[int]";
                    if (prop.Name.ToUpper() != "ID") returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                    //else returnSQL += (string.Format("[{1}{2}] INTEGER PRIMARY KEY ASC {0}", comma, fieldprefix, prop.Name, sqlvartype));

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
            returnSQL = returnSQL.Replace("\r\n", "");
            if (returnSQL.Substring(returnSQL.Length - 1, 1) == ",") returnSQL = returnSQL.Substring(0, returnSQL.Length - 1);
            returnSQL += (string.Format(");", tablename));
            return returnSQL;
        }
        public static List<T> convertDataTabletoObject<T>(DataTable dataTable, string fieldprefix = "")
        {
            int icount = 0;
            DataTable dt = dataTable;

            List<T> list = new List<T>();
            Type tType = typeof(T);
            foreach (DataRow row in dt.Rows)
            {
                icount++;
                var userClass = (T)Activator.CreateInstance(typeof(T));
                List<string> ColumnNames = new List<string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string columnName = dt.Columns[i].ColumnName;
                    if (fieldprefix != "") columnName = columnName.ToLower().Replace(fieldprefix.ToLower(), "");
                    ColumnNames.Add(columnName.ToLower());
                }
                PropertyInfo[] props = tType.GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (prop.PropertyType.ToString().ToLower().Contains("string"))
                    {
                        if (ColumnNames.Where(s => s == prop.Name.ToLower()).FirstOrDefault() != null) prop.SetValue(userClass, row[fieldprefix + prop.Name].ToString());
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("int"))
                    {
                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                        {
                            int i = 0;
                            int.TryParse(row[fieldprefix + prop.Name].ToString(), out i);
                            prop.SetValue(userClass, i);
                        }

                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("float"))
                    {
                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(userClass, float.Parse(row[fieldprefix + prop.Name].ToString()));
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("datetime"))
                    {
                        DateTime now = DateTime.Now;

                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                        {
                            DateTime.TryParse(row[fieldprefix + prop.Name].ToString(), out now);
                            prop.SetValue(userClass, now);
                        }
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("double"))
                    {
                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(userClass, double.Parse(row[fieldprefix + prop.Name].ToString()));
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("decimal"))
                    {
                        if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                        {
                            decimal i = 0;
                            decimal.TryParse(row[fieldprefix + prop.Name].ToString(), out i);
                            prop.SetValue(userClass, i);
                        }
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("bool") && ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                    {
                        if (row[fieldprefix + prop.Name].ToString() != "" && row[fieldprefix + prop.Name].ToString() != null)
                        {
                            int torfint = 0;
                            int.TryParse(row[fieldprefix + prop.Name].ToString(), out torfint);
                            bool torf = false;
                            if (torfint == 1) torf = true;
                            if (torfint == 0) torf = false;
                            if (row[fieldprefix + prop.Name].ToString().ToLower() == "false") torf = false;
                            if (row[fieldprefix + prop.Name].ToString().ToLower() == "true") torf = true;
                            prop.SetValue(userClass, torf);
                        }
                        else
                        {
                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(userClass, false);
                        }
                    }
                }
            }
            return list;
        }
        public static object convertDataRowtoObject(object userClass, DataRow row, string fieldprefix = "")
        {
            int icount = 0;
            DataRow dt = row;
            //if (dt != null && userClass != null && userClass != null)
            {
                Type tType = typeof(object);
                //foreach (DataRow row in dt.Rows)
                {
                    icount++;

                    List<string> ColumnNames = new List<string>();
                    foreach (DataColumn item in row.Table.Columns)
                    {
                        string columnName = item.ColumnName;
                        if (fieldprefix != "") columnName = columnName.ToLower().Replace(fieldprefix.ToLower(), "");
                        ColumnNames.Add(columnName.ToLower());
                    }

                    PropertyInfo[] props = userClass.GetType().GetProperties();
                    foreach (PropertyInfo prop in props)
                    {
                        //Core.iLog(prop.PropertyType.ToString().ToLower());
                        if (prop.PropertyType.Name == "String")
                        {
                            if (ColumnNames.Where(s => s == prop.Name.ToLower()).FirstOrDefault() != null && prop.SetMethod != null)
                            {
                                prop.SetValue(userClass, row[fieldprefix + prop.Name].ToString());
                            }
                        }
                        if (prop.PropertyType.ToString() == ("System.UInt32"))
                        {
                            if (ColumnNames.Where(s => s.ToLower() == prop.Name.ToLower()).FirstOrDefault() != null)
                            {
                                uint i = 0;
                                uint.TryParse(row[fieldprefix + prop.Name].ToString(), out i);
                                prop.SetValue(userClass, i);
                                //Core.log("he");
                            }
                        }
                        if (prop.PropertyType.ToString() == ("System.Int32"))
                        {
                            if (ColumnNames.Where(s => s.ToLower() == prop.Name.ToLower()).FirstOrDefault() != null)
                            {
                                int i = 0;
                                int.TryParse(row[fieldprefix + prop.Name].ToString(), out i);
                                prop.SetValue(userClass, i);
                            }
                        }
                        if (prop.PropertyType.ToString().ToLower().Contains("float"))
                        {
                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(userClass, float.Parse(row[fieldprefix + prop.Name].ToString()));
                        }
                        if (prop.PropertyType.ToString().ToLower().Contains("datetime"))
                        {
                            DateTime now = DateTime.Now;

                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                            {
                                DateTime.TryParse(row[fieldprefix + prop.Name].ToString(), out now);
                                prop.SetValue(userClass, now);
                            }
                        }
                        if (prop.PropertyType.ToString().ToLower().Contains("double"))
                        {
                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(userClass, double.Parse(row[fieldprefix + prop.Name].ToString()));
                        }
                        if (prop.PropertyType.ToString().ToLower().Contains("single"))
                        {
                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                                prop.SetValue(userClass, float.Parse(row[fieldprefix + prop.Name].ToString()));
                        }
                        if (prop.PropertyType.ToString().ToLower().Contains("decimal"))
                        {
                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                            {
                                decimal i = 0;
                                decimal.TryParse(row[fieldprefix + prop.Name].ToString(), out i);
                                prop.SetValue(userClass, i);
                            }
                        }
                        if (prop.PropertyType.ToString().ToLower().Contains("bool") && ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null)
                        {
                            if (row[fieldprefix + prop.Name].ToString() != "" && row[fieldprefix + prop.Name].ToString() != null)
                            {
                                int torfint = 0;
                                int.TryParse(row[fieldprefix + prop.Name].ToString(), out torfint);
                                bool torf = false;
                                if (torfint == 1) torf = true;
                                if (torfint == 0) torf = false;
                                if (row[fieldprefix + prop.Name].ToString().ToLower() == "false") torf = false;
                                if (row[fieldprefix + prop.Name].ToString().ToLower() == "true") torf = true;
                                prop.SetValue(userClass, torf);
                            }
                            else
                            {
                                if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(userClass, false);
                            }
                        }
                    }//
                }
                return userClass;
            }
            //else
            {
                return null;
            }
        }
        public static object convertDataTabletoObjects(List<object> userClasses, object userClass, DataTable dataTable, string fieldprefix = "")
        {
            int icount = 0;
            DataTable dt = dataTable;
            //if (dt != null && userClass != null && userClass != null)
            {
                Type tType = typeof(object);
                List<string> ColumnNames = new List<string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string columnName = dt.Columns[i].ColumnName;
                    if (fieldprefix != "") columnName = columnName.ToLower().Replace(fieldprefix.ToLower(), "");
                    ColumnNames.Add(columnName.ToLower());
                }
                foreach (DataRow row in dt.Rows)
                {
                    icount++;
                    object obj = userClass;
                    PropertyInfo[] props = userClass.GetType().GetProperties();
                    foreach (PropertyInfo prop in props)
                    {
                        if (prop.PropertyType.ToString().ToLower().Contains("string"))
                        {
                            if (ColumnNames.Where(s => s == prop.Name.ToLower()).FirstOrDefault() != null) prop.SetValue(obj, row[fieldprefix + prop.Name].ToString());
                        }
                        if (prop.PropertyType.ToString().ToLower().Contains("int"))
                        {
                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(obj, int.Parse(row[fieldprefix + prop.Name].ToString()));
                        }
                        if (prop.PropertyType.ToString().ToLower().Contains("float"))
                        {
                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(obj, float.Parse(row[fieldprefix + prop.Name].ToString()));
                        }
                        if (prop.PropertyType.ToString().ToLower().Contains("datetime"))
                        {
                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(obj, DateTime.Parse(row[fieldprefix + prop.Name].ToString()));
                        }
                        if (prop.PropertyType.ToString().ToLower().Contains("double"))
                        {
                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(obj, double.Parse(row[fieldprefix + prop.Name].ToString()));
                        }
                        if (prop.PropertyType.ToString().ToLower().Contains("bool"))
                        {
                            int torfint = int.Parse(row[fieldprefix + prop.Name].ToString());
                            bool torf = false;
                            if (torfint == 1) torf = true;
                            if (torfint == 0) torf = false;
                            if (ColumnNames.Where(s => s.Contains(prop.Name.ToLower())).FirstOrDefault() != null) prop.SetValue(userClass, torf);
                        }
                    }
                    userClasses.Add(obj);

                }
                return userClasses;
            }
            //else
            {
                return null;
            }
        }
        public static DataTable convertObjectToDataTable(List<object> items)
        {
            DataTable dt = new DataTable();
            Type tType = typeof(object);
            PropertyInfo[] props = tType.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                DataColumn col = new DataColumn(prop.Name, prop.PropertyType);
                dt.Columns.Add(col);
            }
            foreach (Object item in items)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo prop in props)
                {
                    dr[prop.Name] = prop.GetValue(item, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static DataTable convertObjectToDataTable(object item)
        {
            DataTable dt = new DataTable();
            Type tType = typeof(object);
            foreach (PropertyInfo prop in item.GetType().GetProperties())
            {
                DataColumn col = new DataColumn(prop.Name, prop.PropertyType);
                dt.Columns.Add(col);
            }

            DataRow dr = dt.NewRow();
            foreach (PropertyInfo prop in item.GetType().GetProperties())
            {
                dr[prop.Name] = prop.GetValue(item, null);
            }
            dt.Rows.Add(dr);

            return dt;
        }
        public static object convertObjectToObject(object to, object from)
        {
            PropertyInfo[] toProps = to.GetType().GetProperties();


            PropertyInfo[] props = from.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.PropertyType == toProps.Where(p => p.Name == prop.Name).FirstOrDefault().PropertyType) prop.SetValue(to, prop.GetValue(from));
            }
            return to;
        }
        /*
        public static object mapControlsToObjectForms(object userClass, System.Windows.Forms.Control.ControlCollection Controls)
        {
            PropertyInfo[] props = userClass.GetType().GetProperties();
            foreach (System.Windows.Forms.Control control in Controls)
            {
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name.ToLower() == control.ID.ToLower())
                    {
                        if (control.GetType() == typeof(TextBox))
                        {
                            TextBox tb = (TextBox)control;
                            prop.SetValue(userClass, tb.Text);
                        }
                        if (control.GetType() == typeof(Label))
                        {
                            Label tb = (Label)control;
                            prop.SetValue(userClass, tb.Text);
                        }
                        if (control.GetType() == typeof(DropDownList))
                        {
                            DropDownList tb = (DropDownList)control;
                            prop.SetValue(userClass, tb.SelectedValue);
                        }
                        if (control.GetType() == typeof(ListBox))
                        {
                            ListBox tb = (ListBox)control;
                            prop.SetValue(userClass, tb.SelectedValue);
                        }

                    }
                }
            }
            return userClass;
        }
        public static object mapControlsToObjectWeb(object userClass, System.Web.UI.ControlCollection Controls)
        {
            PropertyInfo[] props = userClass.GetType().GetProperties();
            foreach (Control control in Controls)
            {
                if (control.ID != null)
                {
                    foreach (PropertyInfo prop in props)
                    {
                        if (prop.Name.ToLower() == control.ID.ToLower())
                        {
                            if (control.GetType() == typeof(TextBox))
                            {
                                TextBox tb = (TextBox)control;
                                if (prop.PropertyType.ToString().ToLower().Contains("string"))
                                {
                                    prop.SetValue(userClass, tb.Text);
                                }
                                if (prop.PropertyType.ToString().ToLower().Contains("int"))
                                {
                                    int i = 0;
                                    int.TryParse(tb.Text, out i);
                                    prop.SetValue(userClass, i);
                                }
                            }
                            if (control.GetType() == typeof(Label))
                            {
                                Label tb = (Label)control;
                                prop.SetValue(userClass, tb.Text);
                            }
                            if (control.GetType() == typeof(DropDownList))
                            {
                                DropDownList tb = (DropDownList)control;
                                prop.SetValue(userClass, tb.SelectedValue);
                            }
                            if (control.GetType() == typeof(ListBox))
                            {
                                ListBox tb = (ListBox)control;
                                prop.SetValue(userClass, tb.SelectedValue);
                            }

                        }
                    }
                }
            }
            return userClass;
        }
         * */



    }
}
