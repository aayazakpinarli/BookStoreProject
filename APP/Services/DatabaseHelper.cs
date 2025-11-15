using APP.Domain; // or wherever your AppDbContext is
using Microsoft.EntityFrameworkCore;
using System;

public class DatabaseHelper
{
    private readonly DbContext _context;

    public DatabaseHelper(DbContext context)
    {
        _context = context;
    }

    public void TruncateAllTables()
    {
        var tables = new string[] { "Genres", "Authors", "Books", "BookGenres", "Cities", "Countries"  };

        foreach (var table in tables)
        {
            // Delete all rows
            _context.Database.ExecuteSqlRaw($"DELETE FROM {table};");

            // Reset auto-increment
            _context.Database.ExecuteSqlRaw($"DELETE FROM sqlite_sequence WHERE name='{table}';");
        }

        _context.SaveChanges();
    }
}