using System;
using System.Collections.Generic;

namespace ChatBotWS.Models.WhatsAppAdmin;

public class Contacto
{
    public int ContactoId { get; set; }

    public string Numero { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public bool Favorito { get; set; }

    public string Etiqueta { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public bool Activo { get; set; }

    public bool? Chatbot { get; set; }
}
