using Microsoft.EntityFrameworkCore;
using ProjectOne.API.Models;

namespace ProjectOne.API.Repository
{
    public class TicketContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Persist Security Info=True;User ID=emma-calbank;Password=Sa4755$Sa;Database=TicketingDB;TrustServerCertificate=True;");
        }

        public DbSet<Models.Ticket> Tickets { get; set; }
    }
}
