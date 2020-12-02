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
    public class GroupAddressesController : ControllerBase
    {
        private readonly AddressBookContext _context;

        public GroupAddressesController(AddressBookContext context)
        {
            _context = context;
        }

        // GET: api/GroupAddresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupAddress>>> GetGroupAddress()
        {
            return await _context.GroupAddress.ToListAsync();
        }

        // GET: api/GroupAddresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupAddress>> GetGroupAddress(int id)
        {
            var groupAddress = await _context.GroupAddress.FindAsync(id);

            if (groupAddress == null)
            {
                return NotFound();
            }

            return groupAddress;
        }

        // PUT: api/GroupAddresses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupAddress(int id, GroupAddress groupAddress)
        {
            if (id != groupAddress.Id)
            {
                return BadRequest();
            }

            _context.Entry(groupAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupAddressExists(id))
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

        // POST: api/GroupAddresses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GroupAddress>> PostGroupAddress(GroupAddress groupAddress)
        {
            _context.GroupAddress.Add(groupAddress);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupAddress", new { id = groupAddress.Id }, groupAddress);
        }

        // DELETE: api/GroupAddresses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupAddress>> DeleteGroupAddress(int id)
        {
            var groupAddress = await _context.GroupAddress.FindAsync(id);
            if (groupAddress == null)
            {
                return NotFound();
            }

            _context.GroupAddress.Remove(groupAddress);
            await _context.SaveChangesAsync();

            return groupAddress;
        }

        private bool GroupAddressExists(int id)
        {
            return _context.GroupAddress.Any(e => e.Id == id);
        }
    }
}
