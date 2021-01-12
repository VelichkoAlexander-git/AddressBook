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
            if (!user.GroupAddresses.Any(g => g.Name == name))
            {
                var newGroupAddress = GroupAddress.Create(name);
                if (newGroupAddress.Succeeded)
                {
                    user.AddGroupAddress(newGroupAddress.Value);
                    await db.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Fail(newGroupAddress.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such GroupAddress exists" });
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

        public async Task<Result<bool>> UpdateGroupAddressAsync(User user, int id, string name)
        {
            if (!user.GroupAddresses.Any(g => g.Name == name))
            {
                var answer = user.UpdateGroupAddress(id, name);
                if (answer.Succeeded)
                {
                    await db.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Fail(answer.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such GroupAddress exists" });
        }

        public Result<GroupAddressDto> GetGroupAddress(User user, string name)
        {
            var groupAddress = user.GroupAddresses.FirstOrDefault(u => u.Name == name);
            if (groupAddress != null)
            {
                return Result<GroupAddressDto>.Success(new GroupAddressDto() { Id = groupAddress.Id, UserId = groupAddress.UserId, Name = groupAddress.Name });
            }
            return Result<GroupAddressDto>.Fail(new string[] { "GroupAddress not found" });
        }
    }
}
