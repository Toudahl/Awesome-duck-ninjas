namespace WebApi.EF
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

        public int? Fk_Location { get; set; }

        public int? Fk_SensorType { get; set; }

        public int? Fk_Broadcaster { get; set; }

        public virtual Broadcaster Broadcaster { get; set; }

        public virtual Location Location { get; set; }

        public virtual SensorType SensorType { get; set; }
    }
}
