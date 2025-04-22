using Microsoft.EntityFrameworkCore;

namespace MyntraWeb.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) 
        {
            
        }


    }
}
