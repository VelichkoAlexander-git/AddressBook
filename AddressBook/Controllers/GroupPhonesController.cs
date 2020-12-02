using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AddressBook;
using AddressBook.Models;

namespace AddressBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupPhonesController : ControllerBase
    {
        private readonly AddressBookContext _context;

        public GroupPhonesController(AddressBookContext context)
        {
            _context = context;
        }

        // GET: api/GroupPhones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupPhone>>> GetGroupPhone()
        {
            return await _context.GroupPhone.ToListAsync();
        }

        // GET: api/GroupPhones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupPhone>> GetGroupPhone(int id)
        {
            var groupPhone = await _context.GroupPhone.FindAsync(id);

            if (groupPhone == null)
            {
                return NotFound();
            }

            return groupPhone;
        }

        // PUT: api/GroupPhones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupPhone(int id, GroupPhone groupPhone)
        {
            if (id != groupPhone.Id)
            {
                return BadRequest();
            }

            _context.Entry(groupPhone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupPhoneExists(id))
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

        // POST: api/GroupPhones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GroupPhone>> PostGroupPhone(GroupPhone groupPhone)
        {
            _context.GroupPhone.Add(groupPhone);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupPhone", new { id = groupPhone.Id }, groupPhone);
        }

        // DELETE: api/GroupPhones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupPhone>> DeleteGroupPhone(int id)
        {
            var groupPhone = await _context.GroupPhone.FindAsync(id);
            if (groupPhone == null)
            {
                return NotFound();
            }

            _context.GroupPhone.Remove(groupPhone);
            await _context.SaveChangesAsync();

            return groupPhone;
        }

        private bool GroupPhoneExists(int id)
        {
            return _context.GroupPhone.Any(e => e.Id == id);
        }
    }
}
