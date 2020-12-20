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
    public class GroupPhonesController : ControllerBase
    {
        private readonly AddressBookContext _context;
        private readonly ManageGroupPhoneService _service;

        public GroupPhonesController(AddressBookContext context, ManageGroupPhoneService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/GroupPhones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupPhoneDto>>> GetGroupPhone(int userId)
        {
            var user = _context.GetUser(userId);
            List<GroupPhoneDto> item = user.GroupPhoneInternal.Select(u => new GroupPhoneDto() { Id = u.Id, UserId = u.UserId, Name = u.Name }).ToList();

            return item;
        }

        // GET: api/GroupPhones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupPhoneDto>> GetGroupPhone(int userId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var groupPhone = user.GroupPhoneInternal.Find(u => u.Id == id);
                if (groupPhone != null)
                {
                    GroupPhoneDto item = new GroupPhoneDto()
                    {
                        Id = groupPhone.Id,
                        UserId = groupPhone.UserId,
                        Name = groupPhone.Name
                    };

                    return item;
                }
                return NotFound();
            }
            return NotFound();
        }

        // PUT: api/GroupPhones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupPhone(int userId, int id, GroupPhoneDto groupPhoneDto)
        {
            if (id != groupPhoneDto.Id)
            {
                return BadRequest();
            }

            var user = _context.GetUser(userId);
            if (user != null)
            {
                var groupPhone = user.UpdateGroupPhone(id, groupPhoneDto.Name);
                if (groupPhone.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest(groupPhone.Errors);
            }
            return BadRequest("Group phone not found");
        }

        // POST: api/GroupPhones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GroupPhone>> PostGroupPhone(int userId, GroupPhoneDto groupPhone)
        {
            if (!_context.Users.Find(userId).Groups.Any(g => g.Name == groupPhone.Name))
            {
                groupPhone.UserId = userId;
                await _service.AddGroupPhoneAsync(groupPhone);
                return CreatedAtAction("GetGroupPhone", new { userId = groupPhone.UserId, id = groupPhone.Id }, groupPhone);
            }
            return Conflict();
        }

        // DELETE: api/GroupPhones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupPhone>> DeleteGroupPhone(int userId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var groupPhone = user.GroupPhoneInternal.Find(u => u.Id == id);
                if (groupPhone == null)
                {
                    return NotFound();
                }

                user.RemoveGroupPhone(groupPhone);
                await _context.SaveChangesAsync();
                return groupPhone;
            }
            return NotFound();
        }
    }
}
