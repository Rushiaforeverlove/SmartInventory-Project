using Microsoft.Data.Sqlite;
using SmartInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Data
{
    public static class Dbhelper
    {
        private static string connStr = "Data Source=inventory.db";
        public static void InitDb()
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string sql = """
                        CREATE TABLE IF NOT EXISTS Products (
                           Id INTEGER PRIMARY KEY AUTOINCREMENT,
                           Name TEXT,
                           Category TEXT,
                           Quantity INTEGER,
                           Price REAL
                           );    
                    """;
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

            }
        }

        public static void InsertProduct(Product p)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string sql = """
                    INSERT INTO Products (Name, Category, Quantity, Price) VALUES 
                    (@Name, @Category, @Quantity, @Price);
                    """;

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", p.Name);
                    cmd.Parameters.AddWithValue("@Category", p.Category);
                    cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                    cmd.Parameters.AddWithValue("@Price", (double)p.Price);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Product> GetAllProducts()
        {
            var result = new List<Product>();
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                using var cmd = new SqliteCommand("SELECT * FROM Products ORDER BY Id", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) // 一列一列讀
                    result.Add(new Product
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString() ?? "",
                        Category = reader["Category"].ToString() ?? "",
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Price = Convert.ToDecimal(reader["Price"])
                    });
                return result;
            }
        }
    }
}
