using System;
using System.Collections.Generic;

namespace ChatBotWS.Models;

public partial class Mensaje
{
    public int MensajeId { get; set; }

    public string NumeroEmisor { get; set; } = null!;

    public string WaId { get; set; } = null!;

    public string NumeroReceptor { get; set; } = null!;

    public string Mensaje1 { get; set; } = null!;

    public string Respuesta { get; set; } = null!;

    public DateTime FechaHora { get; set; }
}
