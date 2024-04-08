using koupit_products_manager.Models;
using koupit_products_manager.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace koupit_products_manager.Controllers;

public class ProductsController(PostgresDbContext context) : Controller
{
    // GET: Products
    public async Task<IActionResult> Index()
    {
        var products = await context.Products
            .Where(p => p.DeletedAt == null)
            .ToListAsync();
        products.Sort((a, b) => a.Id.CompareTo(b.Id));

        return View(products);
    }

    // GET: Products/Details/{id}
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Products
            .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Products/Create
    public IActionResult Create()
    {
        var manufacturers = context.Manufacturers
            .Where(m => m.DeletedAt == null)
            .ToList();
        manufacturers.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.InvariantCultureIgnoreCase));

        ViewBag.Manufacturers = manufacturers;

        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,ManufacturerId,PartNumber,Description")] Product product)
    {
        var manufacturer = await context.Manufacturers
            .FirstOrDefaultAsync(m => m.Id == product.ManufacturerId && m.DeletedAt == null);
        product.Manufacturer = manufacturer!;

        ModelState.Clear();
        TryValidateModel(product);

        if (ModelState.IsValid)
        {
            product.CreatedAt = DateTimeOffset.UtcNow;
            context.Add(product);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Manufacturers = context.Manufacturers
            .Where(m => m.DeletedAt == null)
            .ToList();

        return View(product);
    }

    // GET: Products/Edit/{id}
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Products.FindAsync(id);
        if (product == null || product.DeletedAt != null)
        {
            return NotFound();
        }

        ViewBag.Manufacturers = context.Manufacturers
            .Where(m => m.DeletedAt == null)
            .ToList();

        return View(product);
    }

    // POST: Products/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Uuid,Name,ManufacturerId,PartNumber,Description,CreatedAt,UpdatedAt,DeletedAt")]
        Product product)
    {
        if (id != product.Id || !ProductExists(id))
        {
            return NotFound();
        }

        var manufacturer = await context.Manufacturers
            .FirstOrDefaultAsync(m => m.Id == product.ManufacturerId && m.DeletedAt == null);
        product.Manufacturer = manufacturer!;

        ModelState.Clear();
        TryValidateModel(product);

        if (ModelState.IsValid)
        {
            try
            {
                product.UpdatedAt = DateTimeOffset.UtcNow;
                context.Update(product);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Manufacturers = context.Manufacturers
            .Where(m => m.DeletedAt == null)
            .ToList();

        return View(product);
    }

    // GET: Products/Delete/{id}
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Products.FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Products/Delete/{id}
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);

        if (product == null)
        {
            return NotFound();
        }

        product.UpdatedAt = DateTimeOffset.UtcNow;
        product.DeletedAt = DateTimeOffset.UtcNow;

        context.Update(product);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return context.Products.Any(e => e.Id == id && e.DeletedAt == null);
    }
}