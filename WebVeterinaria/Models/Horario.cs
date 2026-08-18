using System;
using System.Collections.Generic;

namespace WebVeterinaria.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public int IdVeterinario { get; set; }

    public short DiaSemana { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public virtual Veterinario IdVeterinarioNavigation { get; set; } = null!;
}
