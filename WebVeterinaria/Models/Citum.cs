using System;
using System.Collections.Generic;

namespace WebVeterinaria.Models;

public partial class Citum
{
    public int IdCita { get; set; }

    public int IdMascota { get; set; }

    public int IdVeterinario { get; set; }

    public DateOnly Fecha { get; set; }

    public TimeOnly Hora { get; set; }

    public string? Motivo { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Atencion? Atencion { get; set; }

    public virtual Mascotum IdMascotaNavigation { get; set; } = null!;

    public virtual Veterinario IdVeterinarioNavigation { get; set; } = null!;
}
