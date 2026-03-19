using SQLite;

namespace CombatManager
{
    public static class ExtSQL
    {
        public static RowsRet ExecuteCommand(this SQLiteConnection sql, string command)
        {
            System.Diagnostics.Debug.WriteLine(command);
            SQLiteCommand cmd =  sql.CreateCommand();
            cmd.CommandText = command;
			
            SQLiteDataReader rd = cmd.ExecuteReader();
			
            return new RowsRet(rd);;
			
        }
		
        public static RowsRet ExecuteCommand(this SQLiteConnection sql, string command, object[] param)
        {
            SQLiteCommand cmd =  sql.CreateCommand();
            cmd.CommandText = command;
            foreach (object obj in param)
            {
                cmd.Parameters.Add(CreateParam(obj));
            }
            SQLiteDataReader rd = cmd.ExecuteReader();
			
            return new RowsRet(rd);
			
        }
		
		
		
        public static bool DatabaseObjectExists(this SQLiteConnection sql, string table)
        {
            RowsRet rows = null;
            rows = sql.ExecuteCommand(
                "SELECT COUNT(*) FROM SQLite_master WHERE name=?", new object[] {table});
            int m_startRow = 1;
			
            if (rows == null || rows.Rows.Count < ((m_startRow == 0) ? 2 : 1))
            {
                return false;
            }
            else
            {
                int count = int.Parse(rows.Rows[(m_startRow == 0) ? 1 : 0].Cols[0]);

                if (count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

			 
    }
}