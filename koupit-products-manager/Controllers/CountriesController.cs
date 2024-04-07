using koupit_products_manager.Models;
using koupit_products_manager.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace koupit_products_manager.Controllers;

public class CountriesController : Controller
{
    private readonly PostgresDbContext _context;

    public CountriesController(PostgresDbContext context)
    {
        _context = context;
    }

    // GET: Countries
    public async Task<IActionResult> Index()
    {
        var countries = await _context.Countries.Select(c => new Country
        {
            Id = c.Id,
            Name = c.Name,
            Code = c.Code,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            DeletedAt = c.DeletedAt
        }).ToListAsync();
        
        countries.Sort((a, b) => a.Id.CompareTo(b.Id));

        return View(countries);
    }

    // GET: Countries/Details/{id}
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var country = await _context.Countries
            .Select(c => new Country
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                DeletedAt = c.DeletedAt
            }).FirstOrDefaultAsync(m => m.Id == id);

        if (country == null)
        {
            return NotFound();
        }

        return View(country);
    }

    // GET: Countries/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Countries/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Code")] Country country)
    {
        if (!ModelState.IsValid)
        {
            return View(country);
        }

        _context.Add(country);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Countries/Edit/{id}
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);
        if (country == null)
        {
            return NotFound();
        }

        return View(country);
    }

    // POST: Countries/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code,CreatedAt,UpdatedAt,DeletedAt")] Country country)
    {
        if (!ModelState.IsValid)
        {
            return View(country);
        }

        try
        {
            country.UpdatedAt = DateTimeOffset.UtcNow;

            _context.Update(country);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CountryExists(country.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Countries/Delete/{id}
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var country = await _context.Countries.FirstOrDefaultAsync(m => m.Id == id);
        if (country == null)
        {
            return NotFound();
        }

        return View(country);
    }

    // POST: Countries/Delete/{id}
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);

        if (country == null)
        {
            return NotFound();
        }

        country.DeletedAt = DateTimeOffset.UtcNow;
        _context.Update(country);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool CountryExists(int id)
    {
        return _context.Countries.Any(e => e.Id == id);
    }
}