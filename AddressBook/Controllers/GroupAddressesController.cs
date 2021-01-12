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
                    return new GroupAddressDto()
                    {
                        Id = groupAddress.Id,
                        UserId = groupAddress.UserId,
                        Name = groupAddress.Name
                    };
                }
                return BadRequest("User -> GroupAddress not found");
            }
            return BadRequest("User not found");
        }

        // PUT: api/GroupAddresses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroupAddress(int userId, int id, GroupAddressDto groupAddressDto)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var groupAddress = user.GroupAddresses.FirstOrDefault(ga => ga.Id == id);
                if (groupAddress != null)
                {
                    var answer = await _groupAddressService.UpdateGroupAddressAsync(user, id, groupAddressDto.Name);
                    if (answer.Succeeded)
                    {
                        return Ok();
                    }
                    return BadRequest(answer.Errors);
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
                var answer = await _groupAddressService.AddGroupAddressAsync(user, (string)groupAddressDto.Name);
                if (answer.Succeeded)
                {
                    var answerDto = _groupAddressService.GetGroupAddress(user, (string)groupAddressDto.Name);
                    if (answerDto.Succeeded)
                    {
                        groupAddressDto = answerDto.Value;
                        return CreatedAtAction("GetGroupAddress", new { userId = groupAddressDto.UserId, id = groupAddressDto.Id }, groupAddressDto);
                    }
                    return BadRequest(answerDto.Errors);
                }
                return BadRequest(answer.Errors);
            }
            return BadRequest("User not found");
        }

        // DELETE: api/GroupAddresses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupAddressDto>> DeleteGroupAddress(int userId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var groupAddress = user.GroupAddresses.FirstOrDefault(u => u.Id == id);
                if (groupAddress != null)
                {
                    GroupAddressDto dto = new GroupAddressDto()
                    {
                        Id = groupAddress.Id,
                        UserId = groupAddress.UserId,
                        Name = groupAddress.Name
                    };
                    await _groupAddressService.DeleteGroupAddressAsync(user, groupAddress);
                    return dto;
                }
                return BadRequest("User -> GroupPhone not found");
            }
            return BadRequest("User not found");
        }
    }
}
