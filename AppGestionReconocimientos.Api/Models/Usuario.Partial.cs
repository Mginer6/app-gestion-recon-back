using AppGestionReconocimientos.Api.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGestionReconocimientos.Api.Models
{
    public partial class Usuario
    {
        [Column("rol")]
        public RolUsuario Rol { get; set; }
    }
}
