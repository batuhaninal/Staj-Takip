using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using StajTakip.Entities.Concrete;

namespace StajTakip.DataAccess.Concrete.Contexts
{
    public class StajTakipContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString: @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StajTakipTempDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
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
    }
}
