using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class TableMerge
    {
        string connetionString = null;
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt;
        string firstSql = null;
        string secondSql = null;
        int i = 0;

        public DataTable MergeTable()
        {
            // string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string conn = "";
            firstSql = "SELECT * FROM PresentModels;";
            secondSql  = "SELECT * FROM ProxyModels;";

            connection = new SqlConnection(conn);
            try
            {
                connection.Open();
                command = new SqlCommand(firstSql, connection);
                adapter.SelectCommand = command;
                adapter.Fill(ds, "Table(0)");
                adapter.SelectCommand.CommandText = secondSql;
                adapter.Fill(ds, "Table(1)");
                adapter.Dispose();
                command.Dispose();
                connection.Close();

                ds.Tables[0].Merge(ds.Tables[1]);
                return dt = ds.Tables[0];
            }
            catch (Exception e)
            {
                return dt = ds.Tables[0];
            }
        }

    }
}