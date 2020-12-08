using AddressBook.DTO;
using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.BL
{
    public class ManageGroupService
    {
        private readonly AddressBookContext _groupContext;

        public ManageGroupService(AddressBookContext GroupContext)
        {
            _groupContext = GroupContext;
        }
        public async Task<Result<bool>> AddGroupAsync(GroupDto group)
        {

            // validate data
            var newGroup = Group.Create(group.Name);
            if (!newGroup.Succeeded)
            {
                return Result<bool>.Success(false);
            }
            // perform additional actions

            await _groupContext.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
