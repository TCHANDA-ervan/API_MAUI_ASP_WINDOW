using ApiRest.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiRest.Context
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Eleve> Eleves { get; set; }
        public DbSet<Adminitrateur> Adminitrateurs { get; set; }
        public DbSet<Coordinateur> Coordinateurs { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Groupe> Groupes { get; set; } 
        public DbSet<Presence> Presences { get; set; }

        public DbSet<Journee> Journees { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Eleve>().ToTable("eleves");
            // Configuration du mapping de l'entité Adminitrateur à la table "adminitrateurs"
            builder.Entity<Adminitrateur>().ToTable("adminitrateurs");

            // Configuration du mapping de l'entité Coordinateur à la table "coordinateurs"
            builder.Entity<Coordinateur>().ToTable("coordinateurs");

            // Configuration du mapping de l'entité Promotion à la table "promotions"
            builder.Entity<Promotion>().ToTable("promotions");

            // Configuration du mapping de l'entité Groupe à la table "groupes"
            builder.Entity<Groupe>().ToTable("groupes");

            // Configuration du mapping de l'entité Presence à la table "presences"
            builder.Entity<Presence>().ToTable("presences");

            // Configuration du mapping de l'entité Journee à la table "journees"
            builder.Entity<Journee>().ToTable("journees");
        }

    }
}

