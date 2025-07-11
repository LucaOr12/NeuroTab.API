using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using neurotab_api.Models;
using neurotab_api.Data;

namespace neurotab_api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TabsController: ControllerBase
{
    private readonly NeuroTabContext _context;
    public TabsController(NeuroTabContext context)
    {
        _context = context;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Tab>>> GetTabs()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId == null)
            return Unauthorized();
        
        var guid = Guid.Parse(userId);
        var tabs = await _context.Tabs
            .Where(t => t.UserId == guid)
            .Include(t => t.Content)
            .ToListAsync();
        return Ok(tabs);
    }
    
    [HttpPost]
    public async Task<ActionResult<Tab>> CreateTab([FromBody] Tab tab)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        tab.UserId = Guid.Parse(userId);
        tab.Id = Guid.NewGuid();
        tab.CreatedAt = DateTime.UtcNow;
        tab.UpdatedAt = DateTime.UtcNow;
        
        
        if (tab.Content != null)
        {
            foreach (var content in tab.Content)
            {
                content.Id = Guid.NewGuid();
                content.TabId = tab.Id;
                content.CreatedAt = DateTime.UtcNow;
                content.UpdatedAt = DateTime.UtcNow;
            }
        }
        
        tab.Tags ??= new List<string>();

        _context.Tabs.Add(tab);
        await _context.SaveChangesAsync();

        return Ok(tab);
    }

    [HttpPatch("update/{id}")]
    public async Task<ActionResult<Tab>> UpdateTab(Guid id, [FromBody] Tab data)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        
        var existingTab = await _context.Tabs.FindAsync(id);
        if (existingTab == null) return NotFound();
        
        existingTab.Title = data.Title ?? existingTab.Title;
        existingTab.Description = data.Description ?? existingTab.Description;;
        existingTab.UpdatedAt = DateTime.UtcNow ;
        existingTab.Url = data.Url ?? existingTab.Url;
        
        await _context.SaveChangesAsync();
        return Ok(existingTab);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<Tab>> DeleteTab(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        
        var tab = await _context.Tabs.FindAsync(id);
        if (tab == null) return NotFound();
        _context.Tabs.Remove(tab);
        await _context.SaveChangesAsync();
        return Ok(tab);
    }
}