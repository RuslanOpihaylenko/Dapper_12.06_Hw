using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper_12._06_W.Models;

namespace Dapper_12._06_W.Repositories
{
    public class UserRepository
    {
        private readonly string? _connectionString;
        public UserRepository(string? connectionString) => _connectionString = connectionString;

        public void Register(string username, int companyid)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM [User] WHERE Name=@Name, CompanyId=@CompanyId";
                var user = conn.QueryFirstOrDefault<User>(query, new { username, companyid });
                if (user != null)
                {
                    throw new Exception("User already exists");
                }
                query = "INSERT INTO [User] (Name, CompanyId) VALUES (@Name, @CompanyId)";
                conn.Execute(query, new { Name = username, CompanyId = companyid });
            }
        }

        //static void ShowAllUsersWCompanies()
        //{
        //    using (IDbConnection db = new SqlConnection(connectionString))
        //    {
        //        var users = db.Query<User>("SELECT * FROM Users");
        //        foreach (var user in users)
        //        {
        //            var company = db.QueryFirstOrDefault<String>($"select Name\r\nfrom Company\r\nwhere Id = {user.CompanyId}");
        //            Console.WriteLine($"{user.Id} {user.Name} {company}");
        //        }
        //    }
        //}
    }
}
