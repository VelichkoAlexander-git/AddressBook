﻿using System;
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
            var user = _context.GetUser(id);
            if (user != null)
            {
                UserDto item = new UserDto()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Password = user.Password
                };

                return item;
            }
            return NotFound();
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest();
            }

            var user = _context.UpdateUser(id, userDto.Login, userDto.Password);
            if (user.Succeeded)
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(user.Errors);
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDto user)
        {
            if (!_context.Users.Any(u => u.Login == user.Login))
            {
                var error = await _usersService.AddUserAsync(user);
                if (!error.Succeeded)
                {
                    BadRequest(error);
                }

                return CreatedAtAction("GetUser", new { id = _context.Users.FirstAsync(u => u.Login == user.Login).Id }, user);
            }
            return Conflict();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = _context.GetUser(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return user;
            }
            return NotFound();
        }
    }
}
