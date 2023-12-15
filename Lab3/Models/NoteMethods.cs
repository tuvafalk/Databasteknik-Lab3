using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System;
using System.Data.SqlClient;
using Lab3.Models;
using System.Data.Common;

namespace Lab3.Models
{
    public class NoteMethods
    {
        public NoteMethods() { }

        // För att komma åt appsettings.json 
        public IConfigurationRoot GetConnection()

        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            return builder;
        }

        public int InsertNote (NoteDetail nd, out string errorMessage)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // SQL query to insert a note into the table
            String sqlquery = "INSERT INTO Tbl_note(note_title, note_content, note_owner) VALUES (@Title, @Content, @Owner)";

            // Creating a command object with the query and connection
            SqlCommand dbCommand = new SqlCommand(sqlquery, dbConnection);

            // Adding parameters to the SQL query to prevent SQL injection
            dbCommand.Parameters.Add("Title", SqlDbType.NVarChar, 50).Value = nd.Title;
            dbCommand.Parameters.Add("Content", SqlDbType.NVarChar, 5000).Value = nd.Content;
            dbCommand.Parameters.Add("Owner", SqlDbType.Int).Value = nd.Owner;

            try
            {
                // Opening the database connection
                dbConnection.Open();

                int i = 0;
                // Executing the SQL command and getting the number of affected rows
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errorMessage = ""; }
                else { errorMessage = "No note added to the data base"; }

