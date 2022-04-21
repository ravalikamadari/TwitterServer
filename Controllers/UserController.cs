using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TwitterServer.DTOs;
using TwitterServer.Models;
using TwitterServer.Repositories;
using TwitterServer.Utilities;

namespace TwitterServer.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private IUserRepository _user;
    private IConfiguration _configuration;

    public UserController(ILogger<UserController> logger, IUserRepository user, IConfiguration configuration)
    {
        _logger = logger;
        _user = user;
        _configuration = configuration;
    }

    [HttpPost()]
    public async Task<ActionResult<User>> CreateUser([FromBody] UserCreateDTO data)
    {
        if (!IsValidEmailAddress(data.Email))
            return BadRequest("Invalid email address");
        var toCreateUser = new User
        {
            Name = data.Name,
            Email = data.Email.Trim().ToLower(),
            Password = BCrypt.Net.BCrypt.HashPassword(data.Password),
        };
        var createdUser = await _user.Create(toCreateUser);
        return StatusCode(StatusCodes.Status201Created, "User Created");
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UserUpdateDTO data)
    {
        var currentUserId = GetCurrentUserId();
        var toUpdateUser = new User
        {
            Id = currentUserId,
            Name = data.Name
        };
        var didUpdate = await _user.Update(toUpdateUser);
        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update user");
        return Ok("Updated");
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserLoginDTO userLogin)
    {
        if (!IsValidEmailAddress(userLogin.Email))
            return BadRequest("Invalid email address");

        var user = await _user.GetByEmail(userLogin.Email);

        if (user == null)
            return NotFound("User not found");

        if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
            return Unauthorized("Invalid password");

        var token = Generate(user);
        var res = new UserLoginResponseDTO
        {
            Token = token,
            Name = user.Name,
            Email = user.Email,
            Id = user.Id
        };
        return Ok(res);
    }

    private bool IsValidEmailAddress(string email)
    {
        try
        {
            var emailChecked = new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(UserConstants.Id, user.Id.ToString()),
            new Claim(UserConstants.Name, user.Name),
            new Claim(UserConstants.Email, user.Email),
        };

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private long GetCurrentUserId()
    {
        var userClaims = User.Claims;
        return Int64.Parse(userClaims.FirstOrDefault(c => c.Type == UserConstants.Id)?.Value);
    }

}
