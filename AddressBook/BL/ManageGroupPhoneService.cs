using AddressBook.DTO;
using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.BL
{
    public class ManageGroupPhoneService
    {
        private readonly AddressBookContext db;

        public ManageGroupPhoneService(AddressBookContext GroupContext)
        {
            db = GroupContext;
        }
        public async Task<Result<bool>> AddGroupPhoneAsync(User user, string name)
        {
            if (!user.Groups.Any(g => g.Name == name))
            {
                var newGroupPhone = GroupPhone.Create(name);
                if (newGroupPhone.Succeeded)
                {
                    user.AddGroupPhone(newGroupPhone.Value);
                    await db.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Fail(newGroupPhone.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such GroupPhone exists" });
        }

        public async Task<Result<bool>> DeleteGroupPhoneAsync(User user, GroupPhone groupPhone)
        {
            if (!user.Abonents.Any(a => a.Phones.Any(p => p.GroupPhone == groupPhone)))
            {
                user.RemoveGroupPhone(groupPhone);
                await db.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            return Result<bool>.Success(false);
        }

        public async Task<Result<bool>> UpdateGroupPhoneAsync(User user, int id, string name)
        {
            if (!user.Groups.Any(g => g.Name == name && g.Id != id))
            {
                var answer = user.UpdateGroupPhone(id, name);
                if (answer.Succeeded)
                {
                    await db.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Fail(answer.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such GroupPhone exists" });
        }

        public Result<GroupPhoneDto> GetGroupPhone(User user, string name)
        {
            var groupPhone = user.GroupPhones.FirstOrDefault(u => u.Name == name);
            if (groupPhone != null)
            {
                return Result<GroupPhoneDto>.Success(new GroupPhoneDto() { Id = groupPhone.Id, UserId = groupPhone.UserId, Name = groupPhone.Name });
            }
            return Result<GroupPhoneDto>.Fail(new string[] { "GroupPhone not found" });
        }
    }
}
