using koupit_products_manager.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Attribute = koupit_products_manager.Models.Attribute;

namespace koupit_products_manager.Controllers;

public class AttributesController : Controller
{
    private readonly PostgresDbContext _context;

    public AttributesController(PostgresDbContext context)
    {
        _context = context;
    }

    // GET: Attributes
    public async Task<IActionResult> Index()
    {
        var attributes = await _context.Attributes
            .Where(a => a.DeletedAt == null)
            .ToListAsync();

        attributes.Sort((a, b) => a.Id.CompareTo(b.Id));

        return View(attributes);
    }

    // GET: Attributes/Details/{id}
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var attribute = await _context.Attributes
            .Where(a => a.DeletedAt == null)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (attribute == null)
        {
            return NotFound();
        }

        return View(attribute);
    }

    // GET: Attributes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Attributes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,PrettyName,Unit,DataType")] Attribute attribute)
    {
        if (!ModelState.IsValid)
        {
            return View(attribute);
        }
        
        _context.Add(attribute);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Attributes/Edit/{id}
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var attribute = await _context.Attributes.FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == null);
        if (attribute == null)
        {
            return NotFound();
        }

        return View(attribute);
    }

    // POST: Attributes/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Name,PrettyName,Unit,DataType,CreatedAt,UpdatedAt,DeletedAt")]
        Attribute attribute)
    {
        if (id != attribute.Id || !ModelState.IsValid || !AttributeExists(id))
        {
            return View(attribute);
        }

        try
        {
            attribute.UpdatedAt = DateTimeOffset.UtcNow;
            _context.Update(attribute);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AttributeExists(attribute.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Attributes/Delete/{id}
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var attribute = await _context.Attributes.FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);
        if (attribute == null)
        {
            return NotFound();
        }

        return View(attribute);
    }

    // POST: Attributes/Delete/{id}
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var attribute = await _context.Attributes.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

        if (attribute == null)
        {
            return NotFound();
        }

        attribute.DeletedAt = DateTimeOffset.UtcNow;
        attribute.UpdatedAt = DateTimeOffset.UtcNow;
        _context.Update(attribute);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool AttributeExists(int id)
    {
        return _context.Attributes.Any(e => e.Id == id && e.DeletedAt == null);
    }
}