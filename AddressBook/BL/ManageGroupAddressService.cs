using AddressBook.DTO;
using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.BL
{
    public class ManageGroupAddressService
    {
        private readonly AddressBookContext db;

        public ManageGroupAddressService(AddressBookContext GroupContext)
        {
            db = GroupContext;
        }
        public async Task<Result<bool>> AddGroupAddressAsync(GroupAddressDto groupAddress)
        {
            var newGroupAddress = GroupAddress.Create(groupAddress.Name);
            if (!newGroupAddress.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            var user = db.GetUser(groupAddress.UserId);
            user.AddGroupAddress(newGroupAddress.Value);

            await db.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
