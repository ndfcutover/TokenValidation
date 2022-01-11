using System;
using System.Linq;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;


namespace TokenValidation
{
    internal class Program
    {
        public static void Main()
        {
            /*
             * Nuova Validazione Token
             */

            string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik1yNS1BVWliZkJpaTdOZDFqQmViYXhib1hXMCJ9.eyJhdWQiOiI5YTRhODBkNi00ZTZhLTRmZjktOWU2MC1iZDdmZWNlYjllZmQiLCJpc3MiOiJodHRwczovL2xvZ2luLm1pY3Jvc29mdG9ubGluZS5jb20vYTAxN2ZiODAtN2MyMC00YjRiLWI4ODUtZjNjNDFkYTAyNTkwL3YyLjAiLCJpYXQiOjE2Mzk1NzM5MzUsIm5iZiI6MTYzOTU3MzkzNSwiZXhwIjoxNjM5NTc3ODM1LCJhaW8iOiJBVFFBeS84VEFBQUE3eGplSEY2c0cxMDJPT0oxTExXVGJFR1dTL2YzYjROTm43bE5mZkVNWEZ3Z0Y5OWhZUXd0M0lXbkVUdGNyd0VkIiwiYXRfaGFzaCI6Ii03QU90MUhaeGVsTkJqM251anVhQ0EiLCJlbWFpbCI6Ikdpb3JnaW8uVmlsbGFAbWVkaWFzZXQuaXQiLCJmYW1pbHlfbmFtZSI6IlZpbGxhIiwiZ2l2ZW5fbmFtZSI6Ikdpb3JnaW8iLCJncm91cHMiOlsiUHJldmlzaW9uaV9QODAtRDA4Il0sIm5hbWUiOiJHaW9yZ2lvIFZpbGxhIiwibm9uY2UiOiI4Mjk4OTEzMDEiLCJvaWQiOiI2MmMyNWI5MC1lYjc5LTQ5YzEtYTUwYS01YzNkYWFjYjNlM2YiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJHaW9yZ2lvLlZpbGxhQG1lZGlhc2V0Lml0IiwicmgiOiIwLkFUQUFnUHNYb0NCOFMwdTRoZlBFSGFBbGtOYUFTcHBxVHZsUG5tQzlmLXpybnYwd0FDWS4iLCJyb2xlcyI6WyJQcmV2aXNpb25pX1A4MC1EMDgiXSwic3ViIjoiSnM0dDh3MkRJcTRNTUpjZFdmdkVSaXRZb20xMlFvd1F2aXoyTnVkSW5PayIsInRpZCI6ImEwMTdmYjgwLTdjMjAtNGI0Yi1iODg1LWYzYzQxZGEwMjU5MCIsInVwbiI6Ikdpb3JnaW8uVmlsbGFAbWVkaWFzZXQuaXQiLCJ1dGkiOiJaRHRzeEZ1dG5VV2k0YlFsQVdoZUFBIiwidmVyIjoiMi4wIiwidmVyaWZpZWRfcHJpbWFyeV9lbWFpbCI6WyJHaW9yZ2lvLlZpbGxhQG1lZGlhc2V0Lml0Il0sImVtcGxveWVlTnVtYmVyIjoiMDAwMTA4ODEifQ.gZ3awLZkaTEF9YIAE0RCKjH_ubauCmPlE-l2yKl7yRUllMYwDegwsbudtPTL_k9-uG7MGfkYMe5xvYoPXgIiY9FDL1C3Kcz_X0RqY0aqheil4AA48XTrchph2iU5BwL1Tj9Rw6hoJS_qrq-unpy-jLge5EPkt-lpe8R2575LnG7PY6V1y5woqcBsO2DqpiSYHM-dkIiWl1lVBhSTK5jMT4QRSdGvKJTcEuqASYJELtNucHJX13g6g-jwZaCSsHLWNDnunCG4ocgvlgK0qeWbR3yfz_poZxQpgPdrZMP7lydmKX1cv0OFV_Vg1pZdXdWPleckNCrfUWSLjVN1-16rOQ";

            var tokenManagement = new TokenManagement();
            Task<bool> task = tokenManagement.IsTokenValid(Token);
            bool TokenIsValid = task.Result;

            if (!TokenIsValid)
            {
                Console.WriteLine("Token non valido");
                Console.ReadKey();
            }
                
        }

        internal class TokenManagement
        { 
            public async Task<bool> IsTokenValid(string jwtToken)
            {
                string kid = "Mr5-AUibfBii7Nd1jBebaxboXW0";
                string endpointKey = "https://login.microsoftonline.com/a017fb80-7c20-4b4b-b885-f3c41da02590/discovery/v2.0/keys";
                string contentResponse = string.Empty;

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(endpointKey);
                    contentResponse = await response.Content.ReadAsStringAsync();
                }
                var jsonWebKeys = new JsonWebKeySet(contentResponse);
                var firstKey = jsonWebKeys.Keys.Where(k => k.Kid == kid).First();
                var validationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = firstKey,
                    ValidateAudience = false,
                    ValidateIssuer = false
                    //ValidAudience = jwtSettings.AudienceSso, // Your API Audience, can be disabled via ValidateAudience = false
                    //ValidIssuer = jwtSettings.IssuerSSo      // Your token issuer, can be disabled via ValidateIssuer = false
                };

                var isValid = ValidateToken(jwtToken, validationParameters);

                return isValid;
            }

            private bool ValidateToken(string token, TokenValidationParameters validationParameters)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                    return validatedToken != null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
    }
}
