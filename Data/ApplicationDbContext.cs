using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TechWaveOnlineShopping.Models;
using TechWaveOnlineShopping.Models;

namespace TechWaveOnlineShopping.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
