using InnoClinic.Authorization.Core.Models.AccountModels;
using InnoClinic.Authorization.Core.Models.DoctorModels;
using InnoClinic.Authorization.Core.Models.ReceptionistModels;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.DataAccess.Context
{
    /// <summary>
    /// Represents the database context for InnoClinic authorization.
    /// </summary>
    public class InnoClinicAuthorizationDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the collection of accounts in the database.
        /// </summary>
        public DbSet<AccountEntity> Accounts { get; set; }

        /// <summary>
        /// Gets or sets the collection of doctors in the database.
        /// </summary>
        public DbSet<DoctorEntity> Doctors { get; set; }

        /// <summary>
        /// Gets or sets the collection of receptionists in the database.
        /// </summary>
        public DbSet<ReceptionistEntity> Receptionists { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InnoClinicAuthorizationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the database context.</param>
        public InnoClinicAuthorizationDbContext(DbContextOptions<InnoClinicAuthorizationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
