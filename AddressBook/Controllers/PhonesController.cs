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
        private readonly ManageAbonentService _abonentService;

        public PhonesController(ManageUsersService userService, ManagePhonesService phoneService, ManageAbonentService abonentService)
        {
            _userService = userService;
            _phoneService = phoneService;
            _abonentService = abonentService;
        }

        // GET: api/Phones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhoneDto>>> GetPhone(int userId, int abonentId)
        {
            var user = _userService.GetUser(userId);
            var abonent = _abonentService.GetAbonent(user, abonentId);
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
                var abonent = _abonentService.GetAbonent(user, abonentId);
                if (abonent != null)
                {
                    var phone = abonent.Phones.FirstOrDefault(u => u.Id == id);
                    if (phone != null)
                    {
                        return new PhoneDto()
                        {
                            Id = phone.Id,
                            AbonentId = phone.AbonentId,
                            GroupPhoneId = phone.GroupPhoneId,
                            Number = phone.Number
                        };
                    }
                    return BadRequest("User -> Abonent -> Phone not found");
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // PUT: api/Phones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePhone(int userId, int abonentId, int id, PhoneDto phoneDto)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = _abonentService.GetAbonent(user, abonentId);
                if (abonent != null)
                {
                    var phone = abonent.Phones.FirstOrDefault(p => p.Id == id);
                    if (phone != null)
                    {
                        var groupPhone = user.GroupPhones.FirstOrDefault(gp => gp.Id == phoneDto.GroupPhoneId);
                        if (groupPhone != null || phoneDto.GroupPhoneId == null)
                        {
                            var answer = await _phoneService.UpdatePhoneAsync(abonent, id, groupPhone, phoneDto.Number);
                            if (answer.Succeeded)
                            {
                                return Ok();
                            }
                            return BadRequest(answer.Errors);
                        }
                        return BadRequest("User -> GroupPhone not found");
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
                var abonent = _abonentService.GetAbonent(user, abonentId);
                if (abonent != null)
                {
                    var phoneGroup = user.GroupPhones.FirstOrDefault(gp => gp.Id == phoneDto.GroupPhoneId);
                    if (phoneGroup != null)
                    {
                        var answer = await _phoneService.AddPhoneAsync(abonent, phoneGroup, phoneDto.Number);
                        if (answer.Succeeded)
                        {
                            var answerDto = _phoneService.GetPhone(abonent, phoneGroup, phoneDto.Number);
                            if (answerDto.Succeeded)
                            {
                                phoneDto = answerDto.Value;
                                return CreatedAtAction("GetPhone", new { userId = userId, abonentId = phoneDto.AbonentId, id = phoneDto.Id }, phoneDto);
                            }
                            return BadRequest(answerDto.Errors);
                        }
                        return BadRequest(answer.Errors);
                    }
                    return BadRequest("User -> GroupPhones not found");
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // DELETE: api/Phones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PhoneDto>> DeletePhone(int userId, int abonentId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = _abonentService.GetAbonent(user, abonentId);
                if (abonent != null)
                {
                    var phone = abonent.Phones.FirstOrDefault(u => u.Id == id);
                    if (phone != null)
                    {
                        await _phoneService.DeletePhoneAsync(abonent, phone);
                        return new PhoneDto() { Id = phone.Id, AbonentId = phone.AbonentId, GroupPhoneId = phone.GroupPhoneId, Number = phone.Number };
                    }
                    return BadRequest("User -> Abonent -> Phone not found");
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }
    }
}
