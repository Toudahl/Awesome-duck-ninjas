namespace WebApi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sensor")]
    public partial class Sensor
    {
        public int Id { get; set; }

        public int Fk_Location { get; set; }

        public int Fk_SensorType { get; set; }

        public int Fk_Broadcaster { get; set; }
    }
}
