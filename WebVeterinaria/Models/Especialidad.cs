using System;
using System.Collections.Generic;

namespace WebVeterinaria.Models;

public partial class Especialidad
{
    public int IdEspecialidad { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Veterinario> Veterinarios { get; set; } = new List<Veterinario>();
}
