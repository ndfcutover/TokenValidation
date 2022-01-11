using System;
using System.Linq;
using System.Runtime;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using JWT.Builder;
using JwtHeader = JWT.Builder.JwtHeader;

namespace TokenValidation
{
    internal class Program
    {
        public static void Main()
        {
            /*
             * Nuova Validazione Token
             */

            // Token scaduto 15/12/2021
            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik1yNS1BVWliZkJpaTdOZDFqQmViYXhib1hXMCJ9.eyJhdWQiOiI5YTRhODBkNi00ZTZhLTRmZjktOWU2MC1iZDdmZWNlYjllZmQiLCJpc3MiOiJodHRwczovL2xvZ2luLm1pY3Jvc29mdG9ubGluZS5jb20vYTAxN2ZiODAtN2MyMC00YjRiLWI4ODUtZjNjNDFkYTAyNTkwL3YyLjAiLCJpYXQiOjE2Mzk1NzM5MzUsIm5iZiI6MTYzOTU3MzkzNSwiZXhwIjoxNjM5NTc3ODM1LCJhaW8iOiJBVFFBeS84VEFBQUE3eGplSEY2c0cxMDJPT0oxTExXVGJFR1dTL2YzYjROTm43bE5mZkVNWEZ3Z0Y5OWhZUXd0M0lXbkVUdGNyd0VkIiwiYXRfaGFzaCI6Ii03QU90MUhaeGVsTkJqM251anVhQ0EiLCJlbWFpbCI6Ikdpb3JnaW8uVmlsbGFAbWVkaWFzZXQuaXQiLCJmYW1pbHlfbmFtZSI6IlZpbGxhIiwiZ2l2ZW5fbmFtZSI6Ikdpb3JnaW8iLCJncm91cHMiOlsiUHJldmlzaW9uaV9QODAtRDA4Il0sIm5hbWUiOiJHaW9yZ2lvIFZpbGxhIiwibm9uY2UiOiI4Mjk4OTEzMDEiLCJvaWQiOiI2MmMyNWI5MC1lYjc5LTQ5YzEtYTUwYS01YzNkYWFjYjNlM2YiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJHaW9yZ2lvLlZpbGxhQG1lZGlhc2V0Lml0IiwicmgiOiIwLkFUQUFnUHNYb0NCOFMwdTRoZlBFSGFBbGtOYUFTcHBxVHZsUG5tQzlmLXpybnYwd0FDWS4iLCJyb2xlcyI6WyJQcmV2aXNpb25pX1A4MC1EMDgiXSwic3ViIjoiSnM0dDh3MkRJcTRNTUpjZFdmdkVSaXRZb20xMlFvd1F2aXoyTnVkSW5PayIsInRpZCI6ImEwMTdmYjgwLTdjMjAtNGI0Yi1iODg1LWYzYzQxZGEwMjU5MCIsInVwbiI6Ikdpb3JnaW8uVmlsbGFAbWVkaWFzZXQuaXQiLCJ1dGkiOiJaRHRzeEZ1dG5VV2k0YlFsQVdoZUFBIiwidmVyIjoiMi4wIiwidmVyaWZpZWRfcHJpbWFyeV9lbWFpbCI6WyJHaW9yZ2lvLlZpbGxhQG1lZGlhc2V0Lml0Il0sImVtcGxveWVlTnVtYmVyIjoiMDAwMTA4ODEifQ.gZ3awLZkaTEF9YIAE0RCKjH_ubauCmPlE-l2yKl7yRUllMYwDegwsbudtPTL_k9-uG7MGfkYMe5xvYoPXgIiY9FDL1C3Kcz_X0RqY0aqheil4AA48XTrchph2iU5BwL1Tj9Rw6hoJS_qrq-unpy-jLge5EPkt-lpe8R2575LnG7PY6V1y5woqcBsO2DqpiSYHM-dkIiWl1lVBhSTK5jMT4QRSdGvKJTcEuqASYJELtNucHJX13g6g-jwZaCSsHLWNDnunCG4ocgvlgK0qeWbR3yfz_poZxQpgPdrZMP7lydmKX1cv0OFV_Vg1pZdXdWPleckNCrfUWSLjVN1-16rOQ";

            // Token valido
            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik1yNS1BVWliZkJpaTdOZDFqQmViYXhib1hXMCJ9.eyJhdWQiOiI3MWJhNGNkYS0xNzJiLTRmNGItYTMzYi03MzZkZjlhNzQ3OWIiLCJpc3MiOiJodHRwczovL2xvZ2luLm1pY3Jvc29mdG9ubGluZS5jb20vYTAxN2ZiODAtN2MyMC00YjRiLWI4ODUtZjNjNDFkYTAyNTkwL3YyLjAiLCJpYXQiOjE2NDE5MTA2NzgsIm5iZiI6MTY0MTkxMDY3OCwiZXhwIjoxNjQxOTE0NTc4LCJhaW8iOiJBVlFBcS84VEFBQUFPaXBUM3pDMVFKSjl0UVI3RTZJbXlkMW9EN0JWbUVDT2lPMUxkSjNyeEY4MVI1aHhXVjNIRGZvTmJaWm1sRjhoYTN6VEdQdWxPLzhWWDBwa1l5RDI3Y1NVOGkzem5KdFRWV1BWdGhBdldpQT0iLCJhdF9oYXNoIjoiajRSb0RJZGFZUTZhM3BMREJ2RnNnZyIsImVtYWlsIjoic2lsdmlvLnBhZ2FuaW5AcHVibGl0YWxpYS5pdCIsIm5vbmNlIjoiNjc4OTEwIiwicmgiOiIwLkFUQUFnUHNYb0NCOFMwdTRoZlBFSGFBbGtOcE11bkVyRjB0UG96dHpiZm1uUjVzd0FBYy4iLCJzdWIiOiJuSmo5aEtLSXZNUGxuVElRT3Zza3luOEtYNHF0VXplem96UWNlUWhLYldRIiwidGlkIjoiYTAxN2ZiODAtN2MyMC00YjRiLWI4ODUtZjNjNDFkYTAyNTkwIiwidXRpIjoiWmpQY2cxb2pnMFdfSkFaVDQxdHlBUSIsInZlciI6IjIuMCIsInZlcmlmaWVkX3ByaW1hcnlfZW1haWwiOlsic2lsdmlvLnBhZ2FuaW5AcHVibGl0YWxpYS5pdCJdLCJlbXBsb3llZU51bWJlciI6IkUwMDAyMDgzIn0.Er3GvY9Mv0GW23oZqAWZgWUr2ZZnYWHcLFjkUcMuR2GLArNrlDwf6TN2g25iqauG2dm0njzlD1TDwSEzmd0n2diEwMPj0zwLhtc0Y15Y2dlTNAJj5TVvFzqOg8tYVlRreEMaR95CmGjVlbDQkL1sqyDR6mCRYANCMgvbD3ZFJUAsrUJRfayumVq-MgrvbRtxFlYoGsVt3GyPh_DszbNtdglGCi9RLu45GaZEm1Cmqw_oQcvBXMjSZX2s3Ogng3pIg78DBFiixY3iIIygRn6C1TKwLgwvfvmgOeKG-Mi08FczvRkecm3qErd1riNUxhrQ21yx8RjB2aEtuPIdR3ZDRg";
            string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik1yNS1BVWliZkJpaTdOZDFqQmViYXhib1hXMCJ9.eyJhdWQiOiI3MWJhNGNkYS0xNzJiLTRmNGItYTMzYi03MzZkZjlhNzQ3OWIiLCJpc3MiOiJodHRwczovL2xvZ2luLm1pY3Jvc29mdG9ubGluZS5jb20vYTAxN2ZiODAtN2MyMC00YjRiLWI4ODUtZjNjNDFkYTAyNTkwL3YyLjAiLCJpYXQiOjE2NDE5MTk4MjYsIm5iZiI6MTY0MTkxOTgyNiwiZXhwIjoxNjQxOTIzNzI2LCJhaW8iOiJBVlFBcS84VEFBQUFVb25Qc1gxY0tTNEJBYUVxY3pIb1RxdVZNTFFoOGlnWmNoT0QwZWxoWmNaZkVoZDEvM3htV2FYSXZQNHh2WjNMREZ1ckpWRmw3RlBadHJ3Q3ZmZjJ2T3RaTTRidGpnamRDYkU1VUorTnEyYz0iLCJhdF9oYXNoIjoiWnhYYm8zRXdpUEV3bk9aclRxZ2VwZyIsImVtYWlsIjoic2lsdmlvLnBhZ2FuaW5AcHVibGl0YWxpYS5pdCIsIm5vbmNlIjoiNjc4OTEwIiwicmgiOiIwLkFWNEFnUHNYb0NCOFMwdTRoZlBFSGFBbGtOcE11bkVyRjB0UG96dHpiZm1uUjVzd0FBYy4iLCJzdWIiOiJuSmo5aEtLSXZNUGxuVElRT3Zza3luOEtYNHF0VXplem96UWNlUWhLYldRIiwidGlkIjoiYTAxN2ZiODAtN2MyMC00YjRiLWI4ODUtZjNjNDFkYTAyNTkwIiwidXRpIjoiM3F6WG1vTVhUazZya05UaVVrMU9BQSIsInZlciI6IjIuMCIsInZlcmlmaWVkX3ByaW1hcnlfZW1haWwiOlsic2lsdmlvLnBhZ2FuaW5AcHVibGl0YWxpYS5pdCJdLCJlbXBsb3llZU51bWJlciI6IkUwMDAyMDgzIn0.w0zX8BFuOBU_1Pf-Lat_p9497CKxPEHvS8MYOTAA-iFUYXUGAtAOxTBI3GFtxQJvRBm5KUos0j4GalykebehDpS5lS44M0c4HNV3E0WuEikHT4DEooAfoHxSImDhIsZ62JoyuSh2GpY_y-PJPh9i9_JlltA5uQq6Ec8_9HLznFPE1gySeXamY6zJW3PBencFlbVIsZk_zSl32EOGBSqDApRgOZy2WUbpoXS57vITn9BDMvYgLOR8tbhLPC9KoOdCTOvti_ywrmMlrJCECLXaBGPQiGUELVVQ7RKEUoPgbXCS3CEojkL2u7kzXR2i8RllH4DrwV_dLJBnnXzSIJdUmQ";

            JwtHeader header = JwtBuilder.Create().DecodeHeader<JwtHeader>(Token);
            var kid = header.KeyId;

            var tokenManagement = new TokenManagement();
            Task<bool> task = tokenManagement.IsTokenValid(Token, kid);
            bool TokenIsValid = task.Result;

            if (!TokenIsValid)            
                Console.WriteLine("Token non valido");                
            else
                Console.WriteLine("Ok - Token valido");

            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = tokenHandler.ReadJwtToken(Token).Claims;
            
            var aud = String.Format("{0}  {1}", "Aud", claims.FirstOrDefault(item => item.Type == "aud").Value);
            Console.WriteLine(aud);

            var nonce = String.Format("{0}  {1}", "Nonce", claims.FirstOrDefault(item => item.Type == "nonce").Value);
            Console.WriteLine(nonce);


            var employeeNumber = String.Format("{0}  {1}", "EmployeeNumber", claims.FirstOrDefault(item => item.Type == "employeeNumber").Value);
            Console.WriteLine(employeeNumber);

            var email = String.Format("{0}  {1}", "Email", claims.FirstOrDefault(item => item.Type == "email").Value);
            Console.WriteLine(email);

            var _kid = String.Format("{0}  {1}", "Kid", kid);
            Console.WriteLine(_kid);

            var exp = String.Format("{0}  {1}", "Expire", Convert.ToInt64(claims.FirstOrDefault(item => item.Type == "exp").Value.ToString()));            
            Console.WriteLine(exp);
            
            //DateTime.UnixEpoch.AddSeconds(claims.FirstOrDefault(item => item.Type == "exp").Value).ToUniversalTime();

            var c = tokenHandler.ReadJwtToken(Token);       

            Console.WriteLine(c.ValidFrom.ToString());
            Console.WriteLine(c.ValidTo.ToString());

            Console.ReadKey();
        }

        internal class TokenManagement
        { 
            public async Task<bool> IsTokenValid(string jwtToken, string kid)
            {                
                string endpointKey = "https://login.microsoftonline.com/a017fb80-7c20-4b4b-b885-f3c41da02590/discovery/v2.0/keys";
                string contentResponse = string.Empty;

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(endpointKey);
                    contentResponse = await response.Content.ReadAsStringAsync();
                }
                var jsonWebKeys = new JsonWebKeySet(contentResponse);
                                
                var key = jsonWebKeys.Keys.Where(k => k.Kid == kid).First();

                var validationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
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
                    Console.WriteLine();
                    return false;
                }
            }
        }
    }
}
