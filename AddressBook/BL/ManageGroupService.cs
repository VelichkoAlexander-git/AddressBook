using AddressBook.DTO;
using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.BL
{
    public class ManageGroupService
    {
        private readonly AddressBookContext db;

        public ManageGroupService(AddressBookContext GroupContext)
        {
            db = GroupContext;
        }
        public async Task<Result<bool>> AddGroupAsync(User user, string name)
        {
            if (!user.Groups.Any(g => g.Name == name))
            {
                var newGroup = Group.Create(name);
                if (newGroup.Succeeded)
                {
                    user.AddGroup(newGroup.Value);
                    await db.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Fail(newGroup.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such Group exists" });
        }

        public async Task DeleteGroupAsync(User user, Group group)
        {
            user.RemoveGroup(group);

            await db.SaveChangesAsync();
        }

        public async Task<Result<bool>> UpdateGroupAsync(User user, int id, string name)
        {
            if (!user.Groups.Any(g => g.Name == name))
            {
                var answer = user.UpdateGroup(id, name);
                if (answer.Succeeded)
                {
                    await db.SaveChangesAsync();
                }
                return Result<bool>.Fail(answer.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such Groups exists" });
        }

        public Result<GroupDto> GetGroup(User user, string name)
        {
            var group = user.Groups.FirstOrDefault(u => u.Name == name);
            if (group != null)
            {
                return Result<GroupDto>.Success(new GroupDto() { Id = group.Id, UserId = group.UserId, Name = group.Name });
            }
            return Result<GroupDto>.Fail(new string[] { "Group not found" });
        }
    }
}
