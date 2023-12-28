using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels
{
    public class MyTaskDto
    {
        [SwaggerSchema(Description = "Id of the task")]
        public int Id { get; set; }

        [SwaggerSchema(Description = "The title of the task")]
        public string Title { get; set; }

        [SwaggerSchema(Description = "Description of the task")]
        public string Description { get; set; }

        [SwaggerSchema(Description = "Id of the user assigned to the task")]
        public int UserId { get; set; }

        [SwaggerSchema(Description = "The current status of the task")]
        public TaskStatus Status { get; set; }

        [SwaggerSchema(Description = "The date the task must be finished")]
        public DateTime DueDate { get; set; }

        [SwaggerSchema(Description = "Task created at this date")]
        public DateTime CreatedAt { get; set; }

        [SwaggerSchema(Description = "Task last updated at this date")]
        public DateTime UpdatedAt { get; set; }

        [SwaggerSchema(Description = "Enum of the TaskStatus 0=todo, 1=doing, 2=completed, 3=cancelled")]
        public enum TaskStatus
        {
            todo,
            doing,
            completed,
            cancelled
        }
    }
}
