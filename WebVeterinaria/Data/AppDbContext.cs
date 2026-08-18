using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Models;

namespace WebVeterinaria.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Atencion> Atencions { get; set; }

    public virtual DbSet<Citum> Cita { get; set; }

    public virtual DbSet<Dueno> Duenos { get; set; }

    public virtual DbSet<Especialidad> Especialidads { get; set; }

    public virtual DbSet<Especie> Especies { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<MascotaVacuna> MascotaVacunas { get; set; }

    public virtual DbSet<Mascotum> Mascota { get; set; }

    public virtual DbSet<Raza> Razas { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vacuna> Vacunas { get; set; }

    public virtual DbSet<Veterinario> Veterinarios { get; set; }

    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-0O0VPIV\\SQLEXPRESS;Database=bd_Veterinaria;Trusted_Connection=True;TrustServerCertificate=True;");
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Atencion>(entity =>
        {
            entity.HasKey(e => e.IdAtencion).HasName("PK__atencion__D0A4023676AF566C");

            entity.ToTable("atencion");

            entity.HasIndex(e => e.IdCita, "UQ__atencion__6AEC3C083CC3D712").IsUnique();

            entity.Property(e => e.IdAtencion).HasColumnName("id_atencion");
            entity.Property(e => e.Diagnostico).HasColumnName("diagnostico");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdCita).HasColumnName("id_cita");
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");
            entity.Property(e => e.Peso)
                .HasColumnType("numeric(5, 2)")
                .HasColumnName("peso");
            entity.Property(e => e.Temperatura)
                .HasColumnType("numeric(4, 1)")
                .HasColumnName("temperatura");
            entity.Property(e => e.Tratamiento).HasColumnName("tratamiento");

            entity.HasOne(d => d.IdCitaNavigation).WithOne(p => p.Atencion)
                .HasForeignKey<Atencion>(d => d.IdCita)
                .HasConstraintName("fk_atencion_cita");
        });

        modelBuilder.Entity<Citum>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("PK__cita__6AEC3C09D05F1A54");

            entity.ToTable("cita");

            entity.Property(e => e.IdCita).HasColumnName("id_cita");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("PENDIENTE")
                .HasColumnName("estado");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Hora).HasColumnName("hora");
            entity.Property(e => e.IdMascota).HasColumnName("id_mascota");
            entity.Property(e => e.IdVeterinario).HasColumnName("id_veterinario");
            entity.Property(e => e.Motivo).HasColumnName("motivo");

            entity.HasOne(d => d.IdMascotaNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdMascota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cita_mascota");

            entity.HasOne(d => d.IdVeterinarioNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdVeterinario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cita_veterinario");
        });

        modelBuilder.Entity<Dueno>(entity =>
        {
            entity.HasKey(e => e.IdDueno).HasName("PK__dueno__3C76252702037A9D");

            entity.ToTable("dueno");

            entity.HasIndex(e => e.Dni, "UQ__dueno__D87608A745CB0C61").IsUnique();

            entity.Property(e => e.IdDueno).HasColumnName("id_dueno");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .HasColumnName("apellidos");
            entity.Property(e => e.Correo)
                .HasMaxLength(120)
                .HasColumnName("correo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .HasColumnName("direccion");
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .HasColumnName("dni");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .HasColumnName("nombres");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Especialidad>(entity =>
        {
            entity.HasKey(e => e.IdEspecialidad).HasName("PK__especial__C1D13763A1BC7C3E");

            entity.ToTable("especialidad");

            entity.Property(e => e.IdEspecialidad).HasColumnName("id_especialidad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Especie>(entity =>
        {
            entity.HasKey(e => e.IdEspecie).HasName("PK__especie__96DDB0B9F24290A9");

            entity.ToTable("especie");

            entity.HasIndex(e => e.Nombre, "UQ__especie__72AFBCC6ACC4E82D").IsUnique();

            entity.Property(e => e.IdEspecie).HasColumnName("id_especie");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__horario__C5836D69A861CB44");

            entity.ToTable("horario");

            entity.Property(e => e.IdHorario).HasColumnName("id_horario");
            entity.Property(e => e.DiaSemana).HasColumnName("dia_semana");
            entity.Property(e => e.HoraFin).HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.IdVeterinario).HasColumnName("id_veterinario");

            entity.HasOne(d => d.IdVeterinarioNavigation).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.IdVeterinario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_horario_veterinario");
        });

        modelBuilder.Entity<MascotaVacuna>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mascota___3213E83F22DA76C2");

            entity.ToTable("mascota_vacuna");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdMascota).HasColumnName("id_mascota");
            entity.Property(e => e.IdVacuna).HasColumnName("id_vacuna");
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");
            entity.Property(e => e.ProximaDosis).HasColumnName("proxima_dosis");

            entity.HasOne(d => d.IdMascotaNavigation).WithMany(p => p.MascotaVacunas)
                .HasForeignKey(d => d.IdMascota)
                .HasConstraintName("fk_mv_mascota");

            entity.HasOne(d => d.IdVacunaNavigation).WithMany(p => p.MascotaVacunas)
                .HasForeignKey(d => d.IdVacuna)
                .HasConstraintName("fk_mv_vacuna");
        });

        modelBuilder.Entity<Mascotum>(entity =>
        {
            entity.HasKey(e => e.IdMascota).HasName("PK__mascota__6F0373525FBBD104");

            entity.ToTable("mascota");

            entity.Property(e => e.IdMascota).HasColumnName("id_mascota");
            entity.Property(e => e.Alergias).HasColumnName("alergias");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.IdDueno).HasColumnName("id_dueno");
            entity.Property(e => e.IdEspecie).HasColumnName("id_especie");
            entity.Property(e => e.IdRaza).HasColumnName("id_raza");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .HasColumnName("nombre");
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");
            entity.Property(e => e.Peso)
                .HasColumnType("numeric(5, 2)")
                .HasColumnName("peso");
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("sexo");

            entity.HasOne(d => d.IdDuenoNavigation).WithMany(p => p.Mascota)
                .HasForeignKey(d => d.IdDueno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_mascota_dueno");

            entity.HasOne(d => d.IdEspecieNavigation).WithMany(p => p.Mascota)
                .HasForeignKey(d => d.IdEspecie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_mascota_especie");

            entity.HasOne(d => d.IdRazaNavigation).WithMany(p => p.Mascota)
                .HasForeignKey(d => d.IdRaza)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_mascota_raza");
        });

        modelBuilder.Entity<Raza>(entity =>
        {
            entity.HasKey(e => e.IdRaza).HasName("PK__raza__084F250AF8596D9F");

            entity.ToTable("raza");

            entity.Property(e => e.IdRaza).HasColumnName("id_raza");
            entity.Property(e => e.IdEspecie).HasColumnName("id_especie");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdEspecieNavigation).WithMany(p => p.Razas)
                .HasForeignKey(d => d.IdEspecie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_raza_especie");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__rol__6ABCB5E0C78205B0");

            entity.ToTable("rol");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuario__4E3E04ADD01238A0");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Usuario1, "UQ__usuario__9AFF8FC67DDFA283").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .HasColumnName("apellidos");
            entity.Property(e => e.Clave)
                .HasMaxLength(255)
                .HasColumnName("clave");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .HasColumnName("nombres");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(50)
                .HasColumnName("usuario");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_rol");
        });

        modelBuilder.Entity<Vacuna>(entity =>
        {
            entity.HasKey(e => e.IdVacuna).HasName("PK__vacuna__BCC290CBA46D0C17");

            entity.ToTable("vacuna");

            entity.Property(e => e.IdVacuna).HasColumnName("id_vacuna");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Veterinario>(entity =>
        {
            entity.HasKey(e => e.IdVeterinario).HasName("PK__veterina__16E06DFFF08129EB");

            entity.ToTable("veterinario");

            entity.Property(e => e.IdVeterinario).HasColumnName("id_veterinario");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .HasColumnName("apellidos");
            entity.Property(e => e.Cmpv)
                .HasMaxLength(30)
                .HasColumnName("cmpv");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .HasColumnName("dni");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.IdEspecialidad).HasColumnName("id_especialidad");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .HasColumnName("nombres");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdEspecialidadNavigation).WithMany(p => p.Veterinarios)
                .HasForeignKey(d => d.IdEspecialidad)
                .HasConstraintName("fk_veterinario_especialidad");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
