using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using Swashbuckle.AspNetCore.Annotations;
using TaskTrackerApi.Models;
using UserApi.Data;
using UserApi.Models;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<MyUser> repository;
        private readonly IConverter<MyUser, MyUserDto> userConverter;
        private readonly UserApiContext _context;

        public UserController(IRepository<MyUser> repos, IConverter<MyUser, MyUserDto> converter, UserApiContext context)
        {
            repository = repos;
            userConverter = converter;
            _context = context;
        }

        [HttpGet(Name = "GetUsers")]
        /*[SwaggerOperation(Summary = "Gets a list of all users")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successfull", typeof(IEnumerable<MyUserDto>))]*/
        public async Task<IEnumerable<MyUserDto>> GetUsers()
        {
            var userDtoList = new List<MyUserDto>();
            foreach (var user in await repository.GetAllAsync())
            {
                var userDto = userConverter.Convert(user);
                userDtoList.Add(userDto);
            }
            return userDtoList;
        }

        [HttpGet("{id}", Name = "GetUser")]
        [SwaggerOperation(Summary = "Gets a specific user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successfull")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await repository.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = userConverter.Convert(user);
            return new ObjectResult(userDto);
        }

        [HttpPost(Name = "CreateUser")]
        [SwaggerOperation(Summary = "Create a new user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successfull")]
        public async Task<IActionResult> PostAsync([FromBody] MyUserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }

            var user = userConverter.Convert(userDto);
            var newUser = await repository.AddAsync(user);

            return CreatedAtRoute("GetUser", new { id = newUser.Id },
                userConverter.Convert(newUser));
        }

        [HttpPut("{id}", Name = "UpdateUser")]
        [SwaggerOperation(Summary = "Update an existing user")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Request Successfull")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] MyUserDto userDto)
        {
            if (userDto == null || userDto.Id != id)
            {
                return BadRequest();
            }

            var modifiedUser = await repository.GetAsync(id);

            if (modifiedUser == null)
            {
                return NotFound();
            }
            modifiedUser.FirstName = userDto.FirstName;
            modifiedUser.LastName = userDto.LastName;
            modifiedUser.UserName = userDto.UserName;
            modifiedUser.Password = userDto.Password;
            modifiedUser.Email = userDto.Email;
            modifiedUser.Phone = userDto.Phone;


            await repository.EditAsync(modifiedUser);
            return new NoContentResult();
        }

        [HttpDelete("{id}", Name = "DeleteUser")]
        [SwaggerOperation(Summary = "Delete an existing user")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Request Successfull")]
        public async Task<IActionResult> DeleteAsync(int id)
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


