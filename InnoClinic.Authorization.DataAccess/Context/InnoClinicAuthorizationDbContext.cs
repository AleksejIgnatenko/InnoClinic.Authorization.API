using InnoClinic.Authorization.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.DataAccess.Context
{
    public class InnoClinicAuthorizationDbContext : DbContext
    {
        public DbSet<AccountModel> Accounts { get; set; }

        public InnoClinicAuthorizationDbContext(DbContextOptions<InnoClinicAuthorizationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
