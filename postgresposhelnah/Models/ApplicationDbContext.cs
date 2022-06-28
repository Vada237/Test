using postgresposhelnah.Models;
using Microsoft.EntityFrameworkCore;

namespace postgresposhelnah.Models
{
    public class ApplicationDbContext : DbContext
    {
        DbSet<books> books { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
