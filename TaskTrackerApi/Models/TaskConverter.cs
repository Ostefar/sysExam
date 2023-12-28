using SharedModels;

namespace TaskTrackerApi.Models
{
    public class TaskConverter
    {
        public class ProductConverter : IConverter<MyTask, MyTaskDto>
        {
            public MyTask Convert(MyTaskDto sharedTask)
            {
                return new MyTask
                {
                    Id = sharedTask.Id,
                    Title = sharedTask.Title,
                    Description = sharedTask.Description,
                    UserId = sharedTask.UserId,
                    Status = (MyTask.TaskStatus)sharedTask.Status,
                    DueDate = sharedTask.DueDate,
                    CreatedAt = sharedTask.CreatedAt,
                    UpdatedAt = sharedTask.UpdatedAt
                };
            }

            public MyTaskDto Convert(MyTask hiddenTask)
            {
                return new MyTaskDto
                {
                    Id = hiddenTask.Id,
                    Title = hiddenTask.Title,
                    Description = hiddenTask.Description,
                    UserId = hiddenTask.UserId,
                    Status = (MyTaskDto.TaskStatus)hiddenTask.Status,
                    DueDate = hiddenTask.DueDate,
                    CreatedAt = hiddenTask.CreatedAt,
                    UpdatedAt = hiddenTask.UpdatedAt
                };
            }
        }
    }
}
