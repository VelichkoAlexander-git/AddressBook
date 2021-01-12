using AddressBook.DTO;
using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.BL
{
    public class ManageAddressService
    {
        private readonly AddressBookContext db;

        public ManageAddressService(AddressBookContext GroupContext)
        {
            db = GroupContext;
        }

        public async Task<Result<bool>> AddAddressAsync(Abonent abonent, GroupAddress groupAddress, string information)
        {
            if (!abonent.Addresses.Any(p => p.Information == information))
            {
                var newAddress = Address.Create(groupAddress, information);
                if (newAddress.Succeeded)
                {
                    abonent.AddAddress(newAddress.Value);
                    await db.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Fail(newAddress.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such Address exists" });
        }

        public async Task DeleteAddressAsync(Abonent abonent, Address address)
        {
            abonent.RemoveAddress(address);
            await db.SaveChangesAsync();
        }

        public async Task<Result<bool>> UpdateAddressAsync(Abonent abonent, int id, GroupAddress addressGroup, string information)
        {
            if (!abonent.Addresses.Any(p => p.Information == information))
            {
                var answer = abonent.UpdateAddress(id, addressGroup, information);
                if (answer.Succeeded)
                {
                    await db.SaveChangesAsync();
                }
                return Result<bool>.Fail(answer.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such Address exists" });
        }

        public Result<AddressDto> GetAddress(Abonent abonent, GroupAddress addressGroup, string information)
        {
            var address = abonent.Addresses.FirstOrDefault(g => g.GroupAddressId == addressGroup.Id
                                                                 && g.Information == information);
            if (address != null)
            {
                return Result<AddressDto>.Success(new AddressDto()
                {
                    Id = address.Id,
                    AbonentId = address.AbonentId,
                    GroupAddressId = address.GroupAddressId,
                    Information = address.Information
                });
            }
            return Result<AddressDto>.Fail(new string[] { "Address not found" });
        }
    }
}
