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
        private readonly AddressBookContext _context;
        private readonly ManageGroupService _service;

        public GroupsController(AddressBookContext context, ManageGroupService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroup(int userId)
        {
            var user = _context.GetUser(userId);
            List<GroupDto> item = user.GroupInternal.Select(u => new GroupDto() { Id = u.Id, UserId = u.UserId, Name = u.Name }).ToList();

            return item;
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDto>> GetGroup(int userId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var group = user.GroupInternal.Find(u => u.Id == id);
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
        public async Task<IActionResult> PutGroup(int userId, int id, GroupDto groupDto)
        {
            if (id != groupDto.Id)
            {
                return BadRequest();
            }

            var user = _context.GetUser(userId);
            if (user != null)
            {
                var group = user.UpdateGroup(id, groupDto.Name);
                if (group.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest(group.Errors);
            }
            return BadRequest("User not found");
        }

        // POST: api/Groups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup(int userId, GroupDto group)
        {
            if (!_context.Users.Find(userId).Groups.Any(g => g.Name == group.Name))
            {
                group.UserId = userId;
                await _service.AddGroupAsync(group);
                return CreatedAtAction("GetGroup", new { userId = group.UserId, id = group.Id }, group);
            }
            return Conflict();
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Group>> DeleteGroup(int userId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var group = user.GroupInternal.Find(u => u.Id == id);
                if (group == null)
                {
                    return NotFound();
                }

                user.RemoveGroup(group);
                await _context.SaveChangesAsync();               
                return group;
            }
            return NotFound();
        }
    }
}
