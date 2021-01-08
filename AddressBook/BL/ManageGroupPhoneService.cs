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

        public async Task UpdateGroupPhoneAsync(User user, int id, string name)
        {
            user.UpdateGroupPhone(id, name);
            await db.SaveChangesAsync();
        }
    }
}
