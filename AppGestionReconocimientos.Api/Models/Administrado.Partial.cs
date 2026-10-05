using AppGestionReconocimientos.Api.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGestionReconocimientos.Api.Models
{
    public partial class Administrado
    {
        [Column("perfil")]
        public PerfilAdministrado Perfil { get; set; }
    }
}
