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
    [Route("api/Users/{userId:int}/Abonents/{AbonentId:int}/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AddressBookContext _context;
        private readonly ManageAddressService _service;

        public AddressesController(AddressBookContext context, ManageAddressService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressDto>>> GetAddress(int userId, int abonentId)
        {
            var user = _context.GetUser(userId);
            var abonent = user.AbonentInternal.Find(a => a.Id == abonentId);
            List<AddressDto> item = abonent.AddressInternal.Select(a => new AddressDto() { Id = a.Id, AbonentId = a.AbonentId, GroupAddressId = a.GroupAddressId, Information = a.Information }).ToList();

            return item;
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressDto>> GetAddress(int userId, int abonentId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(a => a.Id == abonentId);
                if (abonent != null)
                {
                    var address = abonent.AddressInternal.FirstOrDefault(u => u.Id == id);
                    if (address != null)
                    {
                        AddressDto item = new AddressDto()
                        {
                            Id = address.Id,
                            AbonentId = address.AbonentId,
                            GroupAddressId = address.GroupAddressId,
                            Information = address.Information
                        };

                        return item;
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return NotFound();
        }

        // PUT: api/Addresses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int userId, int abonentId, int id, AddressDto addressDto)
        {
            if (id != addressDto.Id)
            {
                return BadRequest();
            }

            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(u => u.Id == abonentId);
                if (abonent != null)
                {
                    var groupAddress = user.GroupAddressInternal.Find(gp => gp.Id == addressDto.GroupAddressId);
                    if (groupAddress != null || addressDto.GroupAddressId == null)
                    {
                        var address = abonent.UpdateAddress(id, groupAddress, addressDto.Information);
                        if (address.Succeeded)
                        {
                            await _context.SaveChangesAsync();
                            return Ok();
                        }
                        return BadRequest(address.Errors);
                    }
                    return BadRequest("User -> GroupAddress not found");
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // POST: api/Addresses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(int userId, int abonentId, AddressDto addressDto)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(u => u.Id == abonentId);
                if (abonent != null)
                {
                    if (!abonent.AddressInternal.Any(p => p.Information == addressDto.Information))
                    {
                        addressDto.AbonentId = abonentId;
                        await _service.AddAddressAsync(userId, addressDto);
                        return CreatedAtAction("GetAddress", new { userId = userId, abonentId = addressDto.AbonentId, id = addressDto.Id }, addressDto);
                    }
                    return Conflict();
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Address>> DeleteAddress(int userId, int abonentId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(u => u.Id == abonentId);
                if (abonent != null)
                {
                    var address = abonent.AddressInternal.FirstOrDefault(u => u.Id == id);
                    if (address == null)
                    {
                        return NotFound();
                    }

                    abonent.RemoveAddress(address);
                    await _context.SaveChangesAsync();

                    return address;
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }
    }
}
