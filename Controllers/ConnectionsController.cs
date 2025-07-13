
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using neurotab_api.Models;
using neurotab_api.Data;

namespace neurotab_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConnectionsController : ControllerBase
{
    private readonly NeuroTabContext _context;
    public ConnectionsController(NeuroTabContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Connection>>> GetAll()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        
        return await _context.Connections.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Connection>> Create([FromBody] ConnectionDTO dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        if (!ModelState.IsValid)
        {
            var errors = string.Join("; ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
            return BadRequest("Model validation failed: " + errors);
        }
        
        var fromExists = await _context.Contents.AnyAsync(c => c.Id == dto.FromContentId);
        var toExists = await _context.Contents.AnyAsync(c => c.Id == dto.ToContentId);
        
        if (!fromExists || !toExists)
            return BadRequest("Invalid content IDs");

        var connection = new Connection
        {
            FromContentId = dto.FromContentId,
            ToContentId = dto.ToContentId,
            ConnectionType = dto.ConnectionType,
            Strength = dto.Strength,
            Notes = dto.Notes,
            IsAiGenerated = dto.IsAiGenerated,
            CreatedAt = DateTime.UtcNow
        };

        _context.Connections.Add(connection);
        await _context.SaveChangesAsync();

        return Ok(connection);
    }

    [HttpDelete("{id}/")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        
        var conn = await _context.Connections.FindAsync(id);
        if (conn == null) return NotFound();

        _context.Connections.Remove(conn);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpHead("ping")]
    [AllowAnonymous]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}

public class ConnectionDTO
{
    public Guid FromContentId { get; set; }
    public Guid ToContentId { get; set; }
    public string ConnectionType { get; set; } = string.Empty;
    public int Strength { get; set; } = 1;
    public string? Notes { get; set; }
    public bool IsAiGenerated { get; set; }
}