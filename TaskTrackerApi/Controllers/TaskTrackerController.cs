using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Data;
using TaskTrackerApi.Models;

namespace TaskTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTrackerController : ControllerBase
    {
        private readonly TaskApiContext _context;

        public TaskTrackerController(TaskApiContext context)
        {
            _context = context;
        }

        // GET: api/TaskTracker
        [HttpGet]
        public async Task<IEnumerable<MyTask>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/TaskTracker/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MyTask>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // POST: api/TaskTracker
        [HttpPost]
        public async Task<ActionResult<MyTask>> CreateTask(MyTask task)
        {
            /*
            // Add validation for UserId existence - needs to get the user first
            if (!_context.Users.Any(u => u.Id == task.UserId))
            {
                return BadRequest("Invalid UserId");
            }
            */
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }

        // PUT: api/TaskTracker/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, MyTask task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }
            /*
                // Add validation for UserId existence - needs to get the user first
            if (!_context.Users.Any(u => u.Id == task.UserId))
            {
                return BadRequest("Invalid UserId");
            }
            */

            task.UpdatedAt = DateTime.UtcNow;
            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // DELETE: api/TaskTracker/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MyTask>> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return task;
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
