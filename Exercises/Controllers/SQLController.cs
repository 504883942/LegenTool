using Newtonsoft.Json;
using SQLUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exercises.Controllers
{
    public class SQLController : Controller
    {
        // GET: SQL
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult UpLoad(HttpPostedFileBase file)
        {
            JsonResult ret = new JsonResult();
            var fileName = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file.FileName));
            ret.Data = JsonConvert.SerializeObject(CSV(file.InputStream));

            return ret;
        }
        public JsonResult ToSql(HttpPostedFileBase file)
        {
            JsonResult ret = new JsonResult();
            var fileName = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file.FileName));

            ret.Data = TableToSQL.ToSql(CSV(file.InputStream));
            return ret;
        }
        public FileResult DownExample()
        {
            MemoryStream output = new System.IO.MemoryStream();
            StreamWriter writer = new System.IO.StreamWriter(output, System.Text.Encoding.UTF8);
            string s = $"{TableToSQL.KEY_Tables},{TableToSQL.KEY_Columns},{TableToSQL.KEY_Types},{TableToSQL.KEY_Key},{TableToSQL.KEY_Identity},{TableToSQL.KEY_IsNull},{TableToSQL.KEY_Default}";
            writer.Write(s);//输出标题，逗号分割（注意最后一列不加逗号）
            writer.WriteLine();

            writer.Flush();
            output.Position = 0;
            return File(output, "text/comma-separated-values", "实例文档.csv");
        }
        public DataTable CSV(Stream str)
        {

            //try
            //{
            String line;
            String[] split = null;
            DataTable table = new DataTable();
            DataRow row = null;
            StreamReader sr = new StreamReader(str, System.Text.Encoding.Default);
            //创建与数据源对应的数据列 
            line = sr.ReadLine();
            split = line.Split(',');
            foreach (String colname in split)
            {
                table.Columns.Add(colname, System.Type.GetType("System.String"));
            }
            //将数据填入数据表 
            int j = 0;
            while ((line = sr.ReadLine()) != null)
            {
                j = 0;
                row = table.NewRow();
                split = line.Split(',');

                //解决csv单元格中有逗号的情况
                List<string> columns = new List<string>();
                bool longword = false;
                string temp = "";
                foreach (var item in split)
                {

                    if (item != "" && item[0] == '\"')
                    {
                        longword = true;

                    }

                    if (longword)
                    {
                        temp += "," + item;
                        if (item != "" && item[item.Length - 1] == '\"')
                        {
                            longword = false;

                            columns.Add(temp.Trim(new char[] { ',', '"', ' ' }));
                            temp = "";
                        }
                    }
                    else
                    {
                        columns.Add(item);
                    }

                }


                foreach (String colname in columns)
                {
                    if (colname != null)
                        row[j] = colname;
                    j++;
                }
                table.Rows.Add(row);
            }
            sr.Close();
            //显示数据 
            return table;
 
            GC.Collect();

        }
  //      public ActionResult Test()
  //      {
  //          string con = "Server=192.168.1.70;Database=dpnetwork_data;uid=sa;pwd=bmc!1234+";
  //          string sqlcommend = @"SELECT TOP 1000 [Fld_Id]
  //    ,[Fld_MaxValue]
  //    ,[Fld_MinValue]
  //    ,[Fld_Time]
  //    ,[Fld_TotalValue]
  //    ,[Fld_OriTotalValue]
  //    ,[Fld_AFUid]
  //    ,[Fld_FlowMeterUid]
  //FROM [dpnetwork_data].[dbo].[FlowDay_t]";
  //          DataTable dt = Reader(con, sqlcommend);


  //          return View();
  //      }
        public JsonResult SQL(string sql, string con)
        {

            JsonResult js = new JsonResult();
            js.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (sql.ToLower().Contains("select"))
            {
                js.Data = JsonConvert.SerializeObject(Reader(con, sql));
            }
            else
            {
                js.Data = Writer(con, sql);
            }

            return js;
        }
        Func<string, string, DataTable> Reader = (cs, sql) =>
        {
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
        Func<string, string, int> Writer = (cs, sql) =>
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