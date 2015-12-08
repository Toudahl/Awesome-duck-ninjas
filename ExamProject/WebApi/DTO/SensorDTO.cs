using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi
{
    public class SensorDTO
    {
        public int Id { get; set; }
        public string LocationName { get; set; } = "N/A";
        public string SensorType { get; set; } = "N/A";

        public string Broadcaster { get; set; } = "N/A";
        public IQueryable<Value> Values { get; set; } 
    }
}