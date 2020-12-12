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
        public async Task<ActionResult<IEnumerable<Group>>> GetGroup(int userId)
        {
            return await _context.Group.ToListAsync();
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int userId, int id)
        {
            var user = _context.GetUser(userId);
            var item = user.GroupInternal.Find(u => u.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int userId, int id, Group @group)
        {
            if (id != @group.Id)
            {
                return BadRequest();
            }

            _context.Entry(@group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(userId,id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Groups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup(int userId, GroupDto item)
        {
            item.UserId = userId;
            await _service.AddGroupAsync(item);

            return CreatedAtAction("GetGroup", new { userId = item.UserId, id = item.Id }, item);
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Group>> DeleteGroup(int userId, int id)
        {
            var user = _context.GetUser(userId);

            if (user == null)
            {
                return NotFound();
            }

            var item = user.RemoveGroup(id);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", item.Succeeded);
        }

        private bool GroupExists(int userId, int id)
        {
            return _context.Group.Any(e => e.Id == id);
        }
    }
}
