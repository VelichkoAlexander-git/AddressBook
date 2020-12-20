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

        public async Task<Result<bool>> AddAddressAsync(int userId, AddressDto addressDto)
        {
            var user = db.GetUser(userId);

            var addressGroup = user.GroupAddressInternal.Find(gp => gp.Id == addressDto.GroupAddressId);
            if (addressGroup == null)
            {
                return Result<bool>.Success(false);
            }

            var newAddress = Address.Create(addressGroup, addressDto.Information);
            if (!newAddress.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            var abonent = user.AbonentInternal.Find(a => a.Id == addressDto.AbonentId);
            if (abonent != null)
            {
                abonent.AddAddress(newAddress.Value);
                await db.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            return Result<bool>.Success(false);
        }
    }
}
