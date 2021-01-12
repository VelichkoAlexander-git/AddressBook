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
        private readonly ManageAbonentService _abonentService;

        public AddressesController(ManageUsersService userService, ManageAddressService service, ManageAbonentService abonentService)
        {
            _userService = userService;
            _addressService = service;
            _abonentService = abonentService;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressDto>>> GetAddress(int userId, int abonentId)
        {
            var user = _userService.GetUser(userId);
            var abonent = _abonentService.GetAbonent(user, abonentId);
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
                var abonent = _abonentService.GetAbonent(user, abonentId);
                if (abonent != null)
                {
                    var address = abonent.Addresses.FirstOrDefault(u => u.Id == id);
                    if (address != null)
                    {
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

        // PUT: api/Addresses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int userId, int abonentId, int id, AddressDto addressDto)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = _abonentService.GetAbonent(user, abonentId);
                if (abonent != null)
                {
                    var address = abonent.Addresses.FirstOrDefault(a => a.Id == id);
                    if (address != null)
                    {
                        var addressGroup = user.GroupAddresses.FirstOrDefault(ga => ga.Id == addressDto.GroupAddressId);
                        if (addressGroup != null)
                        {
                            var answer = await _addressService.UpdateAddressAsync(abonent, id, addressGroup, addressDto.Information);
                            if (answer.Succeeded)
                            {
                                return Ok();
                            }
                            return BadRequest(answer.Errors);
                        }
                        return BadRequest("User -> GroupAddress not found");
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
                var abonent = _abonentService.GetAbonent(user, abonentId);
                if (abonent != null)
                {
                    var addressGroup = user.GroupAddresses.FirstOrDefault(ga => ga.Id == addressDto.GroupAddressId);
                    if (addressGroup != null)
                    {
                        var answer = await _addressService.AddAddressAsync(abonent, addressGroup, addressDto.Information);
                        if (answer.Succeeded)
                        {
                            var answerDto = _addressService.GetAddress(abonent, addressGroup, addressDto.Information);
                            if (answerDto.Succeeded)
                            {
                                addressDto = answerDto.Value;
                                return CreatedAtAction("GetAddress", new { userId = userId, abonentId = addressDto.AbonentId, id = addressDto.Id }, addressDto);
                            }
                            return BadRequest(answerDto.Errors);
                        }
                        return BadRequest(answer.Errors);
                    }
                    return BadRequest("User -> GroupAddress not found");
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
                var abonent = _abonentService.GetAbonent(user, abonentId);
                if (abonent != null)
                {
                    var address = abonent.Addresses.FirstOrDefault(u => u.Id == id);
                    if (address != null)
                    {
                        await _addressService.DeleteAddressAsync(abonent, address);
                        return new AddressDto() { Id = address.Id, AbonentId = address.AbonentId, GroupAddressId = address.GroupAddressId, Information = address.Information };
                    }
                    return BadRequest("User -> Abonent -> Address not found");
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }
    }
}
