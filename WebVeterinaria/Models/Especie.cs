using System;
using System.Collections.Generic;

namespace WebVeterinaria.Models;

public partial class Especie
{
    public int IdEspecie { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Mascotum> Mascota { get; set; } = new List<Mascotum>();

    public virtual ICollection<Raza> Razas { get; set; } = new List<Raza>();
}
