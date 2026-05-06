using SEAA.Astrodex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Solo lo que las anotaciones no pueden definir
            // que es el comportamiento de eliminacion en cascada

            // Auto-referencia CuerpoCeleste
            modelBuilder.Entity<CuerpoCeleste>()
                .HasOne(c => c.PlanetaPadre)
                .WithMany(c => c.Lunas)
                .HasForeignKey(c => c.PlanetaPadreId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // RelacionCeleste origen
            modelBuilder.Entity<RelacionCeleste>()
                .HasOne(r => r.CuerpoOrigen)
                .WithMany(c => c.RelacionesOrigen)
                .HasForeignKey(r => r.CuerpoOrigenId)
                .OnDelete(DeleteBehavior.Restrict);

            // RelacionCeleste destino
            modelBuilder.Entity<RelacionCeleste>()
                .HasOne(r => r.CuerpoDestino)
                .WithMany(c => c.RelacionesDestino)
                .HasForeignKey(r => r.CuerpoDestinoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
