using Microsoft.EntityFrameworkCore;
using NZWalks.Models.Domain;

namespace NZWalks.Data;

public class NZWalksDbContext : DbContext
{
    public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }

    public DbSet<Difficulty> Difficulties { get; set; }

    public DbSet<Region> Regions { get; set; }

    public DbSet<Walk> Walks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data for Difficulties
        // Easy, Medium, Hard

        var difficulties = new List<Difficulty>() {
            new  () {
                Id = Guid.Parse("f326fe8c-e30f-4823-9dd0-33bc5ea07a5d"),
                Name = "Easy"
            },
            new () {
                Id = Guid.Parse("3ffd007e-5ad3-4e89-89c3-77aecefa3b0f"),
                Name = "Medium"
            },
            new () {
                Id = Guid.Parse("fcd3cf5f-92c9-452d-9144-b6d435a6f1db"),
                Name = "Hard"
            },
        };

        // Seed Difficulties to the Database
        modelBuilder.Entity<Difficulty>().HasData(difficulties);

        // Seed data for Regions

        var regions = new List<Region>() {
            new () {
                Id = Guid.Parse("8c0a4803-b027-4056-893e-fbcb3a6d690e"),
                Code = "LO",
                Name = "Land of Oo",
                RegionImageUrl = "https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/0374f040330989.639c9cbb8e0ec.jpg"
             },
            new () {
                Id = Guid.Parse("8f337dd0-d03c-4214-a610-54bf480f68c6"),
                Code = "KN",
                Name = "Knowhere",
                RegionImageUrl = "https://marvel.fandom.com/wiki/Knowhere?file=Knowhere_from_Infinity_Countdown_Vol_1_4_001.jpg"
             },
            new () {
                Id = Guid.Parse("e68c190e-efed-456b-b27e-b4fa43362564"),
                Code = "Asgard",
                Name = "AG",
                RegionImageUrl = "https://en.wikipedia.org/wiki/Asgard#/media/File:Asgard_and_Bifrost_in_interpretation_of_Otto_Schenk_in_Wagner's_Das_Rheingold.jpg"
             },
        };

        modelBuilder.Entity<Region>().HasData(regions);

    }
}