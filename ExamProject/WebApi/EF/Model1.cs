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

        public virtual DbSet<Broadcaster> Broadcasters { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Sensor> Sensors { get; set; }
        public virtual DbSet<SensorType> SensorTypes { get; set; }
        public virtual DbSet<Value> Values { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Broadcaster>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<SensorType>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<Value>()
                .Property(e => e.Value1)
                .IsUnicode(false);
        }
    }
}
