using System;
using System.Collections.Generic;
using ChatBotWS.Models.WhatsAppAdmin;
using Microsoft.EntityFrameworkCore;

namespace ChatBotWS.Models;

public partial class ChatBotWsContext : DbContext
{
    public ChatBotWsContext()
    {
    }

    public ChatBotWsContext(DbContextOptions<ChatBotWsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contacto> Contactos { get; set; }

    public virtual DbSet<Mensaje> Mensajes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Server=chatbotws.database.windows.net;Database=ChatBotWS;User ID=vico;Password=Hugoca13@;Encrypt=True;TrustServerCertificate=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.HasKey(e => e.ContactoId).HasName("PK__Contacto__8E0F85E88E1DF8BA");

            entity.Property(e => e.Etiqueta).HasColumnName("Etiqueta ");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
        });

        modelBuilder.Entity<Mensaje>(entity =>
        {
            entity.HasKey(e => e.MensajeId).HasName("PK__Mensajes__FEA0555F87580C7E");

            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.Mensaje1).HasColumnName("Mensaje");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
