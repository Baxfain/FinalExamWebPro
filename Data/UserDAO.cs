using System;
using System.Data;
using Microsoft.Data.SqlClient;
using EastOutHotel.Models;

namespace EastOutHotel.Data
{
    public class UserDAO
    {
        private readonly string _connectionString;

        public UserDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        // REGISTER
        public bool RegisterUser(User user, string password)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string accountID = GenerateAccountID(conn, transaction);
                        string customerID = GenerateCustomerID(conn, transaction);

                        // Insert account
                        string sqlAccount = "INSERT INTO account (ID, Username, Password, Type) VALUES (@ID, @Username, @Password, @Type)";
                        using (var cmd = new SqlCommand(sqlAccount, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ID", accountID);
                            cmd.Parameters.AddWithValue("@Username", user.Username);
                            cmd.Parameters.AddWithValue("@Password", password);
                            cmd.Parameters.AddWithValue("@Type", "Customer");
                            if (cmd.ExecuteNonQuery() == 0) throw new Exception("Failed to insert account");
                        }

                        // Insert customer
                        string sqlCustomer = @"INSERT INTO customers 
                            (ID, Name, Gender, Birth_Date, Phone_Number, Account_ID, Membership_ID)
                            VALUES (@ID, @Name, @Gender, @BirthDate, @Phone, @AccountID, @MembershipID)";
                        using (var cmd = new SqlCommand(sqlCustomer, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ID", customerID);
                            cmd.Parameters.AddWithValue("@Name", user.FullName);
                            cmd.Parameters.AddWithValue("@Gender", user.Gender);
                            cmd.Parameters.AddWithValue("@BirthDate", user.BirthDate);
                            cmd.Parameters.AddWithValue("@Phone", user.PhoneNumber);
                            cmd.Parameters.AddWithValue("@AccountID", accountID);
                            cmd.Parameters.AddWithValue("@MembershipID", "M1");
                            if (cmd.ExecuteNonQuery() == 0) throw new Exception("Failed to insert customer");
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        // LOGIN
        public User? LoginUser(string username, string password)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT a.ID AS AccountID, c.ID AS CustomerID, a.Username, 
                               c.Name AS FullName, a.Type AS AccountType, m.ID AS MembershipID
                               FROM account a
                               LEFT JOIN customers c ON a.ID = c.Account_ID
                               LEFT JOIN membership m ON c.Membership_ID = m.ID
                               WHERE a.Username = @Username AND a.Password = @Password";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                AccountID = reader["AccountID"]?.ToString() ?? string.Empty,
                                CustomerID = reader["CustomerID"]?.ToString(),
                                Username = reader["Username"]?.ToString() ?? string.Empty,
                                FullName = reader["FullName"]?.ToString() ?? string.Empty,
                                AccountType = reader["AccountType"]?.ToString() ?? "Customer",
                                MembershipID = reader["MembershipID"]?.ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Check username existence
        public bool IsUsernameExists(string username)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM account WHERE Username = @Username";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // Update user profile
        public bool UpdateUser(User user)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"UPDATE c
                               SET c.Name=@Name, c.Gender=@Gender, c.Birth_Date=@BirthDate, c.Phone_Number=@Phone
                               FROM customers c
                               INNER JOIN account a ON c.Account_ID = a.ID
                               WHERE a.Username=@Username";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", user.FullName);
                    cmd.Parameters.AddWithValue("@Gender", user.Gender);
                    cmd.Parameters.AddWithValue("@BirthDate", user.BirthDate);
                    cmd.Parameters.AddWithValue("@Phone", user.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Update password
        public bool UpdatePassword(string username, string newPassword)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE account SET Password=@Password WHERE Username=@Username";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Password", newPassword);
                    cmd.Parameters.AddWithValue("@Username", username);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ID generators
        private string GenerateAccountID(SqlConnection conn, SqlTransaction transaction)
        {
            string sql = "SELECT TOP 1 ID FROM account ORDER BY ID DESC";
            using (var cmd = new SqlCommand(sql, conn, transaction))
            {
                var lastID = cmd.ExecuteScalar()?.ToString();
                if (string.IsNullOrEmpty(lastID)) return "ACC000001";
                int num = int.Parse(lastID.Substring(3)) + 1;
                return $"ACC{num:D6}";
            }
        }

        private string GenerateCustomerID(SqlConnection conn, SqlTransaction transaction)
        {
            string sql = "SELECT TOP 1 ID FROM customers ORDER BY ID DESC";
            using (var cmd = new SqlCommand(sql, conn, transaction))
            {
                var lastID = cmd.ExecuteScalar()?.ToString();
                if (string.IsNullOrEmpty(lastID)) return "CUS0001";
                int num = int.Parse(lastID.Substring(3)) + 1;
                return $"CUS{num:D4}";
            }
        }
    }
}
