using System;
using System.Collections.Generic;

namespace WebVeterinaria.Models;

public partial class Vacuna
{
    public int IdVacuna { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<MascotaVacuna> MascotaVacunas { get; set; } = new List<MascotaVacuna>();
}
