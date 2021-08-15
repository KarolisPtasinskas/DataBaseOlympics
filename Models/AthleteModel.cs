using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseOlympics.Models
{
    public class AthleteModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Country { get; set; }
        public string CountryName { get; set; }
        public int Sport { get; set; }
        public List<int> Sports { get; set; }
        public List<string> SportsNames { get; set; }
    }
}
