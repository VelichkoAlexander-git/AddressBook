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
        private readonly AddressBookContext _context;
        private readonly ManagePhonesService _service;

        public PhonesController(AddressBookContext context, ManagePhonesService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/Phones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhoneDto>>> GetPhone(int userId, int abonentId)
        {
            var user = _context.GetUser(userId);
            var abonent = user.AbonentInternal.Find(a => a.Id == abonentId);
            List<PhoneDto> item = abonent.PhoneInternal.Select(p => new PhoneDto() { Id = p.Id, AbonentId = p.AbonentId, GroupPhoneId = p.GroupPhoneId, Number = p.Number }).ToList();

            return item;
        }

        // GET: api/Phones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhoneDto>> GetPhone(int userId, int abonentId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(a => a.Id == abonentId);
                if (abonent != null)
                {
                    var phone = abonent.PhoneInternal.FirstOrDefault(u => u.Id == id);
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
        public async Task<IActionResult> PutPhone(int userId, int abonentId, int id, PhoneDto phoneDto)
        {
            if (id != phoneDto.Id)
            {
                return BadRequest();
            }

            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(u => u.Id == abonentId);
                if (abonent != null)
                {
                    var groupPhone = user.GroupPhoneInternal.Find(gp => gp.Id == phoneDto.GroupPhoneId);
                    if (groupPhone != null || phoneDto.GroupPhoneId == null)
                    {
                        var phone = abonent.UpdatePhone(id, groupPhone, phoneDto.Number);
                        if (phone.Succeeded)
                        {
                            await _context.SaveChangesAsync();
                            return Ok();
                        }
                        return BadRequest(phone.Errors);
                    }
                    return BadRequest("User -> GroupPhone not found");
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // POST: api/Phones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Phone>> PostPhone(int userId, int abonentId, PhoneDto phoneDto)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(u => u.Id == abonentId);
                if (abonent != null)
                {
                    if (!abonent.PhoneInternal.Any(p => p.Number == phoneDto.Number))
                    {
                        phoneDto.AbonentId = abonentId;
                        await _service.AddPhoneAsync(userId, phoneDto);
                        return CreatedAtAction("GetPhone", new { userId = userId, abonentId = phoneDto.AbonentId, id = phoneDto.Id }, phoneDto);
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
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(u => u.Id == abonentId);
                if (abonent != null)
                {
                    var phone = abonent.PhoneInternal.FirstOrDefault(u => u.Id == id);
                    if (phone == null)
                    {
                        return NotFound();
                    }

                    abonent.RemovePhone(phone);
                    await _context.SaveChangesAsync();

                    return phone;
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }
    }
}
