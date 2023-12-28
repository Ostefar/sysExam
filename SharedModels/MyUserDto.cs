using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels
{
    public class MyUserDto
    {
        [SwaggerSchema(Description = "Id of the user")]
        public int Id { get; set; }

        [SwaggerSchema(Description = "The users firstname")]
        public string FirstName { get; set; }

        [SwaggerSchema(Description = "The users lastname")]
        public string LastName { get; set; }

        [SwaggerSchema(Description = "The users username")]
        public string UserName { get; set; }

        [SwaggerSchema(Description = "The users password")]
        public string Password { get; set; }

        [SwaggerSchema(Description = "The users email")]
        public string Email { get; set; }

        [SwaggerSchema(Description = "The users phone number")]
        public string Phone { get; set; }

        [SwaggerSchema(Description = "Amount of tasks moved by the user")]
        public int TasksMoved { get; set; }

    }
}
