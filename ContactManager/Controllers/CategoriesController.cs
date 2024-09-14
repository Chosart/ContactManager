using ContactManager.Data;
using ContactManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContactManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ContactDbContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ContactDbContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory([FromBody] Category category)
        {
            if (category == null || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid category data.");
                return BadRequest(ModelState);
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id,  Category category)
        {
            if (id != category.Id)
            {
                return BadRequest("ID in URL does not match ID in the request body.");
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = category.Name;

            _context.Entry(existingCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if(category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occured while deleting the category.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An errr occured while deleting the category.");
            }

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
