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
    public class AbonentGroupController : ControllerBase
    {
        private readonly ManageUsersService _userService;
        private readonly ManageAbonentGroupService _abonentGroupServiceService;
        private readonly ManageAbonentService _abonentService;

        public AbonentGroupController(ManageUsersService userService, ManageAbonentGroupService service, ManageAbonentService abonentService)
        {
            _userService = userService;
            _abonentGroupServiceService = service;
            _abonentService = abonentService;
        }

        // GET: api/AbonentGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AbonentGroupDto>>> GetAbonentGroup(int userId, int abonentId)
        {
            var user = _userService.GetUser(userId);
            var abonent = _abonentService.GetAbonent(user, abonentId);
            List<AbonentGroupDto> item = abonent.Groups.Select(ag => new AbonentGroupDto() { AbonentId = abonentId, GroupId = ag.Id }).ToList();

            return item;
        }

        // GET: api/AbonentGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AbonentGroupDto>> GetAbonentGroup(int userId, int abonentId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = _abonentService.GetAbonent(user, abonentId);
                if (abonent != null)
                {
                    var group = abonent.Groups.FirstOrDefault(u => u.Id == id);
                    if (group != null)
                    {
                        return new AbonentGroupDto()
                        {
                            AbonentId = abonentId,
                            GroupId = group.Id
                        };
                    }
                    return BadRequest("User -> Group not found");
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // POST: api/AbonentGroups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AbonentGroupDto>> AddAbonentGroup(int userId, int abonentId, AbonentGroupDto abonentGroup)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = _abonentService.GetAbonent(user, abonentId);
                if (abonent != null)
                {
                    if (!abonent.Groups.Any(p => p.Id == abonentGroup.GroupId))
                    {
                        var group = user.Groups.FirstOrDefault(g => g.Id == abonentGroup.GroupId);
                        if (group != null)
                        {
                            await _abonentGroupServiceService.AddAbonentGroupAsync(abonent, group);
                            abonentGroup.AbonentId = abonentId;
                            return CreatedAtAction("GetAbonentGroup", new { userId = userId, abonentId = abonentGroup.AbonentId, id = abonentGroup.GroupId }, abonentGroup);
                        }
                        return BadRequest("User -> Group not found");
                    }
                    return Conflict();
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }

        // DELETE: api/AbonentGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AbonentGroupDto>> DeleteAbonentGroup(int userId, int abonentId, int id)
        {
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                var abonent = _abonentService.GetAbonent(user, abonentId);
                if (abonent != null)
                {
                    var abonentGroup = abonent.Groups.FirstOrDefault(u => u.Id == id);
                    if (abonentGroup != null)
                    {
                        await _abonentGroupServiceService.DeleteAbonentGroupAsync(user, abonent, abonentGroup);
                        return new AbonentGroupDto() { AbonentId = abonent.Id, GroupId = abonentGroup.Id };
                    }
                    return NotFound();
                }
                return BadRequest("User -> Abonent not found");
            }
            return BadRequest("User not found");
        }
    }
}
