using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper_12._06_W.Models;
using Z.Dapper.Plus;

namespace Dapper_12._06_W.Repositories
{
    public class CompanyRepository
    {
        private readonly string? _connectionString;

        static string? connectionString;
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

        static void BulkInsertingCompanies()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                DapperPlusManager.Entity<Company>().Table("Company").Identity(x => x.Id);

                var companies = new List<Company> { new Company { Name = "Xerox", CountryId = 1 } ,
                new Company { Name = "Toyota", CountryId = 2} };
                db.BulkInsert(companies);
            }
        }

        static void BulkDeleteCompanies()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var companies = db.Query<Company>("select * from Company");
                db.BulkDelete(companies.Where(x => x.Name == "XXerox" || x.Name == "TToyota"));
            }
        }

        static void ShowAllCompanies()
        {

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var companies = db.Query<Company>("SELECT * FROM Company");
                foreach (var company in companies)
                {
                    var contrey = db.QueryFirstOrDefault<String>($"select Name\r\nfrom Country\r\nwhere Id = {company.CountryId}");
                    Console.WriteLine($"{company.Id} {company.Name} {contrey}");
                }
            }
        }

        static void BulkUpdateCompanies()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var companies = db.Query<Company>("select * from Company").ToList();
                companies[2].Name = "XXerox";
                companies[3].Name = "TToyota";


                db.BulkUpdate(companies);
            }
        }
    }
}
