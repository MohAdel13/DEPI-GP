using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JustTech.Helpers
{
    // This Class is not Used Until now. I created it for future if I need it.
    public static class Helpers
    {
        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        }

    }
}
