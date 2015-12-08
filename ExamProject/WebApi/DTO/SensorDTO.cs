using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi
{
    public class SensorDTO
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string SensorType { get; set; }

        public string Broadcaster { get; set; }
        public IQueryable<Value> Values { get; set; } 
    }
}