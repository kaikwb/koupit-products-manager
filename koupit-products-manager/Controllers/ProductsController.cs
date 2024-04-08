using koupit_products_manager.Models;
using koupit_products_manager.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Attribute = koupit_products_manager.Models.Attribute;

namespace koupit_products_manager.Controllers;

public class ProductsController(PostgresDbContext context) : Controller
{
    // GET: Products
    public async Task<IActionResult> Index()
    {
        var products = await GetProducts();

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
    public async Task<IActionResult> Create()
    {
        ViewBag.Manufacturers = await GetManufacturers();
        ViewBag.Attributes = await GetAttributes();

        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,ManufacturerId,PartNumber,Description")] Product product)
    {
        product.Manufacturer = (await GetManufacturer(product.ManufacturerId))!;

        ModelState.Clear();
        TryValidateModel(product);

        if (ModelState.IsValid)
        {
            product.CreatedAt = DateTimeOffset.UtcNow;
            context.Add(product);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Manufacturers = GetManufacturers();

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
        if (product is not { DeletedAt: null })
        {
            return NotFound();
        }

        ViewBag.Manufacturers = GetManufacturers();

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
        
        product.Manufacturer = (await GetManufacturer(product.ManufacturerId))!;

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

        ViewBag.Manufacturers = GetManufacturers();

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

    private async Task<List<Manufacturer>> GetManufacturers()
    {
        var manufacturers = await context.Manufacturers
            .Where(m => m.DeletedAt == null)
            .ToListAsync();
        manufacturers.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.InvariantCultureIgnoreCase));

        return manufacturers;
    }
    
    private async Task<Manufacturer?> GetManufacturer(int id)
    {
        return await context.Manufacturers
            .FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);
    }
    
    private async Task<List<Attribute>> GetAttributes()
    {
        return await context.Attributes
            .Where(a => a.DeletedAt == null)
            .ToListAsync();
    }
    
    private async Task<List<Product>> GetProducts()
    {
        var products = await context.Products
            .Where(p => p.DeletedAt == null)
            .ToListAsync();
        products.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.InvariantCultureIgnoreCase));

        return products;
    }
}