using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Model
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Contracts> Contracts { get; set; }
        public DbSet<TermOfPayment> TermsOfPayment { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Acts> Acts { get; set; }
        public DbSet<ResponsiblePersons> ResponsiblePersons { get; set; }
    }
}
