///*
// *  DBLoader.cs
// *
// *  Copyright (C) 2010-2012 Kyle Olson, kyle@kyleolson.com
// *
// *  This program is free software; you can redistribute it and/or
// *  modify it under the terms of the GNU General Public License
// *  as published by the Free Software Foundation; either version 2
// *  of the License, or (at your option) any later version.
// *
// *  This program is distributed in the hope that it will be useful,
// *  but WITHOUT ANY WARRANTY; without even the implied warranty of
// *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// *  GNU General Public License for more details.
// * 
// *  You should have received a copy of the GNU General Public License
// *  along with this program; if not, write to the Free Software
// *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// *
// */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Reflection;
//using System.Xml.Serialization;
//using ScottsUtils;
//using System.IO;
//using System.Collections;
//using SQLite;
//#if MONO
//using System.Data;
//#endif

//namespace CombatManager
//{

//#if MONO


//#endif


//    public class DBLoader<T> : IDisposable where T : IDBLoadable
//    {
//        private DBTableDesc _RootTableDesc;
//        private Dictionary<string, int> nextIndexForTable = new Dictionary<string, int>();
//        private Dictionary<int, T> itemDictionary = new Dictionary<int, T>();

//#if !MONO
//        private SQL_Lite sql;
//#else
//        SQLiteConnection sql;
//#endif

//        public static string CreateTableStatementForDesc(DBTableDesc table)
//        {
//            string str = "CREATE TABLE " + table.Name + "(ID INTEGER PRIMARY KEY ASC";

//            if (!table.Primary)
//            {
//                str += ", OwnerID INTEGER";
//            }

//            foreach (DBFieldDesc field in table.Fields)
//            {
//                str += ", " + field.Name + " " + field.Type + (field.Nullable ? "" : " NOT NULL");

//            }

//            str += ");";

//            return str;


//        }

//        public DBLoader(string filename)
//        {
//            Type type = typeof(T);

//#if ANDROID
//            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
//#else
//            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
//#endif
//            path = Path.Combine(path, "Combat Manager");

//            if (!Directory.Exists(path))
//            {
//                Directory.CreateDirectory(path);
//            }


//            try
//            {
//                string fullfilename = Path.Combine(path, filename);

//#if !MONO
//                sql = new SQL_Lite();
//                sql.SkipHeaderRow = true;
//                sql.Open(fullfilename);
//                string backtext = Assembly.GetExecutingAssembly().GetName().Version.ToString();
//                string backname = fullfilename + backtext + ".db";
//#else

//#if ANDROID
//                Android.Util.Log.Error("DBLoader", "fullfilename " + fullfilename);
//#endif
//                sql = new SQLiteConnection("DbLinqProvider=Sqlite;Data Source=" + fullfilename);

//#endif





//#if !MONO
//                //make a backup
//                if (File.Exists(fullfilename) && !(File.Exists(backname)))
//                {
//                    try
//                    {
//                        File.Copy(fullfilename, backname);
//                    }
//                    catch (IOException ex)
//                    {
//                        System.Diagnostics.Debug.WriteLine(ex.ToString());
//                    }
//                }
//#endif



//                bool needsCopy = false;

//#if !MONO
//                try
//                {
//                    sql.ExecuteCommand("SELECT name FROM sqlite_master");
//                }
//                catch (SQL_Lite_Exception ex)
//                {
//                    System.Diagnostics.Debug.WriteLine(ex.ToString());
//                    sql.Dispose();
//                    sql = null;

//                    string errorname = fullfilename + ".error.db";

//                    if (File.Exists(errorname))
//                    {
//                        File.Delete(errorname);
//                    }
//                    File.Move(fullfilename, errorname);

//                    sql = new SQL_Lite();
//                    sql.SkipHeaderRow = true;

//                    sql.Open(fullfilename);

//                }
//#endif

//                List<DBTableDesc> tables = GetTablesForType(type);

//                foreach (DBTableDesc desc in tables)
//                {
//                    RowsRet ret = sql.ExecuteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name=?",
//                        new object[] { desc.Name });

//                    if (ret.Rows.Count == 0)
//                    {

