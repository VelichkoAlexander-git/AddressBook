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
        private readonly ManageUsersService _userService;
        private readonly ManageAbonentService _abonentService;

        public AbonentsController(ManageUsersService userService, ManageAbonentService service)
        {
            _userService = userService;
            _abonentService = service;
        }

        // GET: api/Abonents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AbonentDto>>> GetAbonent(int userId)
        {
            var user = _userService.GetUser(userId);
            List<AbonentDto> item = user.Abonents.Select(u => new AbonentDto()
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
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = _abonentService.GetAbonent(user, id);
                if (abonent != null)
                {
                    return new AbonentDto()
                    {
                        Id = abonent.Id,
                        UserId = abonent.UserId,
                        FirstName = abonent.FirstName,
                        MiddleName = abonent.MiddleName,
                        LastName = abonent.LastName,
                        Sex = abonent.Sex,
                        DateOfBirth = abonent.DateOfBirth,
                        Photo = abonent.Photo,
                        Mail = abonent.Mail
                    };
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // PUT: api/Abonents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAbonent(int userId, int id, AbonentDto AbonentDto)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = _abonentService.GetAbonent(user, id);
                if (abonent != null)
                {

                    var answer = await _abonentService.UpdateAbonentAsync(user, id, AbonentDto.FirstName,
                                                                       AbonentDto.MiddleName,
                                                                       AbonentDto.LastName,
                                                                       AbonentDto.DateOfBirth,
                                                                       AbonentDto.Photo,
                                                                       AbonentDto.Sex,
                                                                       AbonentDto.Mail);
                    if (answer.Succeeded)
                    {
                        return Ok();
                    }
                    return BadRequest(answer.Errors);
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // POST: api/Abonents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AbonentDto>> AddAbonent(int userId, AbonentDto AbonentDto)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {

                var answer = await _abonentService.AddAbonentAsync(user, AbonentDto.FirstName,
                                                            AbonentDto.MiddleName,
                                                            AbonentDto.LastName,
                                                            AbonentDto.DateOfBirth,
                                                            AbonentDto.Photo,
                                                            AbonentDto.Sex,
                                                            AbonentDto.Mail);
                if (answer.Succeeded)
                {
                    var answerDto = _abonentService.GetAbonent(user, AbonentDto.FirstName, AbonentDto.MiddleName, AbonentDto.LastName);
                    if (answerDto.Succeeded)
                    {
                        AbonentDto = answerDto.Value;
                        return CreatedAtAction("GetAbonent", new { userId = AbonentDto.UserId, id = AbonentDto.Id }, AbonentDto);
                    }
                    return BadRequest(answerDto.Errors);
                }
                return BadRequest(answer.Errors);
            }
            return BadRequest("User not found");
        }

        // DELETE: api/Abonents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AbonentDto>> DeleteAbonent(int userId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = _abonentService.GetAbonent(user, id);
                if (abonent != null)
                {
                    await _abonentService.DeleteAbonentAsync(user, abonent);
                    return new AbonentDto()
                    {
                        Id = abonent.Id,
                        UserId = abonent.UserId,
                        FirstName = abonent.FirstName,
                        MiddleName = abonent.MiddleName,
                        LastName = abonent.LastName,
                        DateOfBirth = abonent.DateOfBirth,
                        Photo = abonent.Photo,
                        Sex = abonent.Sex,
                        Mail = abonent.Mail
                    };
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }
    }
}
