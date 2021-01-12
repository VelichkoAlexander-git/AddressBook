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
        private readonly ManageUsersService _userService;
        private readonly ManageAddressService _addressService;

        public AddressesController(ManageUsersService userService, ManageAddressService service)
        {
            _userService = userService;
            _addressService = service;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressDto>>> GetAddress(int userId, int abonentId)
        {
            var user = _userService.GetUser(userId);
            var abonent = user.Abonents.FirstOrDefault(a => a.Id == abonentId);
            List<AddressDto> item = abonent.Addresses.Select(a => new AddressDto() { Id = a.Id, AbonentId = a.AbonentId, GroupAddressId = a.GroupAddressId, Information = a.Information }).ToList();

            return item;
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressDto>> GetAddress(int userId, int abonentId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = user.Abonents.FirstOrDefault(a => a.Id == abonentId);
                if (abonent != null)
                {
                    var address = abonent.Addresses.FirstOrDefault(u => u.Id == id);
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
        public async Task<IActionResult> UpdateAddress(int userId, int abonentId, int id, AddressDto addressDto)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = user.Abonents.FirstOrDefault(u => u.Id == abonentId);
                if (abonent != null)
                {
                    var address = abonent.Addresses.FirstOrDefault(a => a.Id == id);
                    if (address != null)
                    {
                        if (!abonent.Addresses.Any(p => p.Information == addressDto.Information))
                        {
                            var addressGroup = user.GroupAddresses.FirstOrDefault(ga => ga.Id == addressDto.GroupAddressId);
                            if (addressGroup != null)
                            {
                                await _addressService.UpdateAddressAsync(abonent, id, addressGroup, addressDto.Information);
                                return Ok();
                            }
                            return BadRequest("User -> GroupAddress not found");
                        }
                        return Conflict();
                    }
                    return BadRequest("User -> Abonent -> Address not found");
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // POST: api/Addresses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Address>> AddAddress(int userId, int abonentId, AddressDto addressDto)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = user.Abonents.FirstOrDefault(u => u.Id == abonentId);
                if (abonent != null)
                {
                    if (!abonent.Addresses.Any(p => p.Information == addressDto.Information))
                    {
                        var addressGroup = user.GroupAddresses.FirstOrDefault(ga => ga.Id == addressDto.GroupAddressId);
                        if (addressGroup != null)
                        {
                            await _addressService.AddAddressAsync(abonent, addressGroup, addressDto.Information);
                            addressDto = _addressService.GetAddress(abonent, addressGroup, addressDto.Information).Value;
                            return CreatedAtAction("GetAddress", new { userId = userId, abonentId = addressDto.AbonentId, id = addressDto.Id }, addressDto);
                        }
                        return BadRequest("User -> GroupAddress not found");
                    }
                    return Conflict();
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AddressDto>> DeleteAddress(int userId, int abonentId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = user.Abonents.FirstOrDefault(u => u.Id == abonentId);
                if (abonent != null)
                {
                    var address = abonent.Addresses.FirstOrDefault(u => u.Id == id);
                    if (address == null)
                    {
                        await _addressService.DeleteAddressAsync(abonent, address);

                        return new AddressDto()
                        {
                            Id = address.Id,
                            AbonentId = address.AbonentId,
                            GroupAddressId = address.GroupAddressId,
                            Information = address.Information
                        };
                    }
                    return BadRequest("User -> Abonent -> Address not found");
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }
    }
}
