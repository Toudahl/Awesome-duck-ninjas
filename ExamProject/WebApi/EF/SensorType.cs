namespace WebApi.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SensorType")]
    public partial class SensorType
    {
        public SensorType()
        {
            Sensors = new HashSet<Sensor>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public virtual ICollection<Sensor> Sensors { get; set; }
    }
}
