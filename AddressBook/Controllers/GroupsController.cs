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
                    return new GroupDto()
                    {
                        Id = group.Id,
                        UserId = group.UserId,
                        Name = group.Name
                    };
                }
                return BadRequest("User -> Group not found");
            }
            return BadRequest("User not found");
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int userId, int id, GroupDto groupDto)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var group = user.Groups.FirstOrDefault(g => g.Id == id);
                if (group != null)
                {
                    var answer = await _groupService.UpdateGroupAsync(user, id, groupDto.Name);
                    if (answer.Succeeded)
                    {
                        return Ok();
                    }
                    return BadRequest(answer.Errors);
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
                var answer = await _groupService.AddGroupAsync(user, groupDto.Name);
                if (answer.Succeeded)
                {
                    var answerDto = _groupService.GetGroup(user, groupDto.Name);
                    if (answerDto.Succeeded)
                    {
                        groupDto = answerDto.Value;
                        return CreatedAtAction("GetGroup", new { userId = userId, id = groupDto.Id }, groupDto);
                    }
                    return BadRequest(answerDto.Errors);
                }
                return BadRequest(answer.Errors);
            }
            return BadRequest("User not found");
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupDto>> DeleteGroup(int userId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var group = user.Groups.FirstOrDefault(u => u.Id == id);
                if (group != null)
                {
                    await _groupService.DeleteGroupAsync(user, group);

                    return new GroupDto() { Id = group.Id, UserId = group.UserId, Name = group.Name };
                }
                return BadRequest("User -> Group not found");
            }
            return BadRequest("User not found");
        }
    }
}
