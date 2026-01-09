using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class LicensesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LicensesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var licenses = await _context.Licenses
                .Include(l => l.Plugin)
                    .ThenInclude(p => p!.Manufacturer)
                .Include(l => l.Plugin)
                    .ThenInclude(p => p!.Category)
                .Where(l => l.UserId == userId)
                .ToListAsync();

            return View(licenses);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var license = await _context.Licenses
                .Include(l => l.Plugin)
                    .ThenInclude(p => p!.Manufacturer)
                .Include(l => l.Plugin)
                    .ThenInclude(p => p!.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (license == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (license.UserId != userId && !User.IsInRole("Administrator"))
            {
                return Forbid();
            }

            return View(license);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["PluginId"] = new SelectList(
                await _context.Plugins.Include(p => p.Manufacturer).ToListAsync(), 
                "Id", 
                "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LicenseCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var license = new License
                {
                    PluginId = model.PluginId,
                    UserId = userId!,
                    SerialKey = model.SerialKey,
                    PurchaseDate = model.PurchaseDate,
                    ExpirationDate = model.ExpirationDate
                };

                _context.Add(license);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Licencja została dodana pomyślnie.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["PluginId"] = new SelectList(
                await _context.Plugins.Include(p => p.Manufacturer).ToListAsync(), 
                "Id", 
                "Name", 
                model.PluginId);
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var license = await _context.Licenses
                .Include(l => l.Plugin)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (license == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (license.UserId != userId && !User.IsInRole("Administrator"))
            {
                return Forbid();
            }

            return View(license);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var license = await _context.Licenses.FindAsync(id);
            if (license != null)
            {
                var userId = _userManager.GetUserId(User);
                if (license.UserId != userId && !User.IsInRole("Administrator"))
                {
                    return Forbid();
                }

                _context.Licenses.Remove(license);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Licencja została usunięta pomyślnie.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
