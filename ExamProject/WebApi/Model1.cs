namespace WebApi
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<data_values> data_values { get; set; }
        public virtual DbSet<sensor> sensors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<data_values>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<sensor>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<sensor>()
                .Property(e => e.location)
                .IsUnicode(false);

            modelBuilder.Entity<sensor>()
                .Property(e => e.platform)
                .IsUnicode(false);

            modelBuilder.Entity<sensor>()
                .HasMany(e => e.data_values)
                .WithRequired(e => e.sensor)
                .HasForeignKey(e => e.sensor_id)
                .WillCascadeOnDelete(false);
        }
    }
}
