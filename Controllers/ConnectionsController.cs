using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using neurotab_api.Models;
using neurotab_api.Data;

namespace neurotab_api.Controllers;

public class ConnectionsController : ControllerBase
{
    private readonly NeuroTabContext _context;
    public ConnectionsController(NeuroTabContext context)
    {
        _context = context;
    }
    
    [HttpHead("ping")]
    [AllowAnonymous]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}