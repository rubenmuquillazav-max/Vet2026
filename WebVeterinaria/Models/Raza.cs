using System;
using System.Collections.Generic;

namespace WebVeterinaria.Models;

public partial class Raza
{
    public int IdRaza { get; set; }

    public int IdEspecie { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual Especie IdEspecieNavigation { get; set; } = null!;

    public virtual ICollection<Mascotum> Mascota { get; set; } = new List<Mascotum>();
}
