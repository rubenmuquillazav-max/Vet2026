using System;
using System.Collections.Generic;

namespace WebVeterinaria.Models;

public partial class Dueno
{
    public int IdDueno { get; set; }

    public string? Dni { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public string? Direccion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Mascotum> Mascota { get; set; } = new List<Mascotum>();
}
