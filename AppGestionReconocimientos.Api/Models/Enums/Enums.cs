using NpgsqlTypes;

namespace AppGestionReconocimientos.Api.Models.Enums;

public enum RolUsuario
{
    [PgName("administrador")] Administrador,
    [PgName("medico")] Medico,
    [PgName("administrativo")] Administrativo,
    [PgName("generico_centro")] GenericoCentro
}

public enum ResultadoReconocimiento
{
    [PgName("apto")] Apto,
    [PgName("no apto")] NoApto,
    [PgName("apto temporal")] AptoTemporal
}

public enum PerfilAdministrado
{
    [PgName("maquinista")] Maquinista,
    [PgName("administrativo")] Administrativo
}