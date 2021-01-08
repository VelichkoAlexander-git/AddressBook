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
    public class PhonesController : ControllerBase
    {
        private readonly ManageUsersService _userService;
        private readonly ManagePhonesService _phoneService;

        public PhonesController(ManageUsersService userService, ManagePhonesService phoneService)
        {
            _userService = userService;
            _phoneService = phoneService;
        }

        // GET: api/Phones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhoneDto>>> GetPhone(int userId, int abonentId)
        {
            var user = _userService.GetUser(userId);
            var abonent = user.Abonents.FirstOrDefault(a => a.Id == abonentId);
            List<PhoneDto> item = abonent.Phones.Select(p => new PhoneDto() { Id = p.Id, AbonentId = p.AbonentId, GroupPhoneId = p.GroupPhoneId, Number = p.Number }).ToList();

            return item;
        }

        // GET: api/Phones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhoneDto>> GetPhone(int userId, int abonentId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = user.Abonents.FirstOrDefault(a => a.Id == abonentId);
                if (abonent != null)
                {
                    var phone = abonent.Phones.FirstOrDefault(u => u.Id == id);
                    if (phone != null)
                    {
                        PhoneDto item = new PhoneDto()
                        {
                            Id = phone.Id,
                            AbonentId = phone.AbonentId,
                            GroupPhoneId = phone.GroupPhoneId,
                            Number = phone.Number
                        };

                        return item;
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return NotFound();
        }

        // PUT: api/Phones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePhone(int userId, int abonentId, int id, PhoneDto phoneDto)
        {
            if (id != phoneDto.Id)
            {
                return BadRequest();
            }

            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = user.Abonents.FirstOrDefault(u => u.Id == abonentId);
                if (abonent != null)
                {
                    var phone = abonent.Phones.FirstOrDefault(p => p.Id == id);
                    if (phone != null)
                    {
                        if (!abonent.Phones.Any(p => p.Number == phoneDto.Number))
                        {
                            var groupPhone = user.GroupPhones.FirstOrDefault(gp => gp.Id == phoneDto.GroupPhoneId);
                            if (groupPhone != null || phoneDto.GroupPhoneId == null)
                            {
                                await _phoneService.UpdatePhoneAsync(abonent, id, groupPhone, phoneDto.Number);
                                return Ok();
                            }
                            return BadRequest("User -> GroupPhone not found");
                        }
                        return Conflict();
                    }
                    return BadRequest("User -> Abonent -> Phone not found");
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // POST: api/Phones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Phone>> AddPhone(int userId, int abonentId, PhoneDto phoneDto)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = user.Abonents.FirstOrDefault(u => u.Id == abonentId);
                if (abonent != null)
                {
                    if (!abonent.Phones.Any(p => p.Number == phoneDto.Number))
                    {
                        phoneDto.AbonentId = abonentId;
                        var phoneGroup = user.GroupPhones.FirstOrDefault(gp => gp.Id == phoneDto.GroupPhoneId);
                        if (phoneGroup != null)
                        {
                            await _phoneService.AddPhoneAsync(abonent, phoneGroup, phoneDto.Number);
                            return CreatedAtAction("GetPhone", new { userId = userId, abonentId = phoneDto.AbonentId, id = phoneDto.Id }, phoneDto);
                        }
                        return BadRequest("User -> GroupPhones not found");
                    }
                    return Conflict();
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // DELETE: api/Phones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Phone>> DeletePhone(int userId, int abonentId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = user.Abonents.FirstOrDefault(u => u.Id == abonentId);
                if (abonent != null)
                {
                    var phone = abonent.Phones.FirstOrDefault(u => u.Id == id);
                    if (phone != null)
                    {
                        await _phoneService.DeletePhoneAsync(abonent, phone);

                        return phone;
                    }
                    return BadRequest("User -> Abonent -> Phone not found");
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }
    }
}
