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
        public async Task<Result<bool>> AddGroupAsync(GroupDto group)
        {
            var newGroup = Group.Create(group.Name);
            if (!newGroup.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            var user = db.GetUser(group.UserId);
            var item = user.AddGroup(newGroup.Value);

            await db.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
