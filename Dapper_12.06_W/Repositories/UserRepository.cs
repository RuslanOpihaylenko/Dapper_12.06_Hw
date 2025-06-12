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
    }
}
