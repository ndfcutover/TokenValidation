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
            string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik1yNS1BVWliZkJpaTdOZDFqQmViYXhib1hXMCJ9.eyJhdWQiOiI3MWJhNGNkYS0xNzJiLTRmNGItYTMzYi03MzZkZjlhNzQ3OWIiLCJpc3MiOiJodHRwczovL2xvZ2luLm1pY3Jvc29mdG9ubGluZS5jb20vYTAxN2ZiODAtN2MyMC00YjRiLWI4ODUtZjNjNDFkYTAyNTkwL3YyLjAiLCJpYXQiOjE2NDIxMDMwODgsIm5iZiI6MTY0MjEwMzA4OCwiZXhwIjoxNjQyMTA2OTg4LCJhaW8iOiJBVlFBcS84VEFBQUFCbWZBWnFGY1duMWNINVFJK3NVaVJvVm9sc281S1VFRFhmeDdDTVoxa1pZUit2azUvNkEyOU96dHBpSHFCak91L0hZL0NCbGh2blZDYWFDYWE1S3B4azh3Y1dzc0pKUTdGeDlSUEp4Q3lSUT0iLCJhdF9oYXNoIjoiRkF6SWNIUUxYaWVMRmQyT29GY0lNZyIsImVtYWlsIjoiR0VOTkFSTy5ESUZSQUlBQHB1YmxpdGFsaWEuaXQiLCJncm91cHMiOlsiUHJldmlzaW9uaV9QODAtRDA4Il0sIm5vbmNlIjoiNjc4OTEwIiwicmgiOiIwLkFUQUFnUHNYb0NCOFMwdTRoZlBFSGFBbGtOcE11bkVyRjB0UG96dHpiZm1uUjVzd0FHMC4iLCJyb2xlcyI6WyJQcmV2aXNpb25pX1A4MC1EMDgiXSwic3ViIjoicTdRQ0FydXV5LTVMNW00RjFLR0RPNGlJaVA4aGswZFQybTgzM2JkZ0JJRSIsInRpZCI6ImEwMTdmYjgwLTdjMjAtNGI0Yi1iODg1LWYzYzQxZGEwMjU5MCIsInV0aSI6ImxHTDJCT0M3QjBLTk1ickhxUHF5QVEiLCJ2ZXIiOiIyLjAiLCJ2ZXJpZmllZF9wcmltYXJ5X2VtYWlsIjpbIkdFTk5BUk8uRElGUkFJQUBwdWJsaXRhbGlhLml0Il0sImVtcGxveWVlTnVtYmVyIjoiRTAwMDIwNzkifQ.TrGh_FC8zLA97aIRrK-SVsiE8BmJZPvRvTp5ydmHkyXKTI-EmD0jqiJjn03Ld2ZlUP8zDbj3XtrdixYyMKWXKRlyHC6JB3PQUFQ6R8LlMWNizZqDEVaZcSUooGe3P_JL1ZibDzatKP1rPtitZWZ6kh9chind0gXGV0S4ohF_UwMQoD3csR3MBiJKWyxxQ5AiUgNBZpy9pohF1vi_8Nn69JYW8IZj9uQNNIxQQKEqV-BZA0S1FErW-UNofq3G5ChefNUNoNI1XZVfZgn8vzRkfZhwva_Yd0RAsrMLElw_MpH5_7UyhQN2gUPuszS-T64qmtQw7Jdiga2SkuJ7OjfLRA";

            JwtHeader header = JwtBuilder.Create().DecodeHeader<JwtHeader>(Token);
            var kid = header.KeyId;

            var _kid = String.Format("{0}: {1}", "Kid", kid);
            Console.WriteLine(_kid);
            Console.WriteLine("---------------------------------------------------------------------------------");

            var tokenManagement = new TokenManagement();
            Task<bool> task = tokenManagement.IsTokenValid(Token, kid);
            bool TokenIsValid = task.Result;

            if (!TokenIsValid)            
                Console.WriteLine("**** Invalid Token ****");                
            else
                Console.WriteLine("==== Valid Token ====");

            Console.WriteLine("---------------------------------------------------------------------------------");
            Console.WriteLine();
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = tokenHandler.ReadJwtToken(Token).Claims;
            
            var aud = String.Format("{0}: {1}", "Aud", claims.FirstOrDefault(item => item.Type == "aud").Value);
            Console.WriteLine(aud);

            var nonce = String.Format("{0}: {1}", "Nonce", claims.FirstOrDefault(item => item.Type == "nonce").Value);
            Console.WriteLine(nonce);

            var employeeNumber = String.Format("{0}: {1}", "EmployeeNumber", claims.FirstOrDefault(item => item.Type == "employeeNumber").Value);
            Console.WriteLine(employeeNumber);

            var email = String.Format("{0}: {1}", "Email", claims.FirstOrDefault(item => item.Type == "email").Value);
            Console.WriteLine(email);
            Console.WriteLine("---------------------------------------------------------------------------------");

            var claim = tokenHandler.ReadJwtToken(Token);

            var iat = String.Format("{0}: {1} ({2})", "IssueAt", Convert.ToInt64(claims.FirstOrDefault(item => item.Type == "iat").Value.ToString()), claim.ValidFrom.ToString());
            Console.WriteLine(iat);

            var exp = String.Format("{0}:  {1} ({2})", "ExpireAt", Convert.ToInt64(claims.FirstOrDefault(item => item.Type == "exp").Value.ToString()), claim.ValidTo.ToString());            
            Console.WriteLine(exp);
            Console.WriteLine("---------------------------------------------------------------------------------");
            
            var issueAt = claim.ValidFrom;
            var expire = new TokenExpire();

            Console.WriteLine(String.Format("New ExpireAt: {0}", expire.GetJwtDuration(14400, issueAt)));
            Console.WriteLine("---------------------------------------------------------------------------------");

            Console.ReadKey();
        }

        internal class TokenExpire
        {
            public DateTime GetJwtDuration(long seconds, DateTime issueAt)
            {
                return issueAt.AddSeconds(seconds).ToUniversalTime();
            }
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
