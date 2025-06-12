﻿using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Z.Dapper.Plus;
using Dapper_12._06_W.Models;
using Dapper_12._06_W.Repositories;
using Microsoft.Extensions.Configuration;

// Bulc CRUD https://dotnetfiddle.net/dbMVfr
namespace Dapper_12._06_W
{
    public class Program
    {
        static string? connectionString;
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder(); // Створення конфігуратора.
            string path = Directory.GetCurrentDirectory(); // Отримує поточну директорію програми.
            builder.SetBasePath(path); // Встановлює базовий шлях для конфігураційних файлів.
            builder.AddJsonFile("appsettings.json"); // Додає файл конфігурації.
            var config = builder.Build(); // Завантажує конфігурацію.
            if (config == null)
            {
                throw new Exception("Configuration not found"); // Викидає помилку, якщо конфігурація не знайдена.
            }
            string connectionString = config.GetConnectionString("DefaultConnection"); // Отримує рядок підключення.
            
            CountryRepository countryRepository = new CountryRepository(connectionString);
            CompanyRepository companyRepository = new CompanyRepository(connectionString);
            UserRepository userRepository = new UserRepository(connectionString);

            DapperPlusManager.Entity<Company>().Table("Company").Identity(x => x.Id);
            DapperPlusManager.Entity<Country>().Table("Country").Identity(x => x.Id);

            BulkInsertingCompanies();
            BulkUpdateCompanies();
            BulkDeleteCompanies();
            ShowAllCompanies();
            ShowAllUsersWCompanies();
            BulkInsertingCountries();
            BulkDeleteCountries();
            ShowAllCountries();
            BulkUpdateCountries();

        }

        static void ShowAllUsersWCompanies()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var users = db.Query<User>("SELECT * FROM Users");
                foreach (var user in users)
                {
                    var company = db.QueryFirstOrDefault<String>($"select Name\r\nfrom Company\r\nwhere Id = {user.CompanyId}");
                    Console.WriteLine($"{user.Id} {user.Name} {company}");
                }
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
