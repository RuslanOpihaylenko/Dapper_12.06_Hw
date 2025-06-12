using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper_12._06_W.Models;

namespace Dapper_12._06_W.Repositories
{
    public class CountryRepository
    {
        private readonly string? _connectionString;
        public CountryRepository(string? connectionString) => _connectionString = connectionString;

        public void Register(string countryname)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM [Country] WHERE Name=@Name";
                var country = conn.QueryFirstOrDefault<Country>(query, new { countryname });
                if (country != null)
                {
                    throw new Exception("Country already exists");
                }
                query = "INSERT INTO [Country] (Name) VALUES (@Name)";
                conn.Execute(query, new { Name = countryname });
            }
        }
    }
}
