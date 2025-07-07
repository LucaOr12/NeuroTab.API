using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId == null)
            return Unauthorized();
        
        tab.UserId = Guid.Parse(userId);
        tab.CreatedAt = DateTime.UtcNow;
        tab.UpdatedAt = DateTime.UtcNow;
        _context.Tabs.Add(tab);
        await _context.SaveChangesAsync();
        return Ok(tab);
    }
}