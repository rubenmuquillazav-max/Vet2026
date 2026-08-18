using System;
using System.Collections.Generic;

namespace WebVeterinaria.Models;

public partial class Atencion
{
    public int IdAtencion { get; set; }

    public int? IdCita { get; set; }

    public decimal? Temperatura { get; set; }

    public decimal? Peso { get; set; }

    public string? Diagnostico { get; set; }

    public string? Tratamiento { get; set; }

    public string? Observaciones { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Citum? IdCitaNavigation { get; set; }
}
