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
    public class AbonentsController : ControllerBase
    {
        private readonly AddressBookContext _context;
        private readonly ManageAbonentService _service;

        public AbonentsController(AddressBookContext context, ManageAbonentService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/Abonents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AbonentDto>>> GetAbonent(int userId)
        {
            var user = _context.GetUser(userId);
            List<AbonentDto> item = user.AbonentInternal.Select(u => new AbonentDto()
            {
                Id = u.Id,
                UserId = u.UserId,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                Sex = u.Sex,
                DateOfBirth = u.DateOfBirth,
                Photo = u.Photo,
                Mail = u.Mail
            }).ToList();

            return item;
        }

        // GET: api/Abonents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AbonentDto>> GetAbonent(int userId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var Abonent = user.AbonentInternal.Find(u => u.Id == id);
                if (Abonent != null)
                {
                    AbonentDto item = new AbonentDto()
                    {
                        Id = Abonent.Id,
                        UserId = Abonent.UserId,
                        FirstName = Abonent.FirstName,
                        MiddleName = Abonent.MiddleName,
                        LastName = Abonent.LastName,
                        Sex = Abonent.Sex,
                        DateOfBirth = Abonent.DateOfBirth,
                        Photo = Abonent.Photo,
                        Mail = Abonent.Mail
                    };

                    return item;
                }
                return NotFound();
            }
            return NotFound();
        }

        // PUT: api/Abonents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAbonent(int userId, int id, AbonentDto AbonentDto)
        {
            if (id != AbonentDto.Id)
            {
                return BadRequest();
            }

            var user = _context.GetUser(userId);
            if (user != null)
            {
                var Abonent = user.UpdateAbonent(id, 
                                                       AbonentDto.FirstName, 
                                                       AbonentDto.MiddleName, 
                                                       AbonentDto.LastName,
                                                       AbonentDto.Sex,
                                                       AbonentDto.DateOfBirth,
                                                       AbonentDto.Photo,
                                                       AbonentDto.Mail
                                                       );
                if (Abonent.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest(Abonent.Errors);
            }
            return BadRequest("User not found");
        }

        // POST: api/Abonents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AbonentDto>> PostAbonent(int userId, AbonentDto AbonentDto)
        {
            if (!_context.Users.Find(userId).AbonentInternal.Any(g => g.FirstName == AbonentDto.FirstName
                                                                 && g.MiddleName == AbonentDto.MiddleName
                                                                 && g.LastName == AbonentDto.LastName))
            {
                AbonentDto.UserId = userId;
                await _service.AddAbonentAsync(AbonentDto);
                return CreatedAtAction("GetAbonent", new { userId = AbonentDto.UserId, id = AbonentDto.Id }, AbonentDto);
            }
            return Conflict();
        }

        // DELETE: api/Abonents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Abonent>> DeleteAbonent(int userId, int id)
        {
            var user = _context.GetUser(userId);
            if (user != null)
            {
                var abonent = user.AbonentInternal.Find(u => u.Id == id);
                if (abonent == null)
                {
                    return NotFound();
                }

                user.RemoveAbonent(abonent);
                await _context.SaveChangesAsync();
                return abonent;
            }
            return NotFound();
        }
    }
}