//                        string str = CreateTableStatementForDesc(desc);
//                        sql.ExecuteCommand(str);
//                    }
//                    else
//                    {
//                        if (!TableMatchesDescription(sql, desc))
//                        {
//                            needsCopy = true;
//                            break;
//                        }

//                    }

//                }

//                if (needsCopy)
//                {
//#if ANDROID
//                    Android.Util.Log.Error("DBLoader", "DBL needs copy");

//#endif
//                    string newfile = fullfilename + ".tmp";
//                    if (File.Exists(newfile))
//                    {

//#if ANDROID
//                        Android.Util.Log.Error("DBLoader", "DBL new file exists");

//#endif
//                        File.Delete(newfile);

//#if ANDROID
//                        Android.Util.Log.Error("DBLoader", "DBL new file delete");

//#endif
//                    }

//#if !MONO
//                    SQL_Lite sql2 = new SQL_Lite();
//                    sql2.SkipHeaderRow = true;

//                    sql2.Open(newfile);
//#else
//                    LogError("DBLoader", "NewFile " + newfile);


//                    SQLiteConnection sql2 = new SQLiteConnection("DbLinqProvider=Sqlite;Data Source=" + newfile);


//#endif

//                    foreach (DBTableDesc table in tables)
//                    {
//                        CopyTable(sql, sql2, table);

//                    }

//                    sql.Dispose();
//                    sql2.Dispose();

//                    File.Delete(fullfilename);
//                    File.Move(newfile, fullfilename);


//                    sql = new SQLiteConnection("DbLinqProvider=Sqlite;Data Source=" + fullfilename);

//                }


//                LoadTableNextIndexes(tables);
//                LoadDBItems();
//            }
//#if !MONO
//            catch (SQL_Lite_Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine(ex);
//            }
//#else
//            finally
//            {
//            }
//#endif

//        }
//#if MONO
//        private bool TableMatchesDescription(SQLiteConnection sql, DBTableDesc table)
//#else
//        private bool TableMatchesDescription(SQL_Lite sql, DBTableDesc table)
//#endif
//        {
//            RowsRet ret = sql.ExecuteCommand("PRAGMA table_info(" + table.Name + ");");

//            Dictionary<string, DBFieldDesc> fieldInfo = new Dictionary<string, DBFieldDesc>();

//            foreach (var field in table.Fields)
//            {
//                fieldInfo.Add(field.Name, field);

//            }



//            bool tableMatches = true;
//            foreach (Row row in ret.Rows)
//            {
//                string name = row["name"];
//                if (fieldInfo.ContainsKey(row["name"]))
//                {

//                    fieldInfo.Remove(row["name"]);
//                }
//                else
//                {
//                    if (name != "ID" && name != "OwnerID")
//                    {
//                        tableMatches = false;
//                        break;
//                    }
//                }
//            }

//            if (fieldInfo.Count > 0)
//            {
//                tableMatches = false;
//            }

//            return tableMatches;
//        }

//        private void LogError(string source, string error)
//        {
//#if !MONO
//            System.Diagnostics.Debug.WriteLine(source + ": " + error);
//#elif ANDROID
//              Android.Util.Log.Error("DBLoader", "DBL new file exists");

//#endif
//        }

//#if !MONO
//        private static void CopyTable(SQL_Lite sql, SQL_Lite sql2, DBTableDesc table)
//#else

//        private static void CopyTable(SQLiteConnection sql, SQLiteConnection sql2, DBTableDesc table)
//#endif
//        {
//#if ANDROID
//            Android.Util.Log.Error("DBLoader", "CopyTable");
            
//#endif

//            string str = CreateTableStatementForDesc(table);
//            sql2.ExecuteCommand(str);

//#if ANDROID
//            Android.Util.Log.Error("DBLoader", "Created");

//#endif

//            if (sql.DatabaseObjectExists(table.Name))
//            {
//                RowsRet data = sql.ExecuteCommand("Select * from " + table.Name);

//#if ANDROID
//                Android.Util.Log.Error("DBLoader", "Select from table.name");

//#endif

//                List<string> validOldFields = new List<string>();
//                List<DBFieldDesc> invalidNewFields = new List<DBFieldDesc>();

