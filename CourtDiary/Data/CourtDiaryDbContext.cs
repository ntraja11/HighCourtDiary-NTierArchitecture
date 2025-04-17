using CourtDiary.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourtDiary.Data;

public class CourtDiaryDbContext : IdentityDbContext<ApplicationUser>
{
    public CourtDiaryDbContext(DbContextOptions<CourtDiaryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Case> Cases { get; set; }

    public DbSet<Hearing> Hearings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
