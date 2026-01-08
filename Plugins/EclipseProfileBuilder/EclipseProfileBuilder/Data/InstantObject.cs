using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ArachnidCreations.DevTools
{
    public class InstantObject
    {
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
            returnSQL += (string.Format("CREATE TABLE {0} ( ", tablename));
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
                    returnSQL += (string.Format("[{1}{2}] {3}{0}", comma, fieldprefix, prop.Name, sqlvartype));
                }
                else if (prop.PropertyType.ToString() == "System.Int32")
                {
                    string sqlvartype = "[int]";
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
            returnSQL += (string.Format(");", tablename));
            return returnSQL;
        }
        public static object convertDataTabletoObject(object userClass, DataTable dataTable, string fieldprefix = "")
        {
            int icount = 0;
            DataTable dt = dataTable;
            //if (dt != null && userClass != null && userClass != null)
            {

                Type tType = typeof(object);
                foreach (DataRow row in dt.Rows)
                {
                    icount++;

                    List<string> ColumnNames = new List<string>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string columnName = dt.Columns[i].ColumnName;
                        if (fieldprefix != "") columnName = columnName.ToLower().Replace(fieldprefix.ToLower(), "");
                        ColumnNames.Add(columnName.ToLower());
                    }
                    PropertyInfo[] props = userClass.GetType().GetProperties();
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
                return userClass;
            }
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
                    }//
                }
                return userClass;
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
