using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.DAL;
using backend.Interface;
using backend.Model;
using Microsoft.IdentityModel.Tokens;

namespace backend.Service;

public class TokenService : ITokenService
{
    private ITokenDAL _tokenDal;
    
    private static readonly byte[] Secret = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!);

    public TokenService(ITokenDAL tokenDal)
    {
        _tokenDal = tokenDal;
    }

    public string createToken(User user)
    {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                }),
                Expires = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now.AddDays(7)),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(Secret), SecurityAlgorithms.HmacSha256Signature)
            };

        
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
    }

    public User validateTokenAndReturnUser(string token)
    {
            var principal = validateAndReturnToken(token); //Validating the token has not been tampered
    
            var nameClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name); // Saves username of the user

            User userFromToken = _tokenDal.userFromUsername(nameClaim.Value);

            return userFromToken;
    }
    

    private ClaimsPrincipal validateAndReturnToken(string token)
    {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Secret),
                ValidateIssuer = false, // No Issuer, not needed for this scale of program
                ValidateAudience = false, // No targeted audience
                ClockSkew = TimeSpan.Zero // Amount of time the token can be over date
            };

            SecurityToken validatedToken;
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            return principal;
    }
    
}