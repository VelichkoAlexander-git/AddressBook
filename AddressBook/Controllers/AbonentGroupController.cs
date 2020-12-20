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
    [Route("api/Users/{userId:int}/Abonents/{abonentId:int}/[controller]")]
    [ApiController]
    public class AbonentGroupController : ControllerBase
    {
        private readonly AddressBookContext _context;
        private readonly ManageAbonentGroupService _service;

        public AbonentGroupController(AddressBookContext context, ManageAbonentGroupService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/AbonentGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AbonentGroupDto>>> GetAbonentGroup(int userId, int abonentId)
        {
            var user = _context.GetUser(userId);
            var abonent = user.AbonentInternal.Find(a => a.Id == abonentId);
            List<AbonentGroupDto> item = abonent.GroupInternal.Select(ag => new AbonentGroupDto() { AbonentId = ag.AbonentId, GroupId = ag.GroupId }).ToList();

            return item;
        }

        // GET: api/AbonentGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AbonentGroupDto>> GetAbonentGroup(int userId, int abonentId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(a => a.Id == abonentId);
                if (abonent != null)
                {
                    var abonentGroup = abonent.GroupInternal.FirstOrDefault(u => u. == id);
                    if (abonentGroup != null)
                    {
                        AbonentGroupDto item = new AbonentGroupDto()
                        {
                             AbonentId = abonentGroup.AbonentId,
                             GroupId = abonentGroup.GroupId
                        };

                        return item;
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return NotFound();
        }

        // PUT: api/AbonentGroups/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAbonentGroup(int id, AbonentGroup AbonentGroup)
        {
            if (id != AbonentGroup.GroupId)
            {
                return BadRequest();
            }

            _context.Entry(AbonentGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbonentGroupExists(id))
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

        // POST: api/AbonentGroups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AbonentGroup>> PostAbonentGroup(AbonentGroup AbonentGroup)
        {
            _context.AbonentGroup.Add(AbonentGroup);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AbonentGroupExists(AbonentGroup.GroupId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAbonentGroup", new { id = AbonentGroup.GroupId }, AbonentGroup);
        }

        // DELETE: api/AbonentGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AbonentGroup>> DeleteAbonentGroup(int id)
        {
            var AbonentGroup = await _context.AbonentGroup.FindAsync(id);
            if (AbonentGroup == null)
            {
                return NotFound();
            }

            _context.AbonentGroup.Remove(AbonentGroup);
            await _context.SaveChangesAsync();

            return AbonentGroup;
        }

        private bool AbonentGroupExists(int id)
        {
            return _context.AbonentGroup.Any(e => e.GroupId == id);
        }
    }
}
