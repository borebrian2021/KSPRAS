
using KSPRAS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace BurnSociety.Application
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<AbstractSubmissionModel> AbstractSubmissionModel { get; set; }
        public DbSet<IPNResponses> IPNResponses { get; set; }
        public DbSet<Registrations> Registrations { get; set; }
        public DbSet<PaymentResponse> PaymentResponse { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
                : base(options)
        {

        }
    }

}

