using DataBaseOlympics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseOlympics.Services
{
    public class SportDBService
    {
        private readonly SqlConnection _connection;

        public SportDBService(SqlConnection connection)
        {
            _connection = connection;
        }

        //SELECT all sports from DB
        public List<SportModel> GetAllSports()
        {
            List<SportModel> sports = new();

            _connection.Open();
            var command = new SqlCommand("SELECT [id], [SportName], [TeamActivity] FROM [dbo].[Sports]", _connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                SportModel sport = new()
                {
                    Id = (int)reader.GetValue(0),
                    SportName = (string)reader.GetValue(1),
                    TeamActivity = (bool)reader.GetValue(2)
                };

                sports.Add(sport);
            }

            _connection.Close();
            return sports;
        }

        //INSERT sport to DB
        public void AddSport(SportModel sport)
        {
            _connection.Open();
            var command = new SqlCommand($"INSERT INTO dbo.Sports (SportName, TeamActivity) values ('{sport.SportName}', '{sport.TeamActivity}')", _connection);
            var reader = command.ExecuteReader();
            _connection.Close();
        }
    }
}
