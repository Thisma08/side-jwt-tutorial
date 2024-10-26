using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TutoJwt.Services;

public class UserService : IUserService
{
    private List<User> _users = new List<User>
    {
        new User{Username = "Admin", Password = "Password"}
        
    };
    
    // Pour accéder à la clé JWT se trouvant dans appsettings.json
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Login(User user)
    {
        var loginUser = _users.SingleOrDefault(x => x.Username == user.Username && x.Password == user.Password);

        if(loginUser == null)
        {
            return string.Empty;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("718bee0add9205c57875e1b0ace10411");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        string userToken = tokenHandler.WriteToken(token);
        return userToken;
    }
}