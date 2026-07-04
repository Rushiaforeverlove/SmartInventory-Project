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
                string sql = "SELECT * FROM Products";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) // 一列一列
                        {
                            var p = new Product();

                            p.Id = Convert.ToInt32(reader["Id"]);
                            p.Name = reader["Name"].ToString()!;
                            p.Category = reader["Category"].ToString()!;
                            p.Quantity = Convert.ToInt32(reader["Quantity"]);
                            p.Price = Convert.ToDecimal(reader["Price"]);

                            result.Add(p);
                        }
                    }

                }

            }
            return result;
        }
        public static void DeleteItem(Product p)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string sql = "delete from item where id=@id";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id", item.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // 刪除全部
        public static void DeleteAllItem()
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string sql = "delete from item";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // 修改      
        public static void UpdateItem(Product p)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string sql = """
                    update item set
                    date=@date,
                    note=@note,
                    amount=@amount,
                    category=@category,
                    isincome=@isincome
                    where id=@id
                    """;

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@date", item.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                    //cmd.Parameters.AddWithValue("@note", item.Note);
                    //cmd.Parameters.AddWithValue("@amount", (double)item.Amount);
                    //cmd.Parameters.AddWithValue("@category", item.CategoryType.ToString());
                    //cmd.Parameters.AddWithValue("@isincome", item.IsIncome);
                    //cmd.Parameters.AddWithValue("@id", item.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteProducts(Product p)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string sql = "delete from Products where id=@id";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", p.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateProducts(Product p)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
               
                string sql = """
                    update Products set
                    Name=@Name,
                    Category=@Category,
                    Quantity=@Quantity,
                    Price=@Price
                    where Id=@Id
                    """;

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", p.Name);
                    cmd.Parameters.AddWithValue("@Category", p.Category);
                    cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                    cmd.Parameters.AddWithValue("@Price", (double)p.Price);
                    cmd.Parameters.AddWithValue("@Id", (double)p.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}