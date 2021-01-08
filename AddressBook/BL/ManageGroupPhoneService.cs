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
            var newGroupPhone = GroupPhone.Create(name);
            if (!newGroupPhone.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            user.AddGroupPhone(newGroupPhone.Value);

            await db.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task DeleteGroupPhoneAsync(User user, GroupPhone groupPhone)
        {
            user.RemoveGroupPhone(groupPhone);
            await db.SaveChangesAsync();
        }

        internal Task UpdateGroupPhoneAsync(User user, int id, string name)
        {
            throw new NotImplementedException();
        }
    }
}
