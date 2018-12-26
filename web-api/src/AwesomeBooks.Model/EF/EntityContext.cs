using AwesomeBooks.Model.DomainEntities.Core;
using AwesomeBooks.Model.DomainEntities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AwesomeBooks.Model.EF
{
    public class EntityContext : IdentityDbContext<UserEntity, RoleEntity, int, UserClaimEntity, UserRoleEntity, UserLoginEntity, RoleClaimEntity, UserTokenEntity>
    {
        public EntityContext() : base()
        {
        }

        public EntityContext(DbContextOptions<EntityContext> options) : base(options)
        {
        }

        public DbSet<SettingEntity> Settings { get; set; }

        public DbSet<CategoryGroupEntity> CategoryGroups{ get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<BookEntity> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Identity Entities
            builder.Entity<UserEntity>().ToTable("User");
            builder.Entity<RoleEntity>().ToTable("Role");
            builder.Entity<UserClaimEntity>().ToTable("UserClaim");
            builder.Entity<UserRoleEntity>().ToTable("UserRole");
            builder.Entity<UserLoginEntity>().ToTable("UserLogin");
            builder.Entity<RoleClaimEntity>().ToTable("RoleClaim");
            builder.Entity<UserTokenEntity>().ToTable("UserToken");

            builder.Entity<UserEntity>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserEntity>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserEntity>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Core Entities
            builder.Entity<SettingEntity>()
                .ToTable("Setting");

            var bookTable = builder.Entity<BookEntity>().ToTable("Book");
            builder.Entity<BookEntity>()
                .HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .IsRequired();

            bookTable.HasIndex(b => b.Name);
            bookTable.HasIndex(b => b.PublishYear);
            bookTable.HasIndex(b => b.Authors);

            builder.Entity<CategoryGroupEntity>().ToTable("CategoryGroup").HasIndex(ca => ca.Name);
            builder.Entity<CategoryGroupEntity>()
                .HasMany(e => e.Categories)
                .WithOne()
                .HasForeignKey(c => c.CategoryGroupId)
                .IsRequired();

            builder.Entity<CategoryEntity>()
                .ToTable("Category").HasIndex(c => c.Name);
            builder.Entity<CategoryEntity>()
                .HasOne(e => e.CategoryGroup)
                .WithMany(e => e.Categories)
                .HasForeignKey(e => e.CategoryGroupId)
                .IsRequired();
        }
    }
}
