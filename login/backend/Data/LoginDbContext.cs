using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class LoginDbContext: DbContext
    {
        public LoginDbContext(DbContextOptions db):base(db)
        {
            
        }
        public DbSet<Usuarios> Usuarios { get; set; }
    }
}
