using AppGestionReconocimientos.Api.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGestionReconocimientos.Api.Models
{
    public partial class ReconocimientoMedico
    {
        [Column("resultado")]
        public ResultadoReconocimiento Resultado { get; set; }
    }
}
