using Microsoft.EntityFrameworkCore;

namespace Dream_Bridge.web.Data
{
    public class DreamBridgeDbContext : DbContext
    {
        public DreamBridgeDbContext(DbContextOptions<DreamBridgeDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình mối quan hệ, bảng, chỉ mục nếu cần
        }
    }
}
