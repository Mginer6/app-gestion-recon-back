using System;
using System.Collections.Generic;
using AppGestionReconocimientos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AppGestionReconocimientos.Api.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrado> Administrados { get; set; }

    public virtual DbSet<CentroMedico> CentrosMedicos { get; set; }

    public virtual DbSet<ReconocimientoMedico> ReconocimientosMedicos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("perfil_administrado", new[] { "maquinista", "administrativo" })
            .HasPostgresEnum("resultado_reconocimiento", new[] { "apto", "no apto", "apto temporal" })
            .HasPostgresEnum("rol_usuario", new[] { "administrador", "medico", "administrativo", "generico_centro" });

        modelBuilder.Entity<Administrado>(entity =>
        {
            entity.HasKey(e => e.IdAdministrado).HasName("administrados_pkey");

            entity.ToTable("administrados");

            entity.HasIndex(e => e.DocIdentidad, "administrados_doc_identidad_key").IsUnique();

            entity.HasIndex(e => e.IdCentro, "idx_administrados_centro");

            entity.HasIndex(e => e.IdUsuario, "idx_administrados_usuario");

            entity.Property(e => e.IdAdministrado).HasColumnName("id_administrado");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(100)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(100)
                .HasColumnName("apellido2");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .HasColumnName("direccion");
            entity.Property(e => e.DocIdentidad)
                .HasMaxLength(20)
                .HasColumnName("doc_identidad");
            entity.Property(e => e.FechaAlta)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_alta");
            entity.Property(e => e.IdCentro).HasColumnName("id_centro");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.LicenciaMaquinista)
                .HasMaxLength(50)
                .HasColumnName("licencia_maquinista");
            entity.Property(e => e.Mail)
                .HasMaxLength(150)
                .HasColumnName("mail");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdCentroNavigation).WithMany(p => p.Administrados)
                .HasForeignKey(d => d.IdCentro)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_administrados_centro");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Administrados)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_administrados_usuario");
        });

        modelBuilder.Entity<CentroMedico>(entity =>
        {
            entity.HasKey(e => e.IdCentro).HasName("centros_medicos_pkey");

            entity.ToTable("centros_medicos");

            entity.Property(e => e.IdCentro).HasColumnName("id_centro");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<ReconocimientoMedico>(entity =>
        {
            entity.HasKey(e => e.IdReconocimiento).HasName("reconocimientos_medicos_pkey");

            entity.ToTable("reconocimientos_medicos");

            entity.HasIndex(e => e.IdAdministrado, "idx_reconocimientos_administrado");

            entity.HasIndex(e => e.IdCentro, "idx_reconocimientos_centro");

            entity.HasIndex(e => e.IdUsuario, "idx_reconocimientos_usuario");

            entity.HasIndex(e => e.CodigoCertificado, "reconocimientos_medicos_codigo_certificado_key").IsUnique();

            entity.Property(e => e.IdReconocimiento).HasColumnName("id_reconocimiento");
            entity.Property(e => e.Adjuntos).HasColumnName("adjuntos");
            entity.Property(e => e.CodigoCertificado)
                .HasMaxLength(50)
                .HasColumnName("codigo_certificado");
            entity.Property(e => e.FechaAlta)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_alta");
            entity.Property(e => e.FechaRealizacion).HasColumnName("fecha_realizacion");
            entity.Property(e => e.FechaValidez).HasColumnName("fecha_validez");
            entity.Property(e => e.IdAdministrado).HasColumnName("id_administrado");
            entity.Property(e => e.IdCentro).HasColumnName("id_centro");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");

            entity.HasOne(d => d.IdAdministradoNavigation).WithMany(p => p.ReconocimientosMedicos)
                .HasForeignKey(d => d.IdAdministrado)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_reconocimientos_administrado");

            entity.HasOne(d => d.IdCentroNavigation).WithMany(p => p.ReconocimientosMedicos)
                .HasForeignKey(d => d.IdCentro)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_reconocimientos_centro");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ReconocimientosMedicos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_reconocimientos_usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.IdCentro, "idx_usuarios_centro");

            entity.HasIndex(e => e.DocIdentidad, "usuarios_doc_identidad_key").IsUnique();

            entity.HasIndex(e => e.Email, "usuarios_email_key").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(100)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(100)
                .HasColumnName("apellido2");
            entity.Property(e => e.CertificadoDigital).HasColumnName("certificado_digital");
            entity.Property(e => e.DocIdentidad)
                .HasMaxLength(20)
                .HasColumnName("doc_identidad");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FechaValidez).HasColumnName("fecha_validez");
            entity.Property(e => e.IdCentro).HasColumnName("id_centro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdCentroNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdCentro)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_usuarios_centro");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
