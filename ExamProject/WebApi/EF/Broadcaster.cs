namespace WebApi.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Broadcaster")]
    public partial class Broadcaster
    {
        public Broadcaster()
        {
            Sensors = new HashSet<Sensor>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Sensor> Sensors { get; set; }
    }
}
