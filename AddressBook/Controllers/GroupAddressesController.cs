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
        private readonly AddressBookContext _context;
        private readonly ManageGroupAddressService _service;

        public GroupAddressesController(AddressBookContext context, ManageGroupAddressService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/GroupAddresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupAddressDto>>> GetGroupAddress(int userId)
        {
            var user = _context.GetUser(userId);
            List<GroupAddressDto> item = user.GroupAddressInternal.Select(u => new GroupAddressDto() { Id = u.Id, UserId = u.UserId, Name = u.Name }).ToList();

            return item;
        }

        // GET: api/GroupAddresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupAddressDto>> GetGroupAddress(int userId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var groupAddress = user.GroupAddressInternal.Find(u => u.Id == id);
                if (groupAddress != null)
                {
                    GroupAddressDto item = new GroupAddressDto()
                    {
                        Id = groupAddress.Id,
                        UserId = groupAddress.UserId,
                        Name = groupAddress.Name
                    };

                    return item;
                }
                return NotFound();
            }
            return NotFound();
        }

        // PUT: api/GroupAddresses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupAddress(int userId, int id, GroupAddressDto groupAddressDto)
        {
            if (id != groupAddressDto.Id)
            {
                return BadRequest();
            }

            var user = _context.GetUser(userId);
            if (user != null)
            {
                var groupAddress = user.UpdateGroupAddress(id, groupAddressDto.Name);
                if (groupAddress.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest(groupAddress.Errors);
            }
            return BadRequest("User not found");
        }

        // POST: api/GroupAddresses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GroupAddress>> PostGroupAddress(int userId, GroupAddressDto groupAddress)
        {
            if (!_context.Users.Find(userId).GroupAddressInternal.Any(g => g.Name == groupAddress.Name))
            {
                groupAddress.UserId = userId;
                await _service.AddGroupAddressAsync(groupAddress);
                return CreatedAtAction("GetGroupAddress", new { userId = groupAddress.UserId, id = groupAddress.Id }, groupAddress);
            }
            return Conflict();
        }

        // DELETE: api/GroupAddresses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupAddress>> DeleteGroupAddress(int userId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var groupAddress = user.GroupAddressInternal.Find(u => u.Id == id);
                user.RemoveGroupAddress(groupAddress);
                await _context.SaveChangesAsync();
                return groupAddress;
            }
            return NotFound();
        }
    }
}
