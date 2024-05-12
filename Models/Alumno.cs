using System;
using System.Collections.Generic;

namespace ChatBotWS.Models;

public partial class Alumno
{
    public int AlumnoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public long Telefono { get; set; }
}
