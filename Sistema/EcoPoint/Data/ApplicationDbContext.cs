using Microsoft.EntityFrameworkCore;
using EcoPoint.Models;

namespace EcoPoint.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Reciclagem> Reciclagens { get; set; }

        public DbSet<Ecoponto> Ecopontos { get; set; }

        public DbSet<TipoMaterial> TiposMaterial { get; set; }

        public DbSet<Recompensa> Recompensas { get; set; }
        public DbSet<Resgate> Resgates { get; set; }
    }
}