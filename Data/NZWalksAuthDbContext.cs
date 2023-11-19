using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.Data;

class NZWalksAuthDbContext : IdentityDbContext
{
    public NZWalksAuthDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var readerRoleId = "76af7101-7fa0-467d-aa32-0e19a33ab44e";
        var writerRoleId = "79364f83-3e6b-40af-9023-054abd19aca5";

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id =  readerRoleId,
                ConcurrencyStamp = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper(),
            },

            new IdentityRole
            {
                Id = writerRoleId,
                ConcurrencyStamp = readerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper(),
            },
        };

        builder.Entity<IdentityRole>().HasData(roles);
    }
}