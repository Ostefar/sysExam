using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Data;
using TaskTrackerApi.Infrastructure;
using TaskTrackerApi.Models;

namespace TaskTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTrackerController : ControllerBase
    {
        private readonly TaskApiContext _context;
        private readonly IMessagePublisher _messagePublisher;


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
        public async Task<IActionResult> UpdateTask(int id, MyTask task) //ADD user ID
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
                //use massing to update user 
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

        //UpdateTaskStatus{ID}
        [HttpPut("UpdateTaskStatus/{id}")]
        public async Task<IActionResult> UpdateTaskStatus(MyTask task) //ADD user ID
        {
            // opdater selveste tasken med:
            // ny status
            // ny timestamp
            task.UpdatedAt = DateTime.UtcNow;

            var test = task.Status.ToString();

            //send message med userId og topic alt efter status

            if (test == "doing")
            {
                task.Status = MyTask.TaskStatus.completed;

            }

            if (task.Status.Equals("completed"))
            {
                Console.WriteLine("Complted task updated");
            }
            Console.WriteLine("Complted task updated");
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            //If userId ->
            // If task.status = todo -> 
            //Increment user's todo counter using messaging 

            // If task.status = doing -> 
            //Increment user's doing counter using messaging 

            // If task.status = done -> 
            //Increment user's done counter 

            // If task.status = thrown -> 
            //Increment user's thrown counter


            //_messagePublisher.PublishTaskStatusChangedMessage(
            //           id, "payed");
            /*
                // Add validation for UserId existence - needs to get the user first
            if (!_context.Users.Any(u => u.Id == task.UserId))
            {
                return BadRequest("Invalid UserId");
            }
            */



            return NoContent();
        }
     



        /*
           _messagePublisher.PublishOrderStatusChangedMessage(
                       order.CustomerId, order.OrderLines, "payed");
         */





        //Put update task status ToDo
        //Update timestamp for update 
        //Send message to update user on UserId to increment User's ToDo counter 

        //Put update task status Doing
        //Send message to update user on UserId to increment User's Doing counter

        //Put update task status Done
        //Send message to update user on UserId to increment User's Done counter

        //Put update task status Thrown
        //Send message to update user on UserId to increment User's Thrown counter



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