//                foreach (DBFieldDesc desc in table.Fields)
//                {
//                    if (data.HasColumn(desc.Name))
//                    {
//                        validOldFields.Add(desc.Name);
//                    }
//                    else
//                    {
//                        invalidNewFields.Add(desc);
//                    }
//                }


//                StringBuilder commandBuilder = new StringBuilder();
//                commandBuilder.Append("Insert into " + table.Name + " (ID");
//                int count = validOldFields.Count + 1; ;
//                if (!table.Primary)
//                {
//                    commandBuilder.Append(", OwnerID");
//                    count++;
//                }
//                StringBuilder fieldBuilder = new StringBuilder();
//                StringBuilder valueBuilder = new StringBuilder();
//                foreach (string strField in validOldFields)
//                {
//                    fieldBuilder.Append(", " + strField);
//                }
//                for (int i = 1; i < count; i++)
//                {
//                    valueBuilder.Append(", ?");
//                }

//                foreach (DBFieldDesc desc in invalidNewFields)
//                {
//                    if (desc.Nullable == false)
//                    {
//                        if (desc.Type == "INTEGER")
//                        {

//                            fieldBuilder.Append(", " + desc.Name);
//                            valueBuilder.Append(", 0");

//                        }
//                    }
//                }

//                fieldBuilder.Append(") VALUES ( ?");
//                valueBuilder.Append(");");
//                commandBuilder.Append(fieldBuilder);
//                commandBuilder.Append(valueBuilder);
//                string command = commandBuilder.ToString();

//                foreach (Row row in data.Rows)
//                {
//                    List<object> values = new List<object>();
//                    values.Add(row["ID"]);
//                    if (!table.Primary)
//                    {
//                        values.Add(row["OwnerID"]);
//                    }

//                    foreach (string strField in validOldFields)
//                    {
//                        values.Add(row[strField]);
//                    }

//                    object[] objParams = values.ToArray();
//                    sql2.ExecuteCommand(command, objParams);

//#if ANDROID
//                    Android.Util.Log.Error("DBLoader", "row command");

//#endif
//                }
//            }
//        }

//        private static List<DBTableDesc> GetTablesForType(Type startType)
//        {
//            return GetTablesForType(startType, null, false);
//        }

//        private DBTableDesc RootTableDesc
//        {
//            get
//            {
//                if (_RootTableDesc == null)
//                {
//                    _RootTableDesc = GetTablesForType(typeof(T), null, true)[0];

//                }

//                return _RootTableDesc;
//            }
//        }

//        private static List<DBTableDesc> GetTablesForType(Type startType, string baseName, bool addSubtables)
//        {

//            Dictionary<string, DBTableDesc> tableDesc = new Dictionary<string, DBTableDesc>();

//            DBTableDesc mainTable = new DBTableDesc(startType.Name, startType);
//            if (baseName != null && baseName.Length > 0)
//            {
//                mainTable.Name = baseName + mainTable.Name;
//            }
//            else
//            {
//                mainTable.Primary = true;
//            }


//            tableDesc.Add(mainTable.Name, mainTable);



//            List<PropertyInfo> propInfo = GetUsableFields(startType);
//            System.Diagnostics.Debug.Assert(propInfo.Count > 0);

//            string subtableBaseName = startType.Name + "__";

//            if (baseName != null && baseName.Length > 0)
//            {
//                subtableBaseName = baseName + subtableBaseName;
//            }


//            foreach (PropertyInfo info in propInfo)
//            {
//                Type type = info.PropertyType;
//                DBFieldDesc desc = GetDescForType(info.Name, type, info);
//                DBTableDesc fieldSubtable = null;

//                if (desc != null)
//                {
//                    mainTable.Fields.Add(desc);
//                }
//                else if (IsEnumerationTableType(type))
//                {
//                    DBFieldDesc subdesc = GetDescForType(info.Name, type.GetGenericArguments()[0], info);

//                    if (subdesc != null)
//                    {
//                        DBTableDesc subtable = new DBTableDesc(subtableBaseName + info.Name, info.PropertyType);
//                        subtable.Fields.Add(subdesc);
//                        tableDesc.Add(subtable.Name, subtable);


