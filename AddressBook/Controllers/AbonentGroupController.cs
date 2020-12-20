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
            List<AbonentGroupDto> item = abonent.GroupInternal.Select(ag => new AbonentGroupDto() { Id = ag.Id, AbonentId = ag.AbonentId, GroupId = ag.GroupId }).ToList();

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
                    var abonentGroup = abonent.GroupInternal.FirstOrDefault(u => u.Id == id);
                    if (abonentGroup != null)
                    {
                        AbonentGroupDto item = new AbonentGroupDto()
                        {
                            Id = abonentGroup.Id,
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
        public async Task<IActionResult> PutAbonentGroup(int userId, int abonentId, int id, AbonentGroupDto abonentGroupDto)
        {
            if (id != abonentGroupDto.Id)
            {
                return BadRequest();
            }

            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(u => u.Id == abonentId);
                if (abonent != null)
                {
                    var group = user.GroupInternal.Find(g => g.Id == abonentGroupDto.GroupId);
                    if (group == null)
                    {
                        return BadRequest("User -> Group not found");
                    }

                    var abonentGroup = abonent.UpdateAbonentGroup(id, group);
                    if (abonentGroup.Succeeded)
                    {
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest(abonentGroup.Errors);
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // POST: api/AbonentGroups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AbonentGroup>> PostAbonentGroup(int userId, int abonentId, AbonentGroupDto abonentGroup)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(u => u.Id == abonentId);
                if (abonent != null)
                {
                    if (!abonent.GroupInternal.Any(p => p.GroupId == abonentGroup.GroupId))
                    {
                        abonentGroup.AbonentId = abonentId;
                        await _service.AddAbonentGroupAsync(userId, abonentGroup);
                        return CreatedAtAction("GetAbonentGroup", new { userId = userId, abonentId = abonentGroup.AbonentId, id = abonentGroup.Id }, abonentGroup);
                    }
                    return Conflict();
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // DELETE: api/AbonentGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AbonentGroup>> DeleteAbonentGroup(int userId, int abonentId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(u => u.Id == abonentId);
                if (abonent != null)
                {
                    var abonentGroup = abonent.GroupInternal.FirstOrDefault(u => u.Id == id);
                    if (abonentGroup == null)
                    {
                        return NotFound();
                    }

                    abonent.RemoveGroup(abonentGroup);
                    await _context.SaveChangesAsync();

                    return abonentGroup;
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }
    }
}
