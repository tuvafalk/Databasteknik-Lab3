using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Lab3.Models
{
	public class Syfte
	{
        public Syfte()
		{
            var GetConnection = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();

            SqlConnection dbConnection = new SqlConnection(GetConnection.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlquery = "CREATE TABLE [dbo].[Kategori] ([KategoriId] INT IDENTITY(1, 1) NOT NULL,[Topic] NVARCHAR(MAX) NULL)";
        }
    }
}

