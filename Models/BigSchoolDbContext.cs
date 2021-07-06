using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BigSchool.Models
{
    public partial class BigSchoolDbContext : DbContext
    {
        public BigSchoolDbContext()
            : base("name=BigSchoolDbContext")
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Course> Course { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Course)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);
        }
    }
}
