using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace AwesomeAlexaSkill.Handlers
{
    public class RequestValidationHandler : System.Net.Http.DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            //check if the request has a signature
            if(!request.Headers.Contains("Signature") || !request.Headers.Contains("SignatureCertChainUrl"))
                ThrowBadRequest();

            var signatureCertChainUrl = request.Headers.GetValues("SignatureCertChainUrl").First().Replace("/../","/");

            if (string.IsNullOrWhiteSpace(signatureCertChainUrl))
                ThrowBadRequest();

            var certUrl = new Uri(signatureCertChainUrl);
            if(!((certUrl.Port == 443 || certUrl.IsDefaultPort)
                && certUrl.Scheme.Equals(Uri.UriSchemeHttps,StringComparison.OrdinalIgnoreCase)
                && certUrl.Host.Equals("s3.amazonaws.com", StringComparison.OrdinalIgnoreCase)
                && certUrl.AbsolutePath.StartsWith("/echo.api/", StringComparison.OrdinalIgnoreCase)))
                ThrowBadRequest();

            using(var web = new System.Net.WebClient())
            {
                var certificate = web.DownloadData(certUrl);
                var cert = new X509Certificate2(certificate);

                var effectiveDate = DateTime.MinValue;
                var expirationDate = DateTime.MinValue;

                if(!((DateTime.TryParse(cert.GetExpirationDateString(),out expirationDate)
                    && expirationDate > DateTime.UtcNow)
                    && (DateTime.TryParse(cert.GetEffectiveDateString(), out effectiveDate)
                    && effectiveDate < DateTime.UtcNow)))
                    ThrowBadRequest();

                if(!cert.Subject.Contains("CN-echo-api.amazon.com") || !cert.Issuer.Contains("CN-VeriSign Class 3 secure"))
                    ThrowBadRequest();

                var signatureString = request.Headers.GetValues("Signature").First();

                byte[] signature = Convert.FromBase64String(signatureString);

                using (var sha1 = new SHA1Managed())
                {
                    var body = await request.Content.ReadAsStringAsync();

                    var data = sha1.ComputeHash(Encoding.UTF8.GetBytes(body));
                    var rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;

                    if(rsa == null || rsa.VerifyHash(data,CryptoConfig.MapNameToOID("SHA1"), signature))
                        ThrowBadRequest();
                }

            }


            return await base.SendAsync(request, cancellationToken);
        }

        public static void ThrowBadRequest()
        {
            throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
        }
    }
}