//                        fieldSubtable = subtable;
//                    }
//                    else
//                    {
//                        List<DBTableDesc> newList = GetTablesForType(type.GetGenericArguments()[0], subtableBaseName, addSubtables);
//                        System.Diagnostics.Debug.Assert(newList.Count > 0);


//                        fieldSubtable = newList[0];

//                        foreach (DBTableDesc subtable in newList)
//                        {
//                            System.Diagnostics.Debug.Assert(subtable.Fields.Count > 0);
//                            if (!tableDesc.ContainsKey(subtable.Name))
//                            {
//                                tableDesc.Add(subtable.Name, subtable);
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    List<DBTableDesc> newList = GetTablesForType(type, subtableBaseName, addSubtables);

//                    System.Diagnostics.Debug.Assert(newList.Count > 0);
//                    foreach (DBTableDesc subtable in newList)
//                    {
//                        if (!tableDesc.ContainsKey(subtable.Name))
//                        {
//                            tableDesc.Add(subtable.Name, subtable);
//                        }
//                    }

//                    fieldSubtable = newList[0];
//                }

//                if (fieldSubtable != null && addSubtables)
//                {
//                    DBFieldDesc fieldDesc = new DBFieldDesc();
//                    fieldDesc.Name = info.Name;
//                    fieldDesc.Subtable = fieldSubtable;
//                    fieldDesc.Info = info;
//                    mainTable.Fields.Add(fieldDesc);
//                }
//            }


//            List<DBTableDesc> tables = new List<DBTableDesc>();
//            foreach (DBTableDesc desc in tableDesc.Values)
//            {
//                tables.Add(desc);
//            }

//            return tables;
//        }

//        private static DBFieldDesc GetDescForType(string fieldname, Type type, PropertyInfo info)
//        {

//            if (type == typeof(int) || type == typeof(long) || type == typeof(bool))
//            {
//                return new DBFieldDesc(fieldname, "INTEGER", false, info);
//            }
//            else if (type == typeof(Nullable<int>) || type == typeof(Nullable<long>))
//            {
//                return new DBFieldDesc(fieldname, "INTEGER", true, info);
//            }
//            else if (type == typeof(string))
//            {
//                return new DBFieldDesc(fieldname, "TEXT", true, info);
//            }

//            return null;
//        }



//        private static List<PropertyInfo> GetUsableFields(Type type)
//        {
//            List<PropertyInfo> fields = new List<PropertyInfo>();

//            Type t = type;

//            foreach (PropertyInfo info in t.GetProperties())
//            {

//                bool ignore = false;

//                if (!info.CanRead || !info.CanWrite)
//                {
//                    ignore = true;
//                }

//                if (!ignore)
//                {
//                    if (info.GetGetMethod().IsStatic)
//                    {
//                        ignore = true;
//                    }
//                }


//                if (!ignore)
//                {
//                    foreach (object attr in info.GetCustomAttributes(typeof(XmlIgnoreAttribute), true))
//                    {
//                        ignore = true;
//                        break;
//                    }
//                }

//                if (!ignore)
//                {
//                    foreach (object attr in info.GetCustomAttributes(typeof(DBLoaderIgnoreAttribute), true))
//                    {
//                        ignore = true;
//                        break;
//                    }
//                }

//                if (!ignore)
//                {
//                    if (info.Name == "DBLoaderID")
//                    {
//                        ignore = true;
//                    }

//                }

//                if (!ignore)
//                {
//                    fields.Add(info);
//                }
//            }

//            return fields;
//        }

//        private void LoadDBItems()
//        {

//            List<T> values = LoadValues();

//            foreach (T item in values)
//            {
//                itemDictionary.Add(item.DBLoaderID, item);
//            }
//        }

//        private List<T> LoadValues()
//        {
//            List<T> items = new List<T>();
//            foreach (T item in LoadValues(RootTableDesc, 0))
//            {
//                items.Add(item);
//            }
//            return items;
//        }

//        private List<object> LoadValues(DBTableDesc table, int owner)
//        {
//            List<object> valueList = new List<object>();


//            RowsRet ret;


