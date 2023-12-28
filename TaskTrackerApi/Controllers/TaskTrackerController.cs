using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using TaskTrackerApi.Data;
using TaskTrackerApi.Infrastructure;
using TaskTrackerApi.Models;
using static TaskTrackerApi.Models.TaskConverter;

namespace TaskTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTrackerController : ControllerBase
    {
        private readonly TaskApiContext _context;
        private readonly IRepository<MyTask> repository;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IConverter<MyTask, MyTaskDto> taskConverter;


        public TaskTrackerController(IRepository<MyTask> repos, TaskApiContext context, IMessagePublisher messagePublisher, IConverter<MyTask, MyTaskDto> converter)
        {
            repository = repos;
            _context = context;
            _messagePublisher = messagePublisher;
            taskConverter = converter;

        }

        [HttpGet]
        public async Task<IEnumerable<MyTaskDto>> GetTasks()
        {
            var taskDtoList = new List<MyTaskDto>();
            foreach (var task in await repository.GetAllAsync())
            {
                var taskDto = taskConverter.Convert(task);
                taskDtoList.Add(taskDto);
            }
            return taskDtoList;
        }

        [HttpGet("{id}", Name = "GetTask")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await repository.GetAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            var taskDto = taskConverter.Convert(task);
            return new ObjectResult(taskDto);
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] MyTaskDto taskDto)
        {
            if (taskDto == null)
            {
                return BadRequest();
            }

            var task = taskConverter.Convert(taskDto);
            task.Status = 0; //todo also the starting point
            task.DueDate = DateTime.Now.AddDays(45);
            task.CreatedAt = DateTime.Now;
            task.UpdatedAt = DateTime.Now;

            var newTask = await repository.AddAsync(task);

            _messagePublisher.PublishTaskStatusChangedMessage(
                   task.UserId, task.Status.ToString(), "todo");

            return CreatedAtRoute("GetTask", new { id = newTask.Id },
                taskConverter.Convert(newTask));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] MyTaskDto taskDto)
        {
            if (taskDto == null || taskDto.Id != id)
            {
                return BadRequest();
            }

            var modifiedTask = await repository.GetAsync(id);

            if (modifiedTask == null)
            {
                return NotFound();
            }

            modifiedTask.Title = taskDto.Title;
            modifiedTask.Description = taskDto.Description;
            modifiedTask.UserId = taskDto.UserId;
            modifiedTask.Status = (MyTask.TaskStatus)taskDto.Status;
            modifiedTask.DueDate = taskDto.DueDate;
            modifiedTask.UpdatedAt = DateTime.Now;

            _messagePublisher.PublishTaskStatusChangedMessage(
                  modifiedTask.UserId, modifiedTask.Status.ToString(), "thrown");

            await repository.EditAsync(modifiedTask);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await repository.GetAsync(id);
            var currentStatus = task.Status.ToString();
            if (await repository.GetAsync(id) == null)
            {
                return NotFound();
            }

            _messagePublisher.PublishTaskStatusChangedMessage(
                   task.UserId, currentStatus, "thrown");

            await repository.RemoveAsync(id);
            return new NoContentResult();
        }
/*
        private void UpdateUserTasks(string status, int userId)
        {
            if (status is not null)
            {
                if (status == "todo")
                {
                    _messagePublisher.PublishTaskStatusChangedMessage(
                   userId, "todo");
                }
                else if (status == "doing")
                {
                    _messagePublisher.PublishTaskStatusChangedMessage(
                   userId, "doing");
                }
                else if (status == "done")
                {
                    _messagePublisher.PublishTaskStatusChangedMessage(
                  userId, "done");
                }
                else if (status == "thrown")
                {
                    _messagePublisher.PublishTaskStatusChangedMessage(
                   userId, "thrown");
                }
            }
            else
            {
                Console.WriteLine("status is empty");
            }
        }*/
    }
}
