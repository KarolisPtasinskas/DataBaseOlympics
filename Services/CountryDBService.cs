using DataBaseOlympics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseOlympics.Services
{
    public class CountryDBService
    {
        private readonly SqlConnection _connection;

        public CountryDBService(SqlConnection connection)
        {
            _connection = connection;
        }

        public List<CountryModel> GetAllCountries()
        {
            List<CountryModel> countries = new();

            _connection.Open();
            var command = new SqlCommand("SELECT [id], [CountryName], [UNDP] FROM [dbo].[Countries]", _connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                CountryModel country = new()
                {
                    Id = (int)reader.GetValue(0),
                    CountryName = (string)reader.GetValue(1),
                    UNDP = (string)reader.GetValue(2)
                };

                countries.Add(country);
            }

            _connection.Close();
            return countries;
        }

        public void AddCountry(CountryModel country)
        {
            _connection.Open();
            var command = new SqlCommand($"INSERT INTO dbo.Countries (CountryName, UNDP) values ('{country.CountryName}', '{country.UNDP}')", _connection);
            var reader = command.ExecuteReader();
            _connection.Close();
        }

    }
}
