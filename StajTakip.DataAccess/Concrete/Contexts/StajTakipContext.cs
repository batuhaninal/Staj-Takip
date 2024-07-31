using Core.Entities.Concrete;
using Core.Utilities.Security.Hashing;
using Microsoft.EntityFrameworkCore;
using StajTakip.Entities.Concrete;

namespace StajTakip.DataAccess.Concrete.Contexts
{
    public class StajTakipContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString: @"Server=localhost,1433; Initial Catalog=sozlukappdb; TrustServerCertificate=True; User ID =sa; Password=Unr4vel!;");
        }

        public DbSet<InternshipsBook> InternshipsBooks { get; set; }
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<BookTemplate> BookTemplates { get; set; }
        public DbSet<InternshipDocument> InternshipDocuments { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<StudentUser> StudentUsers { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<AdminStudentRelation> AdminStudentRelations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            byte[] pswHash, pswSalt;
            HashingHelper.CreatePasswordHash("12345", out pswHash, out pswSalt);

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                CreatedByName = "Initial",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Email = "admin@admin.com",
                IsDeleted = false,
                ModifiedByName = "Initial",
                ModifiedDate = DateTime.Now,
                Note = "Initial",
                Username = "adminuser",
                PasswordHash = pswHash,
                PasswordSalt = pswSalt
            });

            modelBuilder.Entity<AdminUser>().HasData(new AdminUser
            {
                Id=1,
                FirstName = "Admin",
                LastName = "Admin",
                IsCompany = false,
                UserId = 1
            });

            modelBuilder.Entity<OperationClaim>().HasData(
            new OperationClaim
            {
                Id = 1,
                Name = "admin"
            },
            new OperationClaim
            {
                Id = 2,
                Name = "admin.teacher"
            },
            new OperationClaim
            {
                Id = 3,
                Name = "admin.company"
            },
            new OperationClaim
            {
                Id = 4,
                Name = "student"
            }
            );

            modelBuilder.Entity<UserOperationClaim>().HasData(new UserOperationClaim
            {
                Id = 1,
                OperationClaimId = 2,
                UserId = 1,
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
