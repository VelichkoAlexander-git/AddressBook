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
            var newGroup = Group.Create(name);
            if (!newGroup.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            user.AddGroup(newGroup.Value);

            await db.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task DeleteGroupAsync(User user, Group group)
        {
            user.RemoveGroup(group);

            await db.SaveChangesAsync();
        }

        public async Task UpdateGroupAsync(User user, int id, string name)
        {
            user.UpdateGroup(id, name);

            await db.SaveChangesAsync();
        }
    }
}
