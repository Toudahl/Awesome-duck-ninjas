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
        [StringLength(20)]
        public string Value1 { get; set; }

        public int? FK_Sensor { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
