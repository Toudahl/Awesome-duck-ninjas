namespace WebApi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SensorType")]
    public partial class SensorType
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Type { get; set; }
    }
}
