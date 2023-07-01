using Microsoft.EntityFrameworkCore;

//Migration Commands: https://www.entityframeworktutorial.net/efcore/entity-framework-core-migration.aspx
namespace Pros.Database
{
    public class SchoolDatabaseContext: DbContext
    {
        public SchoolDatabaseContext()
        {

        }

        public SchoolDatabaseContext(DbContextOptions<SchoolDatabaseContext> options) : base(options)
        {
            //this.Database.EnsureDeleted();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Setting.ConnectionString, optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
