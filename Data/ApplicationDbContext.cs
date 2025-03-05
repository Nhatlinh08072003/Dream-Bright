using Microsoft.EntityFrameworkCore;
namespace Dream_Bridge.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Thêm DbSet cho các entity của bạn
        // Ví dụ: public DbSet<User> Users { get; set; }
    }
}