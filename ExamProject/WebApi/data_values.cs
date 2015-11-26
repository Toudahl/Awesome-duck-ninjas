namespace WebApi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class data_values
    {
        public int id { get; set; }

        [StringLength(50)]
        public string value { get; set; }

        public DateTime? created_on { get; set; } = DateTime.Now;

        public int sensor_id { get; set; }

        public virtual sensor sensor { get; set; }
    }
}
