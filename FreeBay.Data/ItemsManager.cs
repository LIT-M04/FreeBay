using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBay.Data
{
    public class ItemsManager
    {
        private string _connectionString;

        public ItemsManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Item> GetItems()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Items";
                List<Item> items = new List<Item>();
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Item item = new Item
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        Description = (string)reader["Description"]
                    };
                    items.Add(item);
                }

                return items.OrderByDescending(i => i.DateCreated);
            }
        }

        public void Delete(int itemId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Items WHERE Id = @id";
                command.Parameters.AddWithValue("@id", itemId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public int Add(Item item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Items (Name, PhoneNumber, Description, DateCreated) " +
                                      "VALUES (@name, @phone, @desc, @date); SELECT @@Identity";
                command.Parameters.AddWithValue("@name", item.Name);
                command.Parameters.AddWithValue("@phone", item.PhoneNumber);
                command.Parameters.AddWithValue("@desc", item.Description);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                connection.Open();
                return (int)(decimal)command.ExecuteScalar();
            }
        }
    }
}
