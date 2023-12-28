using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels
{
    public class MyUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int TasksToDo { get; set; }
        public int TasksDoing { get; set; }
        public int TasksDone { get; set; }
        public int TasksThrown { get; set; }

    }
}
