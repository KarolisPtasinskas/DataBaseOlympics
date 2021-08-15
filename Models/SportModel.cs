using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseOlympics.Models
{
    public class SportModel
    {
        public int Id { get; set; }
        public string SportName { get; set; }
        public bool TeamActivity { get; set; }
    }
}
