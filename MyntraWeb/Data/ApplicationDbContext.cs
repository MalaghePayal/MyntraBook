using Microsoft.EntityFrameworkCore;
using MyntraWeb.Models;

namespace MyntraWeb.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) 
        {
            
        }
        public DbSet<Category>  Categories { get; set; }

    }
}
