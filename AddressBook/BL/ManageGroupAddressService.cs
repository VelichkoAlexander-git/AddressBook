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
        public async Task<Result<bool>> AddGroupAddressAsync(User user, string name)
        {
            var newGroupAddress = GroupAddress.Create(name);
            if (!newGroupAddress.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            user.AddGroupAddress(newGroupAddress.Value);
            await db.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteGroupAddressAsync(User user, GroupAddress groupAddress)
        {
            if (!user.Abonents.Any(a => a.Addresses.Any(p => p.GroupAddress == groupAddress)))
            {
                user.RemoveGroupAddress(groupAddress);
                await db.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            return Result<bool>.Success(false);
        }

        public async Task UpdateGroupAddressAsync(User user, int id, string name)
        {
            user.UpdateGroupAddress(id, name);
            await db.SaveChangesAsync();
        }

        public Result<GroupAddressDto> GetGroupAddress(User user, string name)
        {
            var groupAddress = user.GroupAddresses.FirstOrDefault(u => u.Name == name);
            if (groupAddress != null)
            {
                return Result<GroupAddressDto>.Success(new GroupAddressDto() { Id = groupAddress.Id, UserId = groupAddress.UserId, Name = groupAddress.Name });
            }
            else
            {
                return Result<GroupAddressDto>.Fail(new string[] { "Group Address not found" });
            }
        }
    }
}
