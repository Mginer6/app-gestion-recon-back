using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace AppGestionReconocimientos.Api.Controllers
{
    [ApiController]
    [Route("saml2")]
    public class Saml2Controller : ControllerBase
    {
        private readonly Saml2Configuration _config;

        public Saml2Controller(Saml2Configuration config)
        {
            _config = config;
        }

        [HttpGet("metadata")]
        public IActionResult Metadata()
        {
            var entityDescriptor = new EntityDescriptor(_config)
            {
                ValidUntil = 365,    // días de validez del metadata
                SPSsoDescriptor = new SPSsoDescriptor
                {
                    AuthnRequestsSigned = true,
                    WantAssertionsSigned = true,
                    SigningCertificates = [_config.SigningCertificate],
                    SingleLogoutServices = [],
                    AssertionConsumerServices = 
                    [
                        new AssertionConsumerService
                        {
                            Binding = ProtocolBindings.HttpPost,
                            Location = new Uri($"{Request.Scheme}://{Request.Host}/saml2/acs")
                        }
                    ],
                }
            };

            return new Saml2Metadata(entityDescriptor).CreateMetadata().ToActionResult();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var binding = new Saml2RedirectBinding();
            var authnRequest = new Saml2AuthnRequest(_config)
            {
                // más adelante aquí fijaremos el ForceAuthn, el nivel
                // de aseguramiento exigido (RequestAuthnContext), etc.
                // (cosas que Cl@ve sí exige configurar en la guía de integración)
            };

            return binding.Bind(authnRequest).ToActionResult();
        }

        [HttpPost("acs")]
        public async Task<IActionResult> AssertionConsumerService()
        {
            var binding = new Saml2PostBinding();
            var saml2AuthnResponse = new Saml2AuthnResponse(_config);

            binding.ReadSamlResponse(Request.ToGenericHttpRequest(), saml2AuthnResponse);

            if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
            {
                return BadRequest($"Autenticación fallida: {saml2AuthnResponse.Status}");
            }

            binding.Unbind(Request.ToGenericHttpRequest(), saml2AuthnResponse);

            // Aquí, en saml2AuthnResponse.ClaimsIdentity, tendrás los claims
            // que devuelva el IdP: NIF, Nombre, apell., nivel de autenticación, ...
            var claims = saml2AuthnResponse.ClaimsIdentity?.Claims;

            // TODO: siguiente paso - mapear estos claims a tu propio usuario
            // y generar tu JWT/cookie de sesión interna

            return Ok(claims?.Select(c => new { c.Type, c.Value }));
        }

    }
}