                return i;
            }
            catch (Exception ex)
            {
                // If an exception occurs, set the error message to the exception message
                errorMessage = ex.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int DeleteNote (NoteDetail nd, out string errorMessage)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // SQL query to delete a note from the table
            String sqlquery = "DELETE FROM Tbl_note WHERE note_title = 'test'";

            // Creating a command object with the query and connection
            SqlCommand dbCommand = new SqlCommand(sqlquery, dbConnection);


            try
            {
                // Opening the database connection
                dbConnection.Open();

                int i = 0;
                // Executing the SQL command and getting the number of affected rows
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errorMessage = ""; }
                else { errorMessage = "No note added to the data base"; }

                return i;
            }
            catch (Exception ex)
            {
                // If an exception occurs, set the error message to the exception message
                errorMessage = ex.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<NoteDetail> GetNoteWithDataSet (out string errorMessage)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // SQL query to insert a note into the table
            String sqlquery = "SELECT * FROM Tbl_note";

            // Creating a command object with the query and connection
            SqlCommand dbCommand = new SqlCommand(sqlquery, dbConnection);

            SqlDataAdapter myAdapter = new(dbCommand);
            DataSet noteDS = new DataSet();

            List<NoteDetail> NoteList = new List<NoteDetail>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(noteDS, "tbl_notes");

                int count = 0;
                int i = 0;
                count = noteDS.Tables["tbl_notes"].Rows.Count;

                if (count > 0)
                {
                    while (i < count) 
                    {
                        NoteDetail nd = new NoteDetail();
                        nd.Title = noteDS.Tables["tbl_notes"].Rows[i]["note_title"].ToString();
                        nd.Content = noteDS.Tables["tbl_notes"].Rows[i]["note_content"].ToString();
                        nd.Owner = Convert.ToInt32(noteDS.Tables["tbl_notes"].Rows[i]["note_owner"]);

                        i++;
                        NoteList.Add(nd);

                    }
                    errorMessage = "";
                    return NoteList;
                }
                else
                {
                    errorMessage = "Cant fetch note";
                    return null;
                }
                   
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<NoteDetail> GetNoteWithFilter (out string errorMessage, int filterId)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlquery = "SELECT * FROM Tbl_note WHERE note_owner = @filterId";
            SqlCommand dbCommand = new SqlCommand(sqlquery, dbConnection);

            dbCommand.Parameters.Add("filterId", SqlDbType.Int).Value = filterId;

            SqlDataReader reader = null;

            List<NoteDetail> NoteList = new List<NoteDetail>();

            errorMessage = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    NoteDetail note = new NoteDetail();
                    note.Title = reader["note_title"].ToString();
                    note.Content = reader["note_content"].ToString();
                    note.Owner = Convert.ToInt16(reader["note_owner"]);

                    NoteList.Add(note);
                }
                reader.Close();
                return NoteList;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<NoteDetail> GetNoteSorted(out string errorMessage, string sqlquery)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // Creating a command object with the query and connection
            SqlCommand dbCommand = new SqlCommand(sqlquery, dbConnection);


            SqlDataAdapter myAdapter = new(dbCommand);
            DataSet noteDS = new DataSet();

            List<NoteDetail> NoteList = new List<NoteDetail>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(noteDS, "tbl_notes");

                int count = 0;
                int i = 0;
                count = noteDS.Tables["tbl_notes"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        NoteDetail nd = new NoteDetail();
                        nd.Title = noteDS.Tables["tbl_notes"].Rows[i]["note_title"].ToString();
                        nd.Content = noteDS.Tables["tbl_notes"].Rows[i]["note_content"].ToString();
                        nd.Owner = Convert.ToInt32(noteDS.Tables["tbl_notes"].Rows[i]["note_owner"]);

                        i++;
                        NoteList.Add(nd);

                    }
                    errorMessage = "";
                    return NoteList;
                }
                else
                {
                    errorMessage = "Cant fetch note";
                    return null;
                }

            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<NoteColaborators> SelectColaborations (out string errorMessage)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlquery = "SELECT n.note_title, u.user_name, n.note_id FROM Tbl_note n INNER JOIN Tbl_colaboration c ON n.note_id = c.note_id INNER JOIN Tbl_user u ON c.user_id = u.user_id WHERE c.user_id=n.note_owner";
            SqlCommand dbCommand = new SqlCommand(sqlquery, dbConnection);

            SqlDataReader reader = null;

            List<NoteColaborators> ColabList = new List<NoteColaborators>();

            errorMessage = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    NoteColaborators colaboration = new NoteColaborators();
                    colaboration.NoteID = Convert.ToInt16(reader["note_id"]);
                    colaboration.Title = reader["note_title"].ToString();
                    colaboration.User = reader["user_name"].ToString();

                    ColabList.Add(colaboration);
                }
                reader.Close();
                return ColabList;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<OwnerDetail> GetOwner (out string errorMessage)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlquery = "SELECT u.user_name, n.note_title, n.note_owner FROM Tbl_note n INNER JOIN Tbl_user u ON n.note_owner = u.user_id WHERE u.user_name='Anna'";
            SqlCommand dbCommand = new SqlCommand(sqlquery, dbConnection);

            SqlDataReader reader = null;

            List<OwnerDetail> ownerList = new List<OwnerDetail>();

            errorMessage = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    OwnerDetail od = new OwnerDetail();
                    od.Title = reader["note_title"].ToString();
                    od.User = reader["user_name"].ToString();
                    od.Owner = Convert.ToInt16(reader["note_owner"]);

                    ownerList.Add(od);
                }
                reader.Close();
                return ownerList;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<OwnerDetail> GetUser(out string errorMessage)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlquery = "SELECT * FROM Tbl_user";
            SqlCommand dbCommand = new SqlCommand(sqlquery, dbConnection);

            SqlDataReader reader = null;

            List<OwnerDetail> ownerList = new List<OwnerDetail>();

            errorMessage = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    OwnerDetail od = new OwnerDetail();
                    od.User = reader["user_name"].ToString();
                    od.Owner = Convert.ToInt16(reader["user_id"]);

                    ownerList.Add(od);
                }
                reader.Close();
                return ownerList;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int UpdateNote (NoteDetail nd, out string errorMessage, int id)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // SQL query to insert a note into the table
            String sqlquery = "UPDATE Tbl_note(note_title, note_content) VALUES (@Title, @Content) WHERE note_id = @id";

            // Creating a command object with the query and connection
            SqlCommand dbCommand = new SqlCommand(sqlquery, dbConnection);

            // Adding parameters to the SQL query to prevent SQL injection
            dbCommand.Parameters.Add("Title", SqlDbType.NVarChar, 50).Value = nd.Title;
            dbCommand.Parameters.Add("Content", SqlDbType.NVarChar, 5000).Value = nd.Content;
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;

            try
            {
                // Opening the database connection
                dbConnection.Open();

                int i = 0;
                // Executing the SQL command and getting the number of affected rows
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errorMessage = ""; }
                else { errorMessage = "No note updated in the data base"; }

                return i;
            }
            catch (Exception ex)
            {
                // If an exception occurs, set the error message to the exception message
                errorMessage = ex.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int DeleteNote(NoteDetail nd, out string errorMessage, int id)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // SQL query to insert a note into the table
            String sqlquery = "DELETE FROM Tbl_note WHERE note_id = @id";

            // Creating a command object with the query and connection
            SqlCommand dbCommand = new SqlCommand(sqlquery, dbConnection);

            // Adding parameters to the SQL query to prevent SQL injection
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;

            try
            {
                // Opening the database connection
                dbConnection.Open();

                int i = 0;
                // Executing the SQL command and getting the number of affected rows
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errorMessage = ""; }
                else { errorMessage = "No note deleted from the data base"; }

                return i;
            }
            catch (Exception ex)
            {
                // If an exception occurs, set the error message to the exception message
                errorMessage = ex.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<NoteDetail> GetSearchNote(out string errorMessage, string searchStr)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
            String sqlquery;

            if (searchStr == null)
            {
                sqlquery = "SELECT * FROM Tbl_note";
            }
            else
            {
                sqlquery = "SELECT * FROM Tbl_note WHERE note_title " +
                    "LIKE @searchStr OR note_content LIKE @searchStr";
            }

            // Creating a command object with the query and connection
            SqlCommand dbCommand = new SqlCommand(sqlquery, dbConnection);
            searchStr = "%" + searchStr + "%";
            dbCommand.Parameters.Add("searchStr", SqlDbType.NVarChar, 50).Value = searchStr;

            SqlDataAdapter myAdapter = new(dbCommand);
            DataSet noteDS = new DataSet();

            List<NoteDetail> NoteList = new List<NoteDetail>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(noteDS, "tbl_notes");

                int count = 0;
                int i = 0;
                count = noteDS.Tables["tbl_notes"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        NoteDetail nd = new NoteDetail();
                        nd.Title = noteDS.Tables["tbl_notes"].Rows[i]["note_title"].ToString();
                        nd.Content = noteDS.Tables["tbl_notes"].Rows[i]["note_content"].ToString();
                        nd.Owner = Convert.ToInt32(noteDS.Tables["tbl_notes"].Rows[i]["note_owner"]);

                        i++;
                        NoteList.Add(nd);

                    }
                    errorMessage = "";
                    return NoteList;
                }
                else
                {
                    errorMessage = "Cant fetch note";
                    return null;
                }

            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

    }

}
