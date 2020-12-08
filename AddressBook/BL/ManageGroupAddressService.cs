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
        private readonly AddressBookContext _groupAddressContext;

        public ManageGroupAddressService(AddressBookContext GroupAddressContext)
        {
            _groupAddressContext = GroupAddressContext;
        }
        public async Task<Result<bool>> AddGroupAddressAsync(int UserId, GroupAddressDto groupAddress)
        {

            // validate data

            // perform additional actions

            await _groupAddressContext.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
