using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper_12._06_W.Models;
using Z.Dapper.Plus;

namespace Dapper_12._06_W.Repositories
{
    public class CountryRepository
    {
        private readonly string? _connectionString;

        static string? connectionString;
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

        static void BulkInsertingCountries()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                DapperPlusManager.Entity<Country>().Table("Country").Identity(x => x.Id);

                var countries = new List<Country> { new Country { Name = "Germany"} ,
                new Country { Name = "USA"} };
                db.BulkInsert(countries);
            }
        }

        static void BulkDeleteCountries()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var countries = db.Query<Country>("select * from Country");
                db.BulkDelete(countries.Where(x => x.Name == "Germany" || x.Name == "USA"));
            }
        }

        static void ShowAllCountries()
        {

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var countries = db.Query<Country>("SELECT * FROM Country");
                foreach (var country in countries)
                {
                    Console.WriteLine($"{country.Name} ");
                }
            }
        }

        static void BulkUpdateCountries()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var countries = db.Query<Country>("select * from Country").ToList();
                countries[0].Name = "Germany";
                countries[1].Name = "USA";


                db.BulkUpdate(countries);
            }
        }
    }
}
