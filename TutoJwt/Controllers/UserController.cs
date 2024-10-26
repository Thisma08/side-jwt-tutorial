using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TutoJwt.Services;

namespace TutoJwt.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public IActionResult Login(User user)
    {
        // var token = _userService.Login(user);
        // if (token == null || token == string.Empty)
        // {
        //     return BadRequest(new { message = "Username or Password is incorrect" });
        // }
        //
        // return Ok(token);

        if (user == null)
        {
            return BadRequest("Invalid client request");
        }

        if (user.Username == "Admin" && user.Password == "Password")
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("718bee0add9205c57875e1b0ace10411"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            
            var tokenOptions = new JwtSecurityToken(
                issuer: "Test",
                audience: "https://localhost:5001",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );
            
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new { Token = tokenString });
        }
        else
        {
            return Unauthorized();
        }
    }
    
    [HttpGet("Protected"), Authorize]
    public IActionResult ProtectedAction()
    {
        return Ok("You are authorized!");
    }
}