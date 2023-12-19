using System;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<MyTask> GetTasks()
        {
            return _context.Tasks.ToList();
        }

        // GET: api/TaskTracker/5
        [HttpGet("{id}")]
        public ActionResult<MyTask> GetTask(int id)
        {
            var task = _context.Tasks.Find(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // POST: api/TaskTracker
        [HttpPost]
        public ActionResult<MyTask> CreateTask(MyTask task)
        {
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }

        // PUT: api/TaskTracker/5
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, MyTask task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            task.UpdatedAt = DateTime.UtcNow;
            _context.Entry(task).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
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
        public ActionResult<MyTask> DeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return task;
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
