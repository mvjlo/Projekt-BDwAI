using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PluginsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PluginsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString, int? manufacturerId, int? categoryId)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["ManufacturerId"] = manufacturerId;
            ViewData["CategoryId"] = categoryId;

            ViewData["Manufacturers"] = new SelectList(await _context.Manufacturers.ToListAsync(), "Id", "Name");
            ViewData["Categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");

            var plugins = _context.Plugins
                .Include(p => p.Manufacturer)
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                plugins = plugins.Where(p => p.Name.Contains(searchString) || 
                                           (p.Description != null && p.Description.Contains(searchString)));
            }

            if (manufacturerId.HasValue)
            {
                plugins = plugins.Where(p => p.ManufacturerId == manufacturerId.Value);
            }

            if (categoryId.HasValue)
            {
                plugins = plugins.Where(p => p.CategoryId == categoryId.Value);
            }

            return View(await plugins.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plugin = await _context.Plugins
                .Include(p => p.Manufacturer)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plugin == null)
            {
                return NotFound();
            }

            return View(plugin);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Name,Version,Price,SystemRequirements,Description,ManufacturerId,CategoryId")] Plugin plugin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plugin);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Wtyczka została dodana pomyślnie.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name", plugin.ManufacturerId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", plugin.CategoryId);
            return View(plugin);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plugin = await _context.Plugins.FindAsync(id);
            if (plugin == null)
            {
                return NotFound();
            }
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name", plugin.ManufacturerId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", plugin.CategoryId);
            return View(plugin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Version,Price,SystemRequirements,Description,ManufacturerId,CategoryId")] Plugin plugin)
        {
            if (id != plugin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plugin);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Wtyczka została zaktualizowana pomyślnie.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PluginExists(plugin.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name", plugin.ManufacturerId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", plugin.CategoryId);
            return View(plugin);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plugin = await _context.Plugins
                .Include(p => p.Manufacturer)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plugin == null)
            {
                return NotFound();
            }

            return View(plugin);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plugin = await _context.Plugins.FindAsync(id);
            if (plugin != null)
            {
                _context.Plugins.Remove(plugin);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Wtyczka została usunięta pomyślnie.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PluginExists(int id)
        {
            return _context.Plugins.Any(e => e.Id == id);
        }
    }
}
