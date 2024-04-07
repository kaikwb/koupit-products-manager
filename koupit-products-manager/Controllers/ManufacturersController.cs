using koupit_products_manager.Models;
using koupit_products_manager.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace koupit_products_manager.Controllers;

public class ManufacturersController : Controller
{
    private readonly PostgresDbContext _context;

    public ManufacturersController(PostgresDbContext context)
    {
        _context = context;
    }

    // GET: Manufacturers
    public async Task<IActionResult> Index()
    {
        var manufacturers = await _context.Manufacturers.Include(m => m.Country).ToListAsync();
        manufacturers.Sort((a, b) => a.Id.CompareTo(b.Id));

        return View(manufacturers);
    }

    // GET: Manufacturers/Details/{id}
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var manufacturer = await _context.Manufacturers
            .Include(m => m.Country)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (manufacturer == null)
        {
            return NotFound();
        }

        return View(manufacturer);
    }

    // GET: Manufacturers/Create
    public IActionResult Create()
    {
        var countries = _context.Countries.ToList();
        countries.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        ViewBag.Countries = countries;

        return View();
    }

    // POST: Manufacturers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,CountryId")] Manufacturer manufacturer)
    {
        var country = await _context.Countries.FindAsync(manufacturer.CountryId);
        manufacturer.Country = country!;

        ModelState.Clear();
        TryValidateModel(manufacturer);

        if (ModelState.IsValid)
        {
            _context.Add(manufacturer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Countries = _context.Countries.ToList();

        return View(manufacturer);
    }

    // GET: Manufacturers/Edit/{id}
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var manufacturer = await _context.Manufacturers.FindAsync(id);
        if (manufacturer == null)
        {
            return NotFound();
        }

        ViewBag.Countries = _context.Countries.ToList();

        return View(manufacturer);
    }

    // POST: Manufacturers/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Uuid,Name,CountryId,CreatedAt")] Manufacturer manufacturer)
    {
        if (id != manufacturer.Id)
        {
            return NotFound();
        }

        var country = await _context.Countries.FindAsync(manufacturer.CountryId);
        manufacturer.Country = country!;

        ModelState.Clear();
        TryValidateModel(manufacturer);

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(manufacturer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManufacturerExists(manufacturer.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Countries = _context.Countries.ToList();

        return View(manufacturer);
    }

    // GET: Manufacturers/Delete/{id}
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var manufacturer = await _context.Manufacturers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (manufacturer == null)
        {
            return NotFound();
        }

        return View(manufacturer);
    }

    // POST: Manufacturers/Delete/{id}
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var manufacturer = await _context.Manufacturers.FindAsync(id);

        if (manufacturer == null)
        {
            return NotFound();
        }
        
        manufacturer.UpdatedAt = DateTimeOffset.UtcNow;
        manufacturer.DeletedAt = DateTimeOffset.UtcNow;
        
        _context.Update(manufacturer);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool ManufacturerExists(int id)
    {
        return _context.Manufacturers.Any(e => e.Id == id);
    }
}