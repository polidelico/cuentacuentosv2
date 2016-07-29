using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    public class CuentosContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Imageble> Imagebles { get; set; }
        public DbSet<PageType> PageTypes { get; set; }
        public DbSet<BuilderGallery> BuilderGalleries { get; set; }
        public DbSet<ImageCategory> ImageCategories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public CuentosContext() : base("CuentosConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Maps to the expected many-to-many join table name for roles to users.
            modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .Map(m =>
            {
                m.ToTable("RoleMemberships");
                m.MapLeftKey("UserName");
                m.MapRightKey("RoleName");
            });

            // Maps to the expected many-to-many join table name for interests to users.
            modelBuilder.Entity<User>()
            .HasMany(u => u.Interests)
            .WithMany(r => r.Users)
            .Map(m =>
            {
                m.ToTable("UserInterests");
                m.MapLeftKey("UserName");
                m.MapRightKey("InterestId");
            });

            // Maps to the expected many-to-many join table name for interests to users.
            modelBuilder.Entity<Story>()
            .HasMany(s => s.Interests)
            .WithMany(i => i.Stories)
            .Map(m =>
            {
                m.ToTable("StoryInterests");
                m.MapLeftKey("StoryId");
                m.MapRightKey("InterestId");
            });

            // Maps to the expected many-to-many join table name for Categories to Story.
            modelBuilder.Entity<Story>()
            .HasMany(s => s.Categories)
            .WithMany(c => c.Stories)
            .Map(m =>
            {
                m.ToTable("StoryCategories");
                m.MapLeftKey("StoryId");
                m.MapRightKey("CategoryId");
            });

            // Maps to the expected many-to-many join table name for Grades to Story.
            modelBuilder.Entity<Story>()
            .HasMany(s => s.Grades)
            .WithMany(c => c.Stories)
            .Map(m =>
            {
                m.ToTable("StoryGrades");
                m.MapLeftKey("StoryId");
                m.MapRightKey("GradeId");
            });
        }

    }
}