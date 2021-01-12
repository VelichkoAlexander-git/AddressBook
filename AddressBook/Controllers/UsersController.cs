using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AddressBook;
using AddressBook.BL;
using AddressBook.DTO;
using AddressBook.Models;

namespace AddressBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AddressBookContext _context;
        private readonly ManageUsersService _usersService;

        public UsersController(AddressBookContext context, ManageUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            List<UserDto> item = _context.Users.Select(u => new UserDto() { Id = u.Id, Login = u.Login, Password = u.Password }).ToList();
            return item;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = _usersService.GetUser(id);
            if (user != null)
            {
                return new UserDto()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Password = user.Password
                };
            }
            return BadRequest("User not found");
        }

        // PUT: api/Users/5
        // To protect from overing attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            var user = _usersService.GetUser(id);
            if (user != null)
            {
                var answer = await _usersService.UpdateUserAsync(user, userDto.Login, userDto.Password);
                if (answer.Succeeded)
                {
                    return Ok();
                }
                return BadRequest(answer.Errors);
            }
            return BadRequest("User not found");
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(UserDto user)
        {
            var answer = await _usersService.AddUserAsync(user.Login, user.Password);
            if (answer.Succeeded)
            {
                var answerDto = _usersService.GetUser(user.Login, user.Password);
                if (answerDto.Succeeded)
                {
                    user = answerDto.Value;
                    return CreatedAtAction("GetUser", new { id = user.Id }, user);
                }
                return BadRequest(answerDto.Errors);
            }
            return BadRequest(answer.Errors);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDto>> DeleteUser(int id)
        {
            var user = _usersService.GetUser(id);
            if (user != null)
            {
                await _usersService.DeleteUserAsync(user);
                return new UserDto() { Id = user.Id, Login = user.Login, Password = user.Password };
            }
            return BadRequest("User not found");
        }
    }
}
