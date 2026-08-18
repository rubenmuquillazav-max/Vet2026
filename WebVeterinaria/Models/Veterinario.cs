using System;
using System.Collections.Generic;

namespace WebVeterinaria.Models;

public partial class Veterinario
{
    public int IdVeterinario { get; set; }

    public int? IdEspecialidad { get; set; }

    public string? Dni { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public string? Cmpv { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual Especialidad? IdEspecialidadNavigation { get; set; }
}
