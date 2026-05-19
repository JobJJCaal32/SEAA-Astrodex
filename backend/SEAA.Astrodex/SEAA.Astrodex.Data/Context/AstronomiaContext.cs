// Data/Context/AstronomiaContext.cs
using SEAA.Astrodex.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace SEAA.Astrodex.Data.Context
{
    public class AstronomiaContext : DbContext
    {
        public AstronomiaContext(DbContextOptions<AstronomiaContext> options)
            : base(options) { }

        public DbSet<CuerpoCeleste> CuerposCelestes { get; set; }
        public DbSet<LunaRef> LunasRef { get; set; }
        public DbSet<RelacionCeleste> RelacionesCelestes { get; set; }
        public DbSet<HistorialConsulta> HistorialConsultas { get; set; }
        public DbSet<PaginaCargada> PaginasCargadas { get; set; }
        public DbSet<PaginaCargadaCuerpo> PaginasCargadasCuerpos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CuerpoCeleste>()
                .HasOne(c => c.PlanetaPadre)
                .WithMany(c => c.Lunas)
                .HasForeignKey(c => c.PlanetaPadreId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RelacionCeleste>()
                .HasOne(r => r.CuerpoOrigen)
                .WithMany(c => c.RelacionesOrigen)
                .HasForeignKey(r => r.CuerpoOrigenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RelacionCeleste>()
                .HasOne(r => r.CuerpoDestino)
                .WithMany(c => c.RelacionesDestino)
                .HasForeignKey(r => r.CuerpoDestinoId)
                .OnDelete(DeleteBehavior.Restrict);
            // Llave primaria compuesta para tabla intermedia
            modelBuilder.Entity<PaginaCargadaCuerpo>()
                .HasKey(pcc => new { pcc.PaginaCargadaId, pcc.CuerpoCelesteId });

            // Relación PaginaCargadaCuerpo → PaginaCargada
            modelBuilder.Entity<PaginaCargadaCuerpo>()
                .HasOne(pcc => pcc.PaginaCargada)
                .WithMany(pc => pc.Cuerpos)
                .HasForeignKey(pcc => pcc.PaginaCargadaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación PaginaCargadaCuerpo → CuerpoCeleste
            modelBuilder.Entity<PaginaCargadaCuerpo>()
                .HasOne(pcc => pcc.CuerpoCeleste)
                .WithMany()
                .HasForeignKey(pcc => pcc.CuerpoCelesteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}