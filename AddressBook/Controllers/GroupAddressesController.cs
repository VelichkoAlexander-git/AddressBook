using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AddressBook;
using AddressBook.Models;
using AddressBook.BL;
using AddressBook.DTO;

namespace AddressBook.Controllers
{
    [Route("api/Users/{userId:int}/[controller]")]
    [ApiController]
    public class GroupAddressesController : ControllerBase
    {
        private readonly ManageUsersService _userService;
        private readonly ManageGroupAddressService _groupAddressService;

        public GroupAddressesController(ManageUsersService userService, ManageGroupAddressService service)
        {
            _userService = userService;
            _groupAddressService = service;
        }

        // GET: api/GroupAddresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupAddressDto>>> GetGroupAddress(int userId)
        {
            var user = _userService.GetUser(userId);
            List<GroupAddressDto> item = user.GroupAddresses.Select(u => new GroupAddressDto() { Id = u.Id, UserId = u.UserId, Name = u.Name }).ToList();

            return item;
        }

        // GET: api/GroupAddresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupAddressDto>> GetGroupAddress(int userId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var groupAddress = user.GroupAddresses.FirstOrDefault(u => u.Id == id);
                if (groupAddress != null)
                {
                    GroupAddressDto item = new GroupAddressDto()
                    {
                        Id = groupAddress.Id,
                        UserId = groupAddress.UserId,
                        Name = groupAddress.Name
                    };

                    return item;
                }
                return NotFound();
            }
            return NotFound();
        }

        // PUT: api/GroupAddresses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroupAddress(int userId, int id, GroupAddressDto groupAddressDto)
        {
            if (id != groupAddressDto.Id)
            {
                return BadRequest();
            }

            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var groupAddress = user.GroupAddresses.FirstOrDefault(ga => ga.Id == id);
                if (groupAddress != null)
                {
                    if (!user.GroupAddresses.Any(g => g.Name == groupAddressDto.Name))
                    {
                        await _groupAddressService.UpdateGroupAddressAsync(user, id, groupAddressDto.Name);
                        return Ok();
                    }
                    return Conflict();
                }
                return BadRequest("User -> GroupAddress not found");
            }
            return BadRequest("User not found");
        }

        // POST: api/GroupAddresses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GroupAddress>> AddGroupAddress(int userId, GroupAddressDto groupAddressDto)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                if (!user.GroupAddresses.Any(g => g.Name == groupAddressDto.Name))
                {
                    await _groupAddressService.AddGroupAddressAsync(user, groupAddressDto.Name);
                    return CreatedAtAction("GetGroupAddress", new { userId = groupAddressDto.UserId, id = groupAddressDto.Id }, groupAddressDto);
                }
                return Conflict();
            }
            return BadRequest("User not found");
        }

        // DELETE: api/GroupAddresses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupAddress>> DeleteGroupAddress(int userId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var groupAddress = user.GroupAddresses.FirstOrDefault(u => u.Id == id);
                if (groupAddress != null)
                {
                    await _groupAddressService.DeleteGroupAddressAsync(user, groupAddress);
                    return groupAddress;
                }
                return BadRequest("User -> GroupPhone not found");
            }
            return BadRequest("User not found");
        }
    }
}
