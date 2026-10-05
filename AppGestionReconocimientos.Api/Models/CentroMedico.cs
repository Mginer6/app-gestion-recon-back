using System;
using System.Collections.Generic;

namespace AppGestionReconocimientos.Api.Models;

public partial class CentroMedico
{
    public int IdCentro { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Administrado> Administrados { get; set; } = new List<Administrado>();

    public virtual ICollection<ReconocimientoMedico> ReconocimientosMedicos { get; set; } = new List<ReconocimientoMedico>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
