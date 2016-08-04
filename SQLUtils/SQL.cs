using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLUtils
{
    public static class SQL
    {
        public static Func<string, string, DataTable> Reader = (cs, sql) => {
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = con;
            SqlDataAdapter dbAdapter = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            dbAdapter.Fill(ds);
            con.Close();
            return ds;
        };
        public static Func<string, string, int> Writer = (cs, sql) =>
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = con;
            int num = cmd.ExecuteNonQuery();
            con.Close();
            return num;
        };
    }
}
