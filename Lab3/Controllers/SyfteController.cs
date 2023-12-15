using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab3.Controllers
{
    public class SyfteController : Controller
    {
        // GET: /<controller>/
        public IActionResult Test()
        {
            DataSet ds = new DataSet();
            var GetConnection = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            SqlConnection dbConnection = new SqlConnection(GetConnection.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlstring = "Select * From Kategori";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            try
            {
                dbConnection.Open();
                myAdapter.Fill(ds, "memo");
                ViewBag.i = ds.Tables["memo"].Rows.Count.ToString();
            }
            catch (Exception e)
            {
                ViewBag.e = e.Message;
            }
            finally
            {
                dbConnection.Close();
            }
            return View();
        }
    }
}
