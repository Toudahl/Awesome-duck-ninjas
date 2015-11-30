namespace WebApi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sensor")]
    public partial class sensor
    {
        public sensor()
        {
            data_values = new HashSet<data_values>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [StringLength(50)]
        public string location { get; set; }

        [StringLength(100)]
        public string platform { get; set; }

        public virtual ICollection<data_values> data_values { get; set; }
    }
}
