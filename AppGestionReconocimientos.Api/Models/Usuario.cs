using System;
using System.Collections.Generic;

namespace AppGestionReconocimientos.Api.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string? Apellido2 { get; set; }

    public string DocIdentidad { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly? FechaValidez { get; set; }

    public int IdCentro { get; set; }

    public bool CertificadoDigital { get; set; }

    public virtual ICollection<Administrado> Administrados { get; set; } = new List<Administrado>();

    public virtual CentroMedico IdCentroNavigation { get; set; } = null!;

    public virtual ICollection<ReconocimientoMedico> ReconocimientosMedicos { get; set; } = new List<ReconocimientoMedico>();
}
