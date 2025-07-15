using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using neurotab_api.Models;
using neurotab_api.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace neurotab_api.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContentsController : ControllerBase
{
    private readonly NeuroTabContext _context;
    public ContentsController(NeuroTabContext context)
    {
        _context = context;
    }
    
    [HttpGet("tab/{tabId}")]
    public async Task<ActionResult<IEnumerable<Content>>> GetContentsByTab(Guid tabId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var tab = await _context.Tabs.FirstOrDefaultAsync(t => t.Id == tabId && t.UserId == Guid.Parse(userId));
        if (tab == null)
            return NotFound("Tab not found or not owned by user");

        var contents = await _context.Contents
            .Where(c => c.TabId == tabId)
            .ToListAsync();

        return Ok(contents);
    }
    
    [HttpPost]
    public async Task<ActionResult<Content>> CreateContent([FromBody] Content content)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var tab = await _context.Tabs.FirstOrDefaultAsync(t => t.Id == content.TabId && t.UserId == Guid.Parse(userId));
        if (tab == null)
            return NotFound("Tab not found or not owned by user");

        content.CreatedAt = DateTime.UtcNow;
        content.UpdatedAt = DateTime.UtcNow;

        _context.Contents.Add(content);
        await _context.SaveChangesAsync();

        return Ok(content);
    }

    [HttpPut("{id}/position")]
    public async Task<IActionResult> UpdateNodePosition(Guid id, [FromBody] PositionDTO pos)
    {
        var content = await _context.Contents.FindAsync(id);
        if (content == null)
        {
            return NotFound();
        }

        content.PositionX = pos.X;
        content.PositionY = pos.Y;
        
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("update/{id}")]
    public async Task<ActionResult<Content>> UpdateContent(Guid id, [FromBody] ContentDTO data)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        
        var existingContent = await _context.Contents.FindAsync(id);
        if (existingContent == null) return NotFound();
        
        existingContent.Title = data.Title ?? existingContent.Title;
        existingContent.Description = data.Description ?? existingContent.Description;
        existingContent.UpdatedAt = DateTime.UtcNow;
        existingContent.Url = data.Url ?? existingContent.Url;
        
        await _context.SaveChangesAsync();
        return Ok(existingContent);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContent(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId == null)
            return Unauthorized();
        var content = await _context.Contents.FindAsync(id);
        if (content == null)
        {
            return NotFound();
        }
        _context.Contents.Remove(content);
        await _context.SaveChangesAsync();
        return Ok(content);
    }
}

public class ContentDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Url { get; set; }
}

public class PositionDTO
{
    public float X { get; set; }
    public float Y { get; set; }
}