//            if (table.Primary)
//            {
//                ret = sql.ExecuteCommand("SELECT * FROM " + table.Name + ";");
//            }
//            else
//            {
//                ret = sql.ExecuteCommand("SELECT * FROM " + table.Name + " WHERE OwnerID = ?;", new object[] { owner });

//            }

//            foreach (Row row in ret.Rows)
//            {
//                ConstructorInfo info = table.Type.GetConstructor(new Type[] { });
//                object item = info.Invoke(new object[] { });

//                int index = row.IntValue("ID");


//                foreach (DBFieldDesc field in table.ValueFields)
//                {
//                    string stringVal = row[field.Name];

//                    object obVal = ParseObjectForType(stringVal, field.Info.PropertyType);


//                    field.Info.GetSetMethod().Invoke(item, new object[] { obVal });
//                }

//                foreach (DBFieldDesc field in table.SubtableFields)
//                {
//                    if (IsEnumerationTableType(field.Info.PropertyType))
//                    {
//                        List<object> list;

//                        if (IsSimpleValueEnumeration(field.Info.PropertyType))
//                        {
//                            list = LoadSimpleValues(field.Subtable, index);
//                        }
//                        else
//                        {
//                            list = LoadValues(field.Subtable, index);
//                        }


//                        ConstructorInfo cons = field.Info.PropertyType.GetConstructor(new Type[] { });
//                        IList newList = (IList)cons.Invoke(new object[0]);
//                        foreach (object listitem in list)
//                        {
//                            newList.Add(listitem);
//                        }

//                        field.Info.GetSetMethod().Invoke(item, new object[] { newList });

//                    }
//                    else
//                    {
//                        List<object> list = LoadValues(field.Subtable, index);

//                        if (list.Count > 0)
//                        {

//                            field.Info.GetSetMethod().Invoke(item, new object[] { list[0] });
//                        }
//                    }
//                }

//                if (table.Primary)
//                {
//                    IDBLoadable loadable = (IDBLoadable)item;
//                    loadable.DBLoaderID = index;
//                }

//                valueList.Add(item);


//            }

//            return valueList;
//        }

//        public List<object> LoadSimpleValues(DBTableDesc table, int owner)
//        {

//            string field = table.Fields[0].Name;
//            Type type = table.Type.GetGenericArguments()[0];

//            RowsRet ret = sql.ExecuteCommand("SELECT " + field + " FROM " + table.Name + " WHERE OwnerID = ?;", new object[] { owner });

//            List<object> list = new List<object>();


//            foreach (Row row in ret.Rows)
//            {
//                list.Add(ParseObjectForType(row.Cols[0], type));
//            }

//            return list;
//        }

//        public object ParseObjectForType(string text, Type t)
//        {
//            try
//            {
//                if (t == typeof(int))
//                {
//                    return int.Parse(text);
//                }
//                if (t == typeof(long))
//                {
//                    return long.Parse(text);
//                }
//                if (t == typeof(Nullable<int>))
//                {
//                    Nullable<int> val = null;

//                    int num;
//                    if (int.TryParse(text, out num))
//                    {
//                        val = num;
//                    }

//                    return val;
//                }
//                if (t == typeof(Nullable<long>))
//                {
//                    Nullable<long> val = null;

//                    long num;
//                    if (long.TryParse(text, out num))
//                    {
//                        val = num;
//                    }

//                    return val;
//                }
//                if (t == typeof(bool))
//                {
//                    return text == "1";
//                }
//                if (t == typeof(string))
//                {
//                    return text;
//                }
//            }
//            catch (Exception)
//            {
//                throw;
//            }

//            System.Diagnostics.Debug.Assert(false);
//            return null;


//        }

//        public int AddItem(T item)
//        {

//            sql.ExecuteCommand("BEGIN TRANSACTION");
//            int index = InsertItem(item);
//            sql.ExecuteCommand("END TRANSACTION");
//            itemDictionary.Add(index, item);
//            return index;
//        }

//        public void AddItems(IEnumerable<T> items)
//        {
//            sql.ExecuteCommand("BEGIN TRANSACTION");
//            foreach (T item in items)
//            {
//                InsertItem(item);
//            }
//            sql.ExecuteCommand("END TRANSACTION");
//            foreach (T item in items)
//            {
//                itemDictionary.Add(item.DBLoaderID, item);
//            }
//        }

