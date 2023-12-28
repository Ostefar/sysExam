namespace UserApi.Models
{
    public class MyUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }   
        public string Phone { get; set; }
        public int TasksToDo { get; set; } //update by messaging
        public int TasksDoing { get; set; } //update by messaging
        public int TasksDone { get; set; } // update by messaging
        public int TasksThrown { get; set; } // update by messaging from TaskTrackerController gg


    }
}
