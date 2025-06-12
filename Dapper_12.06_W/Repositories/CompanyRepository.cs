using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper_12._06_W.Models;

namespace Dapper_12._06_W.Repositories
{
    public class CompanyRepository
    {
        private readonly string? _connectionString;
        public CompanyRepository(string? connectionString) => _connectionString = connectionString;

        public void Register(string companyname, int countryid)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM [Company] WHERE Name=@Name, CountryId=@CountryId";
                var company = conn.QueryFirstOrDefault<Company>(query, new { companyname, countryid });
                if (company != null)
                {
                    throw new Exception("Company already exists");
                }
                query = "INSERT INTO [Company] (Name, CountryId) VALUES (@Name, @CountryId)";
                conn.Execute(query, new { Name = companyname, CountryId = countryid });
            }
        }
    }
}
