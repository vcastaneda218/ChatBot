using System;
using System.Collections.Generic;

namespace ChatBotWS.Models.WhatsAppAdmin;

public class Contacto
{
    public int ContactoId { get; set; }

    public string Numero { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public sbyte Favorito { get; set; }

    public string Etiqueta { get; set; } = null!;

    public sbyte Chatbot { get; set; }

    public DateTime FechaCreacion { get; set; }

    public sbyte Activo { get; set; }
}
