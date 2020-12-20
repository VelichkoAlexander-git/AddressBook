using AddressBook.DTO;
using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.BL
{
    public class ManageAbonentGroupService
    {
        private readonly AddressBookContext db;

        public ManageAbonentGroupService(AddressBookContext GroupContext)
        {
            db = GroupContext;
        }
        public async Task<Result<bool>> AddAbonentGroupAsync(int userId, AbonentGroupDto abonentGroupDto)
        {
            var user = db.GetUser(userId);

            var group = user.GroupInternal.Find(g => g.Id == abonentGroupDto.GroupId);
            if (group == null)
            {
                return Result<bool>.Success(false);
            }

            var abonent = user.AbonentInternal.Find(a => a.Id == abonentGroupDto.AbonentId);
            if (abonent == null)
            {
                return Result<bool>.Success(false);
            }

            var newAddress = AbonentGroup.Create(abonent, group);
            if (!newAddress.Succeeded)
            {
                return Result<bool>.Success(false);
            }

                abonent.AddGroup(newAddress.Value);
                await db.SaveChangesAsync();

                return Result<bool>.Success(true);
        }
    }
}
