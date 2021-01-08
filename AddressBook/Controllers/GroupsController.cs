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
    public class GroupsController : ControllerBase
    {
        private readonly ManageUsersService _userService;
        private readonly ManageGroupService _groupService;

        public GroupsController(ManageUsersService userService, ManageGroupService groupService)
        {
            _userService = userService;
            _groupService = groupService;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroup(int userId)
        {
            var user = _userService.GetUser(userId);
            List<GroupDto> item = user.Groups.Select(u => new GroupDto() { Id = u.Id, UserId = u.UserId, Name = u.Name }).ToList();

            return item;
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDto>> GetGroup(int userId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var group = user.Groups.FirstOrDefault(u => u.Id == id);
                if (group != null)
                {
                    GroupDto item = new GroupDto()
                    {
                        Id = group.Id,
                        UserId = group.UserId,
                        Name = group.Name
                    };

                    return item;
                }
                return NotFound();
            }
            return NotFound();
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int userId, int id, GroupDto groupDto)
        {
            if (id != groupDto.Id)
            {
                return BadRequest();
            }

            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var group = user.Groups.FirstOrDefault(g => g.Id == id);
                if (group != null)
                {
                    if (!user.Groups.Any(g => g.Name == groupDto.Name))
                    {
                        await _groupService.UpdateGroupAsync(user, id, groupDto.Name);
                        return Ok();
                    }
                    return Conflict();
                }
                return BadRequest("User -> Group not found");
            }
            return BadRequest("User not found");
        }

        // POST: api/Groups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Group>> AddGroup(int userId, GroupDto groupDto)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                if (!user.Groups.Any(g => g.Name == groupDto.Name))
                {
                    await _groupService.AddGroupAsync(user, groupDto.Name);
                    return CreatedAtAction("GetGroup", new { userId = userId, id = groupDto.Id }, groupDto);
                }
                return Conflict();
            }
            return BadRequest("User not found");
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Group>> DeleteGroup(int userId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var group = user.Groups.FirstOrDefault(u => u.Id == id);
                if (group != null)
                {
                    await _groupService.DeleteGroupAsync(user, group);

                    return group;
                }
                return BadRequest("User -> Group not found");
            }
            return BadRequest("User not found");
        }
    }
}
