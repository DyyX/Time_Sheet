using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace PersonalTimeSheet.DBUtils
{
    public class DbService
    {
        #region Privats

        private List<Row> _rows = new List<Row>();

        private void FillRowsList()
        {
            if (0 != _rows.Count)
            {
                _rows.Clear();
            }

            using (
                var connection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var command = new SqlCommand("SELECT * FROM dbo.Tasks", connection))
                {
                    try
                    {
                        connection.Open();

                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                           _rows.Add(new Row()
                               {
                                   Id = (int)reader[0],
                                   Title = (string)reader[1],
                                   Description = (string)reader[2],
                                   SpentTime = TimeSpan.FromSeconds((Int64)reader[3])
                               });
                        }

                        reader.Close();
                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        #endregion

        #region Publics

        public List<Row> Rows
        {
            get
            {
                if (0 == _rows.Count)
                {
                    FillRowsList();
                }

                return _rows;
            }

            private set { }
        }

        public void AddTableRow(Row row)
        {
            using (
                var connection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var command = new SqlCommand("INSERT INTO dbo.Tasks (DbTitle, DbDescription, DbSpentTime) VALUES (@Title, @Description, @SpentTime)", connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@Title", row.Title);
                        command.Parameters.AddWithValue("@Description", row.Description);
                        command.Parameters.AddWithValue("@SpentTime", row.SpentTime.TotalSeconds);

                        connection.Open();

                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void UpdateTableRow(Row row)
        {
            using (
                var connection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var command = new SqlCommand("UPDATE dbo.Tasks SET DbTitle = @Title, DbDescription = @Description, DbSpentTime = @SpentTime WHERE DbId = @Id", connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@Id", row.Id);
                        command.Parameters.AddWithValue("@Title", row.Title);
                        command.Parameters.AddWithValue("@Description", row.Description);
                        command.Parameters.AddWithValue("@SpentTime", row.SpentTime.TotalSeconds);

                        connection.Open();

                        var reader = command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void RemoveTableRow(int rowId)
        {
            using (
                var connection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var command = new SqlCommand("DELETE FROM dbo.Tasks WHERE DbId = @Id", connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@Id", rowId);
                        connection.Open();

                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        #endregion
    }
}