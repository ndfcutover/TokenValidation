using JWT.Builder;

using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik1yNS1BVWliZkJpaTdOZDFqQmViYXhib1hXMCJ9.eyJhdWQiOiI3MWJhNGNkYS0xNzJiLTRmNGItYTMzYi03MzZkZjlhNzQ3OWIiLCJpc3MiOiJodHRwczovL2xvZ2luLm1pY3Jvc29mdG9ubGluZS5jb20vYTAxN2ZiODAtN2MyMC00YjRiLWI4ODUtZjNjNDFkYTAyNTkwL3YyLjAiLCJpYXQiOjE2NDIxMDMwODgsIm5iZiI6MTY0MjEwMzA4OCwiZXhwIjoxNjQyMTA2OTg4LCJhaW8iOiJBVlFBcS84VEFBQUFCbWZBWnFGY1duMWNINVFJK3NVaVJvVm9sc281S1VFRFhmeDdDTVoxa1pZUit2azUvNkEyOU96dHBpSHFCak91L0hZL0NCbGh2blZDYWFDYWE1S3B4azh3Y1dzc0pKUTdGeDlSUEp4Q3lSUT0iLCJhdF9oYXNoIjoiRkF6SWNIUUxYaWVMRmQyT29GY0lNZyIsImVtYWlsIjoiR0VOTkFSTy5ESUZSQUlBQHB1YmxpdGFsaWEuaXQiLCJncm91cHMiOlsiUHJldmlzaW9uaV9QODAtRDA4Il0sIm5vbmNlIjoiNjc4OTEwIiwicmgiOiIwLkFUQUFnUHNYb0NCOFMwdTRoZlBFSGFBbGtOcE11bkVyRjB0UG96dHpiZm1uUjVzd0FHMC4iLCJyb2xlcyI6WyJQcmV2aXNpb25pX1A4MC1EMDgiXSwic3ViIjoicTdRQ0FydXV5LTVMNW00RjFLR0RPNGlJaVA4aGswZFQybTgzM2JkZ0JJRSIsInRpZCI6ImEwMTdmYjgwLTdjMjAtNGI0Yi1iODg1LWYzYzQxZGEwMjU5MCIsInV0aSI6ImxHTDJCT0M3QjBLTk1ickhxUHF5QVEiLCJ2ZXIiOiIyLjAiLCJ2ZXJpZmllZF9wcmltYXJ5X2VtYWlsIjpbIkdFTk5BUk8uRElGUkFJQUBwdWJsaXRhbGlhLml0Il0sImVtcGxveWVlTnVtYmVyIjoiRTAwMDIwNzkifQ.TrGh_FC8zLA97aIRrK-SVsiE8BmJZPvRvTp5ydmHkyXKTI-EmD0jqiJjn03Ld2ZlUP8zDbj3XtrdixYyMKWXKRlyHC6JB3PQUFQ6R8LlMWNizZqDEVaZcSUooGe3P_JL1ZibDzatKP1rPtitZWZ6kh9chind0gXGV0S4ohF_UwMQoD3csR3MBiJKWyxxQ5AiUgNBZpy9pohF1vi_8Nn69JYW8IZj9uQNNIxQQKEqV-BZA0S1FErW-UNofq3G5ChefNUNoNI1XZVfZgn8vzRkfZhwva_Yd0RAsrMLElw_MpH5_7UyhQN2gUPuszS-T64qmtQw7Jdiga2SkuJ7OjfLRA";
            string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik1yNS1BVWliZkJpaTdOZDFqQmViYXhib1hXMCJ9.eyJhdWQiOiI5YTRhODBkNi00ZTZhLTRmZjktOWU2MC1iZDdmZWNlYjllZmQiLCJpc3MiOiJodHRwczovL2xvZ2luLm1pY3Jvc29mdG9ubGluZS5jb20vYTAxN2ZiODAtN2MyMC00YjRiLWI4ODUtZjNjNDFkYTAyNTkwL3YyLjAiLCJpYXQiOjE2NDI2OTI4NzYsIm5iZiI6MTY0MjY5Mjg3NiwiZXhwIjoxNjQyNjk2Nzc2LCJhaW8iOiJBVlFBcS84VEFBQUFFMXBLS21lb2NJMm9SblN0ZzlKQXdrL1E0eEpvQVdhVmltTjRaK0RkN2pucGhaUGlDKzVQQS9jV1VDVStPMmhSR3luU1JnMFdhNzJROVpCVnlqVnRwSDA1NmxtY05sSk9EaUpJRkhkODBubz0iLCJhdF9oYXNoIjoieWNqSWFoWkZJbHlDN01DYk9OS3VGZyIsImZhbWlseV9uYW1lIjoiQmVuY2V0dGkiLCJnaXZlbl9uYW1lIjoiU3RlZmFubyIsImdyb3VwcyI6WyJQcmV2aXNpb25pX1A4MC1EMDgiXSwibmFtZSI6IlN0ZWZhbm8gQmVuY2V0dGlfYWdlbnRlIiwibm9uY2UiOiI3MzAxMDA2NTciLCJvaWQiOiJkYzIxODNhOS02N2E5LTQxZmQtOGM2ZC01ZTE4OGVhYjg5YmYiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJzdGVmYW5vLmJlbmNldHRpX2FnZW50ZUBwdWJsaXRhbGlhLml0IiwicmgiOiIwLkFUQUFnUHNYb0NCOFMwdTRoZlBFSGFBbGtOYUFTcHBxVHZsUG5tQzlmLXpybnYwd0FQRS4iLCJyb2xlcyI6WyJQcmV2aXNpb25pX1A4MC1EMDgiXSwic3ViIjoib1dpeldoaUhiaUx1OEVDLXJRQmluek9lV2lSUU9penFUNW9xUWVSdmNmSSIsInRpZCI6ImEwMTdmYjgwLTdjMjAtNGI0Yi1iODg1LWYzYzQxZGEwMjU5MCIsInVwbiI6InN0ZWZhbm8uYmVuY2V0dGlfYWdlbnRlQHB1YmxpdGFsaWEuaXQiLCJ1dGkiOiIxZWVGcXhIRXFVLW92SzJCbjIwZkFnIiwidmVyIjoiMi4wIiwiZW1wbG95ZWVOdW1iZXIiOiJFMDAwNzA5MyJ9.G6Bdo56XENZRIiQuQcIa7VadQmrixDCWdZrPOVhsM10SvYzsGTmI8Qnuo3Z8pmWZYRmIq9FM9e2_C5jwQDwUer-u6FKAxARCkn6JiMWKLIqS4ijUwSnsKymAuP_Hi2HA8YIctw8JYdhnS4RdYdLk2W39gpBuASaHlSg-CvI2z0b8FtpwRUMzhvk5YdDzvSzia58W7GXC4EcPff1OMMA4bVmATydTf_excylHs0kBmql9a5TAt6PiEk3Yab832UNHOKJmDRK3J6uQ0MDE9NkbLTQ6a3ZzNGeDm1TCsmkE2M0wrjHqdHWpq_E4dN3vyjR1abL1IpMOuS6uQmFj-YfbcA";
            JwtHeader header = JwtBuilder.Create().DecodeHeader<JwtHeader>(Token);
            var kid = header.KeyId;
            var _kid = String.Format("{0}: {1}", "Kid", kid);

            var tokenManagement = new TokenManagement();
            var tokenHandler = new JwtSecurityTokenHandler();

            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------");

            Task<bool> task = tokenManagement.IsTokenValid(Token, kid);
            bool TokenIsValid = task.Result;
            
            if (TokenIsValid) 
                Console.WriteLine("==== Valid Token ====");

            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(_kid);
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------");

            var claims = tokenHandler.ReadJwtToken(Token).Claims;            
            
            var aud = String.Format("{0}: {1}", "Aud", claims.FirstOrDefault(item => item.Type == "aud").Value);
            Console.WriteLine(aud);

            var nonce = String.Format("{0}: {1}", "Nonce", claims.FirstOrDefault(item => item.Type == "nonce").Value);
            Console.WriteLine(nonce);

            var employeeNumber = String.Format("{0}: {1}", "EmployeeNumber", claims.FirstOrDefault(item => item.Type == "employeeNumber").Value);
            Console.WriteLine(employeeNumber);

            string userName = null;
            if (claims.Any(x => x.Type.Contains("name")))
                userName = String.Format("{0}: {1}", "User", claims.FirstOrDefault(item => item.Type == "name").Value);
            
            Console.WriteLine(userName.IsNullOrEmpty() ? "Not specified" : userName);

            string email = null;
            if (claims.Any(x => x.Type.Contains("email")))
                email = String.Format("{0}: {1}", "Email", claims.FirstOrDefault(item => item.Type == "email").Value);
            
            Console.WriteLine(email.IsNullOrEmpty() ? "No Mail for User (Validation fail by policy check for mandatory field email)" : email);
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------");

            var claim = tokenHandler.ReadJwtToken(Token);

            var iat = String.Format("{0}: {1} ({2})", "IssueAt", Convert.ToInt64(claims.FirstOrDefault(item => item.Type == "iat").Value.ToString()), claim.ValidFrom.ToString());
            Console.WriteLine(iat);

            var exp = String.Format("{0}:  {1} ({2})", "ExpireAt", Convert.ToInt64(claims.FirstOrDefault(item => item.Type == "exp").Value.ToString()), claim.ValidTo.ToString());            
            Console.WriteLine(exp);
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------");

            var issueAt = claim.ValidFrom;            
            Console.WriteLine(String.Format("New ExpireAt 4 hours example: {0}", tokenManagement.GetJwtDuration(14400, issueAt)));
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------");

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
                    return (validatedToken != null);
                }
                catch (SecurityTokenExpiredException ex)
                {                    
                    Console.WriteLine("**** Invalid Token ****");
                    Console.WriteLine(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                    return false;
                }
            }

            public DateTime GetJwtDuration(long seconds, DateTime issueAt)
            {
                return issueAt.AddSeconds(seconds).ToUniversalTime();
            }
        }
    }
}
