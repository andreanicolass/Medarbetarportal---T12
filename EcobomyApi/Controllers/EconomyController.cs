using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcobomyApi.Data;
using EcobomyApi.Models;

namespace EcobomyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EconomyController : ControllerBase
    {
        private readonly EconomyDbContext _context;

        public EconomyController(EconomyDbContext context)
        {
            _context = context;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.EconomyRecords.ToListAsync();
            return Ok(data);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.EconomyRecords.FindAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST (CREATE)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EconomyRecord record)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.EconomyRecords.AddAsync(record);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }

        // PUT (UPDATE)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EconomyRecord updated)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _context.EconomyRecords.FindAsync(id);
            if (existing == null)
                return NotFound();

            // Uppdatera fält (ändra efter din modell)
            
            existing.ItemName = updated.ItemName;
            existing.EmployeeName = updated.EmployeeName;
            existing.Cost = updated.Cost;
            existing.LoanDate = updated.LoanDate;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.EconomyRecords.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.EconomyRecords.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // STATISTICS
        [HttpGet("statistics")]
        public async Task<IActionResult> Stats()
        {
            var totalCost = await _context.EconomyRecords
                .Select(x => x.Cost)
                .DefaultIfEmpty(0)
                .SumAsync();

            var count = await _context.EconomyRecords.CountAsync();

            return Ok(new
            {
                totalCost,
                count
            });
        }

        // FILTER BY ITEM NAME
        [HttpGet("by-item/{name}")]
        public async Task<IActionResult> GetByItem(string name)
        {
            var result = await _context.EconomyRecords
                .Where(x => x.ItemName.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            return Ok(result);
        }
    }
}