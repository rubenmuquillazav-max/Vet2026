using System;
using System.Collections.Generic;

namespace WebVeterinaria.Models;

public partial class MascotaVacuna
{
    public int Id { get; set; }

    public int? IdMascota { get; set; }

    public int? IdVacuna { get; set; }

    public DateOnly? Fecha { get; set; }

    public DateOnly? ProximaDosis { get; set; }

    public string? Observaciones { get; set; }

    public virtual Mascotum? IdMascotaNavigation { get; set; }

    public virtual Vacuna? IdVacunaNavigation { get; set; }
}
