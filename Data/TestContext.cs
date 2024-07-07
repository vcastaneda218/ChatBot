using System;
using System.Collections.Generic;
using ChatBotWS.Models;
using ChatBotWS.Models.WhatsAppAdmin;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ChatBotWS.Data;

public partial class TestContext : DbContext
{
    public TestContext()
    {
    }

    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contacto> Contactos { get; set; }

    public virtual DbSet<Mensaje> Mensajes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=216.225.195.85;port=3306;database=ChatBotWS;uid=vico;pwd=Hugoca13@", ServerVersion.Parse("10.6.18-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.HasKey(e => e.ContactoId).HasName("PRIMARY");

            entity.Property(e => e.ContactoId).HasColumnType("int(11)");
            entity.Property(e => e.Activo).HasColumnType("tinyint(4)");
            entity.Property(e => e.Chatbot).HasColumnType("tinyint(4)");
            entity.Property(e => e.Favorito).HasColumnType("tinyint(4)");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime(3)");
        });

        modelBuilder.Entity<Mensaje>(entity =>
        {
            entity.HasKey(e => e.MensajeId).HasName("PRIMARY");

            entity.Property(e => e.MensajeId).HasColumnType("int(11)");
            entity.Property(e => e.FechaHora).HasColumnType("datetime(3)");
            entity.Property(e => e.Mensaje1).HasColumnName("Mensaje");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