//        private void LoadTableNextIndexes(List<DBTableDesc> tables)
//        {
//            foreach (DBTableDesc table in tables)
//            {
//                UpdateNextIndexForTableFromDB(table.Name);
//            }
//        }

//        private void UpdateNextIndexForTableFromDB(string tableName)
//        {
//            string statement = "SELECT MAX(ID) FROM " + tableName + ";";

//            RowsRet ret = sql.ExecuteCommand(statement);
//            if (ret.Rows.Count == 0)
//            {
//                nextIndexForTable[tableName] = 1;
//            }
//            else
//            {
//                var row = ret.Rows[0];
//                string text = row.Cols[0];
//                int index;
//                int.TryParse(text, out index);

//                nextIndexForTable[tableName] = index + 1;
//            }
//        }


//        private int InsertItem(T item)
//        {
//            DBTableDesc table = RootTableDesc;

//            int val = InsertItemForTable(item, table, 0);

//            return val;
//        }

//        private int InsertItemForTable(object item, DBTableDesc table, int owner)
//        {

//            List<object> insertParams = new List<object>();


//            int index;
//            if (table.Primary && ((IDBLoadable)item).DBLoaderID != 0)
//            {
//                index = ((IDBLoadable)item).DBLoaderID;
//            }
//            else
//            {
//                index = GetNextIndex(table);
//            }


//            int count = 1;
//            insertParams.Add(index);

//            if (!table.Primary)
//            {
//                insertParams.Add(owner);
//                count++;
//            }

//            StringBuilder strb = new StringBuilder();
//            strb.Append("INSERT INTO " + table.Name + "(ID");
//            if (!table.Primary)
//            {
//                strb.Append(", OwnerID");
//            }
//            List<DBFieldDesc> subtables = new List<DBFieldDesc>();
//            foreach (DBFieldDesc field in table.Fields)
//            {
//                if (field.Subtable == null)
//                {
//                    strb.Append(", " + field.Name);
//                    insertParams.Add(field.Info.GetGetMethod().Invoke(item, new object[] { }));
//                    count++;
//                }
//                else
//                {
//                    subtables.Add(field);
//                }
//            }

//            strb.Append(") VALUES (?");

//            for (int i = 1; i < count; i++)
//            {
//                strb.Append(", ?");
//            }

//            strb.Append(");");

//            string command = strb.ToString();
//            sql.ExecuteCommand(command, insertParams.ToArray());

//            InsertItemSubtables(item, table, index);


//            if (table.Primary)
//            {
//                IDBLoadable loadable = (IDBLoadable)item;
//                loadable.DBLoaderID = index;
//            }

//            return index;
//        }

//        private void InsertItemSubtables(object item, DBTableDesc table, int index)
//        {
//            foreach (DBFieldDesc desc in table.SubtableFields)
//            {

//                object subitem = desc.Info.GetGetMethod().Invoke(item, new object[] { });

//                if (subitem != null)
//                {
//                    if (IsEnumerationTableType(desc.Info.PropertyType))
//                    {
//                        foreach (object o in ((IEnumerable)subitem))
//                        {
//                            if (IsSimpleValueEnumeration(desc.Info.PropertyType))
//                            {
//                                InsertSimpleSubtableItem(o, desc.Subtable, index);
//                            }
//                            else
//                            {
//                                InsertItemForTable(o, desc.Subtable, index);
//                            }
//                        }

//                    }
//                    else
//                    {
//                        InsertItemForTable(subitem, desc.Subtable, index);
//                    }
//                }
//            }
//        }


//        private int InsertSimpleSubtableItem(object item, DBTableDesc table, int owner)
//        {
//            string command = "INSERT INTO " + table.Name + " (ID, OwnerID, " +
//                table.Fields[0].Name + ") VALUES (?,?,?);";

//            int index = GetNextIndex(table);

//            object[] insertParams = new object[] { index, owner, item };

//            sql.ExecuteCommand(command, insertParams);

//            return index;

//        }

