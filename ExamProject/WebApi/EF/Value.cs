namespace WebApi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Value
    {
        public int Id { get; set; }

        [Column("Value")]
        [Required]
        [StringLength(100)]
        public string ValueInput { get; set; }

        public int FK_Sensor { get; set; }

        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        {
            get;
            set;
        } = DateTime.Now;
    }
}
