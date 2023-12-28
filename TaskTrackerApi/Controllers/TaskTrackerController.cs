﻿using System;
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

            await repository.EditAsync(modifiedTask);
            return new NoContentResult();
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await repository.GetAsync(id) == null)
            {
                return NotFound();
            }

            await repository.RemoveAsync(id);
            return new NoContentResult();
        }
    }
}
