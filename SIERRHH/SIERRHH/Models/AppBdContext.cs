using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace SIERRHH.Models
{
    public class AppBdContext : DbContext
    {
        public AppBdContext(DbContextOptions<AppBdContext> options) : base(options)
        {




        }

        //Importantes para conectar con la BD

        public DbSet<UsuarioEmpleado> UsuarioEmpleado { get; set; }

        public DbSet<PerfilProfesional> PerfilProfesional { get; set; }

        public DbSet<Aptitudes> Aptitudes { get; set; }

        public DbSet<Concurso> Concursos { get; set; }

        public DbSet<Certificacion> Certificacion { get; set; }

        public DbSet<Capacitacion> Capacitacion { get; set; }
        public DbSet<PuestosDesempenados> PuestosDesempenados { get; set; }

        public DbSet<Titulos> Titulos { get; set; }

        public DbSet<Sector> Sector { get; set; }

        public DbSet<Grado> Grado { get; set; }

        public DbSet<PuestosVacantes> PuestosVacantes { get; set; }

        public DbSet<GanadorConcurso> GanadorConcursos { get; set; }

        public DbSet<PerfilAptitudes> PerfilAptitudes { get; set; }

        public DbSet<PuestoAptitudes> PuestoAptitudes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UsuarioEmpleado>().HasData(new UsuarioEmpleado()
            //{
            //    IdEmpleado = 0,
            //    Correo = "LuisJose@gmail.com",
            //    Password = "12345",
            //    Rol = "Admin",
            //    Estado ="Activo"
            //});

            modelBuilder.Entity<Concurso>()
            .HasKey(e => new { e.IdEmpleado, e.IdPuesto });

            modelBuilder.Entity<GanadorConcurso>()
             .HasKey(e => new { e.IdPuesto, e.IdEmpleado });

            modelBuilder.Entity<PerfilAptitudes>()
            .HasKey(e => new { e.IdAptitudes, e.IdEmpleado });

            modelBuilder.Entity<PuestoAptitudes>()
             .HasKey(e => new { e.IdAptitudes, e.IdPuesto });



        }

        [DbFunction("dbo")]
        public IQueryable<Aptitudes> GetListaPuestoAptitudes(int idPuesto)
        {
            var idPuestoParam = new SqlParameter("@idPuesto", idPuesto);

            return Set<Aptitudes>().FromSqlRaw("EXEC Sp_Cns_ListaAptitudesPuestos @idPuesto", idPuestoParam);
        }


    }
}
