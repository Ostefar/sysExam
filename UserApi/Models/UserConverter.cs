using SharedModels;
using UserApi.Models;

namespace TaskTrackerApi.Models
{

    public class UserConverter : IConverter<MyUser, MyUserDto>
    {
        public MyUser Convert(MyUserDto sharedUser)
        {
            return new MyUser
            {
                Id = sharedUser.Id,
                FirstName = sharedUser.FirstName,
                LastName = sharedUser.LastName,
                UserName = sharedUser.UserName,
                Password = sharedUser.Password,
                Email = sharedUser.Email,
                Phone = sharedUser.Phone,
                TasksMoved = sharedUser.TasksMoved
            };
        }

        public MyUserDto Convert(MyUser hiddenUser)
        {
            return new MyUserDto
            {
                Id = hiddenUser.Id,
                FirstName = hiddenUser.FirstName,
                LastName = hiddenUser.LastName,
                UserName = hiddenUser.UserName,
                Password = hiddenUser.Password,
                Email = hiddenUser.Email,
                Phone = hiddenUser.Phone,
                TasksMoved = hiddenUser.TasksMoved
            };
        }
    }
}

