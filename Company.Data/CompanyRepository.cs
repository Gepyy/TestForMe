using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Company.Data
{
    public class CompanyRepository : ICompanyRepository
    {
        private static IConfigurationRoot configuration;
        private static List<CompanyItems> companies = new List<CompanyItems>();

        public static async Task Main(string[] args)
        {
            await InitConfig();
            //int answear = 0;
            //do
            //{
            //    Console.Clear();
            //    Console.WriteLine("DapperQuery_Moroz-------");
            //    Console.WriteLine("1. Create Company");
            //    Console.WriteLine("2. Read Company");
            //    Console.WriteLine("3. Update Company");
            //    Console.WriteLine("4. Delete Company");
            //    Console.WriteLine("0. Exit");
            //    Console.Write("Choose operation:");
            //    string strAnswear = Console.ReadLine();
            //    if (int.TryParse(strAnswear, out answear))
            //    {
            //        switch (answear)
            //        {
            //            case (0):
            //                break;
            //            case (1):
            //                await CreateCompany();
            //                ShowItemsOfCompanyList();
            //                Console.ReadKey();
            //                break;
            //            case (2):
            //                await ReadCompany();
            //                ShowItemsOfCompanyList();
            //                Console.ReadKey();
            //                break;
            //            case (3):
            //                await UpdateCompany();
            //                ShowItemsOfCompanyList();
            //                Console.ReadKey();
            //                break;
            //            case (4):
            //                await DeleteCompany();
            //                ShowItemsOfCompanyList();
            //                Console.ReadKey();
            //                break;
            //        }
            //    }
            //}
            //while (answear != 0);
            //Console.WriteLine("End...");
            //Console.ReadKey();

            void ShowItemsOfCompanyList()
            {
                Console.WriteLine("CompanyItems-------");
                companies.ForEach(item => Console.WriteLine($"Id:{item.Id}, nameOfCompany:{item.NameOfCompany}, Description: {item.Description}, Owner: {item.Owner}"));
                Console.WriteLine("-------------------");
            }
        }

        private static async Task InitConfig()
        {
            var builder = new ConfigurationBuilder();

            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();

            var a = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateCompany(int id, string nameOfCompany, string description, string owner)
        {
            //Console.WriteLine("Input ID:");
            //int.TryParse(Console.ReadLine(), out int id);
            //Console.WriteLine("Input name of company:");
            //string nameOfCompany = Console.ReadLine();
            //Console.WriteLine("Input description:");
            //string description = Console.ReadLine();
            //Console.WriteLine("Input Owner:");
            //string owner = Console.ReadLine();

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync("Moroz.Create_Company", new
                {
                    id,
                    nameOfCompany,
                    description,
                    owner
                }, commandType: CommandType.StoredProcedure);

                CompanyItems item = new CompanyItems(id, nameOfCompany, description, owner);
                if (!companies.Contains(item))
                {
                    companies.Add(item);
                }

                connection.Close();
            }

        }
        public List<CompanyItems> ReadCompany()
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("Moroz.GetAllItems_Company", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("Read Company-----------");
                        while (reader.Read())
                        {
                            int id = (int)reader["id"];
                            string nameOfCompany = (string)reader["nameOfCompany"];
                            string description = (string)reader["description"];
                            string owner = (string)reader["owner"];
                            Console.WriteLine($"ID: {id}, Name: {nameOfCompany}, Description: {description}, Owner: {owner}");

                            CompanyItems item = new CompanyItems(id, nameOfCompany, description, owner);
                            if (!companies.Contains(item))
                            {
                                companies.Add(item);
                            }
                        }
                        Console.WriteLine("----------------------");
                    }
                }
                connection.Close();
                return companies;
            }
        }
        public async Task UpdateCompany(int id, string nameOfCompany, string description, string owner)
        {
            //Console.WriteLine("Input which ID you want to Update:");
            //int.TryParse(Console.ReadLine(), out int id);
            //Console.WriteLine("Input new name of company:");
            //string nameOfCompany = Console.ReadLine();
            //Console.WriteLine("Input new description:");
            //string description = Console.ReadLine();
            //Console.WriteLine("Input new Owner:");
            //string owner = Console.ReadLine();

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                CompanyItems previosData = GetByIdCompany(id);
                await connection.ExecuteAsync("Moroz.UpdateById_Company", new
                {
                    id,
                    nameOfCompany,
                    description,
                    owner
                }, commandType: CommandType.StoredProcedure);

                //My Collection Company doesn't have ID which repeats

                CompanyItems item = new CompanyItems(id, nameOfCompany, description, owner);
                if (companies.Contains(previosData))
                {
                    int deleteIndex = companies.LastIndexOf(previosData);
                    companies[deleteIndex] = item;
                }
                connection.Close();
            }
        }

        public CompanyItems GetByIdCompany(int getId)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                CompanyItems result = connection.QuerySingleOrDefault<CompanyItems>("Moroz.GetById_Company", new { id = getId }, commandType: CommandType.StoredProcedure);
                connection.Close();
                if (result != null)
                {
                    Console.WriteLine($"Id:{result.Id}, nameOfCompany:{result.NameOfCompany}, Description: {result.Description}, Owner: {result.Owner}");
                    return result;
                }
                else
                {
                    throw new Exception("Incorrect Id!");
                }
            }
        }

        public async Task DeleteCompany(int id)
        {
            Console.WriteLine("Input which ID you want to Delete:");
            //int.TryParse(Console.ReadLine(), out int id);
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                CompanyItems previosData = GetByIdCompany(id);

                int deleteIndex = companies.LastIndexOf((previosData));

                await connection.ExecuteAsync("Moroz.DeleteById_Company", new { id });
                companies.RemoveAt(deleteIndex);
                connection.Close();
            }
        }
    }
}
