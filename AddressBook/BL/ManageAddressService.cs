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
            var newAddress = Address.Create(groupAddress, information);
            if (!newAddress.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            abonent.AddAddress(newAddress.Value);
            await db.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task DeleteAddressAsync(Abonent abonent, Address address)
        {
            abonent.RemoveAddress(address);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(Abonent abonent, int id, GroupAddress addressGroup, string information)
        {
            abonent.UpdateAddress(id, addressGroup, information);
            await db.SaveChangesAsync();
        }
    }
}
