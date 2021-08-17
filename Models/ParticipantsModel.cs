using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseOlympics.Models
{
    public class ParticipantsModel
    {
        public ParticipantsModel()
        {
            OrderByList = new List<string>()
            {
                "FirstName", "LastName", "Country"
            };

            FilterByList = new List<string>()
            {
                "", ""
            };
        }

        public AthleteModel Athlete { get; set; }
        public List<AthleteModel> AllAthletes { get; set; }
        public CountryModel Country { get; set; }
        public List<CountryModel> AllCountries { get; set; }
        public SportModel Sport { get; set; }
        public List<SportModel> AllSports { get; set; }
        public string OrderBy { get; set; }
        public List<string> OrderByList { get; set; }
        public List<string> FilterByList { get; set; }
    }


}
