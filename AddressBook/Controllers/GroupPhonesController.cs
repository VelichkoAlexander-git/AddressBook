﻿using System;
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
    public class GroupPhonesController : ControllerBase
    {
        private readonly ManageUsersService _userService;
        private readonly ManageGroupPhoneService _groupPhoneService;

        public GroupPhonesController(ManageUsersService userService, ManageGroupPhoneService groupPhoneService)
        {
            _userService = userService;
            _groupPhoneService = groupPhoneService;
        }

        // GET: api/GroupPhones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupPhoneDto>>> GetGroupPhone(int userId)
        {
            var user = _userService.GetUser(userId);
            List<GroupPhoneDto> item = user.GroupPhones.Select(u => new GroupPhoneDto() { Id = u.Id, UserId = u.UserId, Name = u.Name }).ToList();

            return item;
        }

        // GET: api/GroupPhones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupPhoneDto>> GetGroupPhone(int userId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var groupPhone = user.GroupPhones.FirstOrDefault(u => u.Id == id);
                if (groupPhone != null)
                {
                    return new GroupPhoneDto()
                    {
                        Id = groupPhone.Id,
                        UserId = groupPhone.UserId,
                        Name = groupPhone.Name
                    };
                }
                return BadRequest("User -> GroupPhone not found");
            }
            return BadRequest("User not found");
        }

        // PUT: api/GroupPhones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroupPhone(int userId, int id, GroupPhoneDto groupPhoneDto)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var groupPhone = user.GroupPhones.FirstOrDefault(gp => gp.Id == id);
                if (groupPhone != null)
                {
                    var answer = await _groupPhoneService.UpdateGroupPhoneAsync(user, id, groupPhoneDto.Name);
                    if (answer.Succeeded)
                    {
                        return Ok();
                    }
                    return BadRequest(answer.Errors);
                }
                return BadRequest("User -> GroupPhone not found");
            }
            return BadRequest("User not found");
        }

        // POST: api/GroupPhones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GroupPhoneDto>> AddGroupPhone(int userId, GroupPhoneDto groupPhoneDto)
        {
            groupPhoneDto.UserId = userId;
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var answer = await _groupPhoneService.AddGroupPhoneAsync(user, (string)groupPhoneDto.Name);
                if (answer.Succeeded)
                {
                    var answerDto = _groupPhoneService.GetGroupPhone(user, (string)groupPhoneDto.Name);
                    if (answerDto.Succeeded)
                    {
                        groupPhoneDto = answerDto.Value;
                        return CreatedAtAction("GetGroupPhone", new { userId = groupPhoneDto.UserId, id = groupPhoneDto.Id }, groupPhoneDto);
                    }
                    return BadRequest(answerDto.Errors);
                }
                return BadRequest(answer.Errors);
            }
            return BadRequest("User not found");
        }

        // DELETE: api/GroupPhones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupPhoneDto>> DeleteGroupPhone(int userId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var groupPhone = user.GroupPhones.FirstOrDefault(u => u.Id == id);
                if (groupPhone != null)
                {
                    GroupPhoneDto dto = new GroupPhoneDto()
                    {
                        Id = groupPhone.Id,
                        UserId = groupPhone.UserId,
                        Name = groupPhone.Name
                    };
                    await _groupPhoneService.DeleteGroupPhoneAsync(user, groupPhone);

                    return dto;
                }
                return BadRequest("User -> GroupPhone not found");
            }
            return BadRequest("User not found");
        }
    }
}
