using Microsoft.EntityFrameworkCore;
using Profiler_Api.DbModels;

namespace Profiler_Api.Data;

public class SchedulerContext : DbContext
{
    public SchedulerContext(DbContextOptions<SchedulerContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID);
            entity.Property(e => e.Username);
            entity.Property(e => e.PasswordHash);
            entity.Property(e => e.PasswordSalt);
            entity.Property(e => e.Email);
            entity.Property(e => e.Role);
            entity.Property(e => e.Date);

            entity.HasData(new User
            {
                ID = 1,
                Username = "admin",
                PasswordHash = "1D57743E41C84192A747A6A2F692FC97B5A2C3D1DB7CDCA328463B85FFB7DBF2457C419486640E458D0EC1664AA2E80EA0E7289F8EF79DBEE9DF1BCCDF2388A7",
                PasswordSalt = "4C501A810366409E230AECB8A57D1DB01F834DDDA53416B32D1E7C9EDB9A7DB8B07932EFA3FD773552BCFE097426E713E1D3D525E5B632D3447C0A91EC4860D6",
                Email = "admin@localhost.com",
                Role = "A"
            });
        });
    }

    public DbSet<User> Users { get; set; }
}
