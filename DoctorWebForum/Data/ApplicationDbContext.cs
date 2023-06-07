using DoctorWebForum.Configurations;
using DoctorWebForum.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DoctorWebForum.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Qualification> Qualification { get; set; }
        public DbSet<Experience> Experience { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Feedback> Feedback { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {



            // Configuration relation table
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new PostConfiguration());
            builder.ApplyConfiguration(new FeedbackConfiguration());
            builder.ApplyConfiguration(new ExperienceConfiguration());
            builder.ApplyConfiguration(new QualificationConfiguration());

            
            //builder.Entity<ApplicationUser>()
            //   .HasOptional(x => x.Posts)
            //   .WithMany()
            //   .WillCascadeOnDelete(true);

            base.OnModelCreating(builder);

            //Data seed table Qualification
            builder.Entity<Qualification>().HasData(new Qualification { Id = 1, Name = " College" });
            builder.Entity<Qualification>().HasData(new Qualification { Id = 2, Name = " Bachelor" });
            builder.Entity<Qualification>().HasData(new Qualification { Id = 3, Name = " Master" });
            builder.Entity<Qualification>().HasData(new Qualification { Id = 4, Name = " Doctorate" });
            builder.Entity<Qualification>().HasData(new Qualification { Id = 5, Name = " Other" });

            //Data seed table Experience
            builder.Entity<Experience>().HasData(new Experience { Id = 1, Years = " Less 1 year" });
            builder.Entity<Experience>().HasData(new Experience { Id = 2, Years = " 1 to 2 years" });
            builder.Entity<Experience>().HasData(new Experience { Id = 3, Years = " 2 to 4 years" });
            builder.Entity<Experience>().HasData(new Experience { Id = 4, Years = " More 5 years" });
            builder.Entity<Experience>().HasData(new Experience { Id = 5, Years = " Other " });


            

        }
    }
}