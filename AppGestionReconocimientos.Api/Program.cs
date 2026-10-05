using AppGestionReconocimientos.Api.Data;
using AppGestionReconocimientos.Api.Models.Enums;
using ITfoxtec.Identity.Saml2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


/* CUANDO ESTÉ LA INTEGRACIÓN CON CL@VE, ESTO IRÁ FUERA */
var saml2Section = builder.Configuration.GetSection("Saml2");

var spCertPath = saml2Section["SPCertificate:Path"];
var spCertPassword = saml2Section["SPCertificate:Password"];

var saml2Config = new Saml2Configuration
{
    Issuer = saml2Section["EntityId"],
    SigningCertificate = X509CertificateLoader.LoadPkcs12FromFile(
        spCertPath,
        spCertPassword,
        X509KeyStorageFlags.MachineKeySet),     // en Linux/contenedores puede que no haga falta este flag
};

saml2Config.SingleSignOnDestination = new Uri("https://localhost:7138/idp/sso");

var idpPublicCertPath = Path.Combine(AppContext.BaseDirectory, "Certificados", "idp-dev-public.cer");
saml2Config.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.PeerTrust;
saml2Config.RevocationMode = X509RevocationMode.NoCheck;
saml2Config.SignatureValidationCertificates.Add(X509CertificateLoader.LoadCertificateFromFile(idpPublicCertPath));

saml2Config.AllowedAudienceUris.Add(saml2Config.Issuer);


/* AUTENTICACIÓN */
var jwt = builder.Configuration.GetSection("Jwt");



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(saml2Config);
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("ConexionBBDDPostgreSQLDocker"),
        o =>
        {
            o.MapEnum<RolUsuario>("rol_usuario");
            o.MapEnum<ResultadoReconocimiento>("resultado_reconocimiento");
            o.MapEnum<PerfilAdministrado>("perfil_administrado");
        }
        ));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.MapInboundClaims = false;     // Sirve para que no traduzca los nombres de los claims
        o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
            ValidateLifetime = true,
            RoleClaimType = "role",
            NameClaimType = "name"
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("PermitirFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();