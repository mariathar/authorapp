using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthorApp.Models.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "AuthorApp";
        public const string AUDIENCE = "client";
        const string KEY = "ff5eb75f-24cd-491f-a44d-18cf1329c4c3";
        public const int LIFETIME = 30;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
