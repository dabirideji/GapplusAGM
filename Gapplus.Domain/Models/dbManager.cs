using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Gapplus.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BarcodeGenerator.Models
{
    public class dbManager
    {


      

        public List<string> GetBacodeModelCompanies()
        {
            // string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string connStr= DatabaseManager.GetConnectionString();
            SqlConnection conn = new SqlConnection(connStr);
            List<string> companies = new List<string>();

            string query = "Select DISTINCT Company from [dbo].[BarcodeModels]";

            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandTimeout = 0;
            SqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                var company = (read.GetValue(0).ToString());

                companies.Add(company);
            }
            read.Close();
            conn.Close();
            return companies;
        }

        public DataTable DeleteNullValue(DataTable dt)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (dt.Rows[i][1] == DBNull.Value)
                {
                    dt.Rows[i].Delete();
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        public int GetCompanyCount(string companyinfo)
        {
            // string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string connStr = DatabaseManager.GetConnectionString();

            SqlConnection conn = new SqlConnection(connStr);
            int count = 0;

            string query =
                "SELECT COUNT(*) FROM [dbo].[BarcodeModels] WHERE Company = '" + companyinfo + "'";

            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandTimeout = 0;
            SqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                count = read.GetInt32(0);
            }
            read.Close();
            conn.Close();
            return count;
        }

        public List<BarcodeModel> GetBacodeModelWithOutAccessCode(string company)
        {
            // string connStr = ConfigurationManager.ConnectionStrings[
            //     "DefaultConnection"
            // ].ConnectionString;
            string connStr =  DatabaseManager.GetConnectionString();;

            SqlConnection conn = new SqlConnection(connStr);
            List<BarcodeModel> Shareholders = new List<BarcodeModel>();
            var companyinfo = company.ToLower();

            string query =
                "Select * from [dbo].[BarcodeModels] Where Company ='"
                + company
                + "' AND (emailAddress != '' OR emailAddress != NULL) AND (accesscode = '' OR accesscode = NULL)";

            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandTimeout = 0;
            SqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                BarcodeModel model = new BarcodeModel
                {
                    SN = (Int64.Parse(read["SN"].ToString())),
                    Id = int.Parse(read["Id"].ToString()),
                    Name = (read["Name"].ToString()),
                    Address = (read["Address"].ToString()),
                    ShareholderNum = (Int64.Parse(read["ShareholderNum"].ToString())),
                    Holding = double.Parse(read["Holding"].ToString()),
                    PercentageHolding = double.Parse(read["PercentageHolding"].ToString()),
                    Company = (read["Company"].ToString()),
                    PhoneNumber = (read["PhoneNumber"].ToString()),
                    emailAddress = (read["emailAddress"].ToString()),
                    Clikapad = (read["Clikapad"].ToString()),
                    Present = bool.Parse(read["Present"].ToString()),
                    Selected = bool.Parse(read["Selected"].ToString()),
                    Consolidated = bool.Parse(read["Consolidated"].ToString()),
                    PresentByProxy = bool.Parse(read["PresentByProxy"].ToString()),
                    split = bool.Parse(read["split"].ToString()),
                    resolution = bool.Parse(read["resolution"].ToString()),
                    TakePoll = bool.Parse(read["TakePoll"].ToString()),
                    NotVerifiable = bool.Parse(read["NotVerifiable"].ToString())
                };
                Shareholders.Add(model);
            }
            read.Close();
            conn.Close();

            return Shareholders;
        }

        public bool UpdateBacodeModelWithAccessCode(int id, string accesscode, string url)
        {
            // string connStr = ConfigurationManager.ConnectionStrings[
            //     "DefaultConnection"
            // ].ConnectionString;
            string connStr =  DatabaseManager.GetConnectionString();;

            SqlConnection conn = new SqlConnection(connStr);

            string query =
                "UPDATE [dbo].[BarcodeModels] SET [accesscode] = '"
                + accesscode
                + "',[OnlineEventUrl] = '"
                + url
                + "' WHERE Id='"
                + id
                + "'";

            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandTimeout = 0;
            try
            {
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                conn.Close();
                return false;
            }
        }
    }
}
