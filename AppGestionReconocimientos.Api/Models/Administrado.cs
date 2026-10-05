using System;
using System.Collections.Generic;

namespace AppGestionReconocimientos.Api.Models;

public partial class Administrado
{
    public int IdAdministrado { get; set; }

    public string DocIdentidad { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string? Apellido2 { get; set; }

    public string? Direccion { get; set; }

    public string? Mail { get; set; }

    public string? Telefono { get; set; }

    public string? LicenciaMaquinista { get; set; }

    public string? Observaciones { get; set; }

    public int IdCentro { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaAlta { get; set; }

    public virtual CentroMedico IdCentroNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<ReconocimientoMedico> ReconocimientosMedicos { get; set; } = new List<ReconocimientoMedico>();
}
