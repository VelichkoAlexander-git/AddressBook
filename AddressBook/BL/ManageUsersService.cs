using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBook.DTO;
using AddressBook.Models;

namespace AddressBook.BL
{
    public class ManageUsersService
    {

        private readonly AddressBookContext _usersContext;

        public ManageUsersService(AddressBookContext usersContext)
        {
            _usersContext = usersContext;
        }
        public async Task<Result<bool>> AddUserAsync(UserDto user)
        {
            if (_usersContext.Users.Any(u => u.Login == user.Login))
            {
                return Result<bool>.Success(false);
            }
            // validate data
            var newUser = User.Register(user.Login, user.Password);
            if (!newUser.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            // perform additional actions

            _usersContext.Users.Add(newUser.Value);
            await _usersContext.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }

}
