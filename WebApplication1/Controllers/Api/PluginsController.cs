using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PluginsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PluginsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/plugins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plugin>>> GetPlugins()
        {
            return await _context.Plugins
                .Include(p => p.Manufacturer)
                .Include(p => p.Category)
                .ToListAsync();
        }

        // GET: api/plugins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plugin>> GetPlugin(int id)
        {
            var plugin = await _context.Plugins
                .Include(p => p.Manufacturer)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plugin == null)
            {
                return NotFound();
            }

            return plugin;
        }

        // POST: api/plugins
        [HttpPost]
        public async Task<ActionResult<Plugin>> CreatePlugin(Plugin plugin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Plugins.Add(plugin);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlugin), new { id = plugin.Id }, plugin);
        }

        // PUT: api/plugins/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlugin(int id, Plugin plugin)
        {
            if (id != plugin.Id)
            {
                return BadRequest();
            }

            _context.Entry(plugin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PluginExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/plugins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlugin(int id)
        {
            var plugin = await _context.Plugins.FindAsync(id);
            if (plugin == null)
            {
                return NotFound();
            }

            _context.Plugins.Remove(plugin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PluginExists(int id)
        {
            return _context.Plugins.Any(e => e.Id == id);
        }
    }
}