//        public void DeleteItems(IEnumerable<T> items)
//        {

//            List<string> statementList = new List<string>();
//            List<object[]> paramList = new List<object[]>();
//            foreach (T item in items)
//            {
//                CreateDeleteTableItemStatements(RootTableDesc, item.DBLoaderID, statementList, paramList);

//            }


//            RunTransaction(statementList, paramList);

//            foreach (T item in items)
//            {
//                itemDictionary.Remove(item.DBLoaderID);
//            }
//        }

//        public void DeleteItem(T item)
//        {
//            if (item.DBLoaderID == 0)
//            {
//                throw new ArgumentException("Invalid DBLoaderID", "item");
//            }

//            List<string> statementList = new List<string>();
//            List<object[]> paramList = new List<object[]>();
//            CreateDeleteTableItemStatements(RootTableDesc, item.DBLoaderID, statementList, paramList);

//            RunTransaction(statementList, paramList);

//            itemDictionary.Remove(item.DBLoaderID);
//        }

//        public void RunTransaction(List<string> statementList, List<object[]> paramList)
//        {

//            sql.ExecuteCommand("BEGIN TRANSACTION;");

//            RunStatementList(statementList, paramList);

//            sql.ExecuteCommand("END TRANSACTION;");
//        }

//        public void RunStatementList(List<string> statementList, List<object[]> paramList)
//        {
//            for (int i = 0; i < statementList.Count; i++)
//            {
//                sql.ExecuteCommand(statementList[i], paramList[i]);
//            }
//        }

//        public void UpdateItem(T item)
//        {


//            sql.ExecuteCommand("BEGIN TRANSACTION;");

//            List<string> statementList = new List<string>();
//            List<object[]> paramList = new List<object[]>();
//            CreateDeleteTableItemStatements(RootTableDesc, item.DBLoaderID, statementList, paramList);

//            RunStatementList(statementList, paramList);
//            InsertItemForTable(item, RootTableDesc, 0);

//            sql.ExecuteCommand("END TRANSACTION;");

//        }


//        public void CreateDeleteTableItemStatements(DBTableDesc basetable, int index, List<string> statementList, List<object[]> paramList)
//        {
//            CreateDeleteTableItemStatements(basetable, index, statementList, paramList, false);
//        }

//        public void CreateDeleteTableItemStatements(DBTableDesc basetable, int index, List<string> statementList, List<object[]> paramList, bool ignoreCurrent)
//        {
//            object[] idParam = new object[] { index };
//            if (!ignoreCurrent)
//            {
//                statementList.Add("DELETE FROM " + basetable.Name + " WHERE ID = ?;");
//                paramList.Add(idParam);
//            }

//            foreach (DBFieldDesc field in basetable.SubtableFields)
//            {
//                DBTableDesc table = field.Subtable;
//                RowsRet ret = sql.ExecuteCommand("SELECT ID FROM " + table.Name + " WHERE OwnerID = ?", idParam);

//                foreach (Row row in ret.Rows)
//                {
//                    CreateDeleteTableItemStatements(table, row.IntValue("ID"), statementList, paramList);
//                }
//            }
//        }

//        private int GetNextIndex(DBTableDesc table)
//        {

//            int index = nextIndexForTable[table.Name];
//            nextIndexForTable[table.Name] = index + 1;

//            return index;
//        }

//        private static bool IsEnumerationTableType(Type type)
//        {
//            return type.GetInterface("IEnumerable") != null && type.IsGenericType == true;
//        }

//        private static bool IsSimpleValueEnumeration(Type enumType)
//        {
//            Type type = enumType.GetGenericArguments()[0];

//            return type == typeof(int) || type == typeof(long) || type == typeof(bool) ||
//                type == typeof(Nullable<int>) || type == typeof(Nullable<long>) ||
//                type == typeof(string);
//        }

//        public void Dispose()
//        {
//            if (sql != null)
//            {
//                sql.Dispose();
//                sql = null;
//            }

//        }

//        public IEnumerable<T> Items
//        {
//            get
//            {
//                if (itemDictionary == null)
//                {
//                    LoadDBItems();
//                }

//                return itemDictionary.Values;
//            }
//        }
//    }
//}
