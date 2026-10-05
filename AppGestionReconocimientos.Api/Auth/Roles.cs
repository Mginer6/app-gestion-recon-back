using AppGestionReconocimientos.Api.Models.Enums;

namespace AppGestionReconocimientos.Api.Auth
{
    public static class Roles
    {
        public const string Administrador = nameof(RolUsuario.Administrador);
        public const string Medico = nameof(RolUsuario.Medico);
        public const string Administrativo = nameof(RolUsuario.Administrativo);
        public const string GenericoCentro = nameof(RolUsuario.GenericoCentro);
    }
}
