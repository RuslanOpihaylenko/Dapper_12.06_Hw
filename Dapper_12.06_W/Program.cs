﻿using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Z.Dapper.Plus;
using Dapper_12._06_W.Models;
using Dapper_12._06_W.Repositories;
using Microsoft.Extensions.Configuration;

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
            connectionString = config.GetConnectionString("DefaultConnection"); // Отримує рядок підключення.
            
            CountryRepository countryRepository = new CountryRepository(connectionString);
            CompanyRepository companyRepository = new CompanyRepository(connectionString);
            UserRepository userRepository = new UserRepository(connectionString);

            DapperPlusManager.Entity<Company>().Table("Company").Identity(x => x.Id);
            DapperPlusManager.Entity<Country>().Table("Country").Identity(x => x.Id);

           //BulkInsertingCompanies();
            //BulkUpdateCompanies();
            //BulkDeleteCompanies();
            //ShowAllCompanies();
            //ShowAllUsersWCompanies();
            //BulkInsertingCountries();
            //BulkDeleteCountries();
            //ShowAllCountries();
            //BulkUpdateCountries();

        }
    }
}
