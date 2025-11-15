using APP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

public class AdminController : Controller
{
    private readonly DbContext _context;

    public AdminController(DbContext context)
    {
        _context = context;
    }

    public IActionResult Truncate()
    {
        var helper = new DatabaseHelper(_context);
        helper.TruncateAllTables();
        return Content("Database truncated!");
    }
}
