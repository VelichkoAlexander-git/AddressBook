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
        public async Task<Result<bool>> AddGroupPhoneAsync(GroupPhoneDto groupPhone)
        {
            var newGroupPhone = GroupPhone.Create(groupPhone.Name);
            if (!newGroupPhone.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            var user = db.GetUser(groupPhone.UserId);
            user.AddGroupPhone(newGroupPhone.Value);

            await db.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
