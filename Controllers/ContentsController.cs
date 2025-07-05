using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using neurotab_api.Models;
using neurotab_api.Data;

namespace neurotab_api.Controllers;

public class ContentsController : ControllerBase
{
    private readonly NeuroTabContext _context;
    public ContentsController(NeuroTabContext context)
    {
        _context = context;
    }
}