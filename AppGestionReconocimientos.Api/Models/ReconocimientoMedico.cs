using System;
using System.Collections.Generic;

namespace AppGestionReconocimientos.Api.Models;

public partial class ReconocimientoMedico
{
    public int IdReconocimiento { get; set; }

    public int IdAdministrado { get; set; }

    public int IdUsuario { get; set; }

    public int IdCentro { get; set; }

    public DateOnly FechaRealizacion { get; set; }

    public DateOnly? FechaValidez { get; set; }

    public string? Observaciones { get; set; }

    public string CodigoCertificado { get; set; } = null!;

    public DateTime FechaAlta { get; set; }

    public bool Adjuntos { get; set; }

    public virtual Administrado IdAdministradoNavigation { get; set; } = null!;

    public virtual CentroMedico IdCentroNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
