using Microsoft.AspNetCore.Mvc;
using Sustainsys.Saml2.WebSso;
using System.Security.Cryptography.X509Certificates;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace SamlProxyApp.Controllers
{
    [Route("saml")]
    public class SamlController : Controller
    {
        private readonly IConfiguration _configuration;

        public SamlController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("initiate")]
        public IActionResult Initiate(string returnUrl)
        {
            var binding = new Saml2RedirectBinding();
            var request = new Saml2AuthenticationRequest
            {
                AssertionConsumerServiceUrl = new Uri("https://saml.pubstrat.com/saml/acs"),
                DestinationUrl = new Uri("https://login.microsoftonline.com/<TENANT-ID>/saml2"),
                Issuer = new EntityId("https://saml.pubstrat.com/Saml2")
            };

            binding.Bind(request);
            return binding.ToActionResult();
        }

        [HttpPost("acs")]
        public IActionResult Acs()
        {
            var binding = new Saml2PostBinding();
            var response = binding.Unbind(Request.ToSaml2Response());

            // Validate response
            if (!response.IsSigned || !response.SignatureValidated)
            {
                return BadRequest("Invalid SAML Response signature.");
            }

            return Ok("SAML Response processed.");
        }

        [HttpGet("metadata")]
        public IActionResult Metadata()
        {
            var metadata = new Saml2Metadata();
            return Content(metadata.ToXml(), "application/xml");
        }

        private X509Certificate2 GetSigningCertificate()
        {
            var keyVaultUri = _configuration["KeyVault:Uri"];
            var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
            var secret = client.GetSecret("SamlSigningCert");
            var certBytes = Convert.FromBase64String(secret.Value.Value);
            return new X509Certificate2(certBytes, (string)null, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
        }
    }
}