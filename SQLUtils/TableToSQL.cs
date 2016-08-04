using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLUtils
{
    public static class TableToSQL
    {
       
        public static string KEY_Tables = "Tables";
        public static string KEY_Columns = "Columns";
        public static string KEY_Types = "Types";
        public static string KEY_Identity = "Identity";
        public static string KEY_IsNull = "IsNull";
        public static string KEY_Default = "Default";
        public static string KEY_Key = "Key";
        public static string ToSql(DataTable dt)
        {

            //显示有哪些表名
            DataTable Table = dt.DefaultView.ToTable(true, KEY_Tables);
         
            //显示有某表哪些字段
            StringBuilder sb = new StringBuilder();
            DataTable Column = dt.DefaultView.ToTable(true, KEY_Columns);
            foreach (DataRow tables in Table.Rows)
            {

                sb.Append("create table");
                sb.Append(" " + tables[KEY_Tables] + " (\r\n");

                foreach (DataRow dr in dt.Select("Tables='" + tables[KEY_Tables] + "'"))
                {
                    string defaults = dr[KEY_Default] + "" == "" ? "" : "Default((" + dr[KEY_Default] + "))";

                    string Key = "";
                    if (((string)dr[KEY_Key])=="PK")
                     Key = "PRIMARY KEY";

                    string sql = $"{dr[KEY_Columns]}  {dr[KEY_Types]}  {dr[KEY_Identity]}  {dr[KEY_IsNull]}  {defaults} {Key}, \r\n";
                    sb.Append(sql);

                }
                sb.Remove(sb.Length - 2, 1);
                sb.Append(")\r\n");
            }
            //显示某字段是什么类型
            return sb.ToString();
        }
    }
}
