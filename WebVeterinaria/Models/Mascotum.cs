using System;
using System.Collections.Generic;

namespace WebVeterinaria.Models;

public partial class Mascotum
{
    public int IdMascota { get; set; }

    public int IdDueno { get; set; }

    public int IdEspecie { get; set; }

    public int IdRaza { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Sexo { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public decimal? Peso { get; set; }

    public string? Color { get; set; }

    public string? Alergias { get; set; }

    public string? Observaciones { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual Dueno IdDuenoNavigation { get; set; } = null!;

    public virtual Especie IdEspecieNavigation { get; set; } = null!;

    public virtual Raza IdRazaNavigation { get; set; } = null!;

    public virtual ICollection<MascotaVacuna> MascotaVacunas { get; set; } = new List<MascotaVacuna>();
}
