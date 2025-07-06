using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using neurotab_api.Models;
using neurotab_api.Data;
using neurotab_api.DTOs;

namespace neurotab_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly NeuroTabContext _context;
    private readonly ILogger<UsersController> _logger;

    public UsersController(NeuroTabContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    [HttpGet("logged-user")]
    [Authorize(AuthenticationSchemes = "CookieAuth")]
    public async Task<IActionResult> LoggedUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var user = await _context.Users
            .Where(u => u.Id.ToString() == userId)
            .Select(u => new UserDTO
            {
                GoogleId = u.GoogleId,
                Email = u.Email,
                Name = u.Name,
                ProfilePictureUrl = u.ProfilePictureUrl,
                CreatedAt = u.CreatedAt,
                LastLoginAt = u.LastLoginAt
            })
            .FirstOrDefaultAsync();

        return user != null ? Ok(user) : Unauthorized();
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

            var googleId = payload.Subject;
            var email = payload.Email;
            var name = payload.Name;
            var picture = payload.Picture;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId);

            if (user == null)
            {
                user = new User
                {
                    GoogleId = googleId,
                    Email = email,
                    Name = name,
                    ProfilePictureUrl = picture,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
            }
            else
            {
                user.Name = name;
                user.Email = email;
                user.ProfilePictureUrl = picture;
                user.UpdatedAt = DateTime.UtcNow;
                user.LastLoginAt = DateTime.UtcNow;

                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(15)
            });

            return Ok(new UserDTO
            {
                GoogleId = user.GoogleId,
                Email = user.Email,
                Name = user.Name,
                ProfilePictureUrl = user.ProfilePictureUrl,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            });
        }
        catch (InvalidJwtException ex)
        {
            _logger.LogWarning("Invalid Google token: {Message}", ex.Message);
            return Unauthorized("Invalid Google token.");
        }
    }
    
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return Ok();
    }
}



public class GoogleLoginRequest
{
    public string IdToken { get; set; } = string.Empty;
}