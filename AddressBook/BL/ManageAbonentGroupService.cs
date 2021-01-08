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
        public async Task<Result<bool>> AddAbonentGroupAsync(Abonent abonent, Group group)
        {
            var newAddress = AbonentGroup.Create(abonent, group);
            if (!newAddress.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            abonent.AddGroup(newAddress.Value);
            await db.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task DeleteAbonentGroupAsync(Abonent abonent, Group abonentGroup)
        {
            abonent.RemoveGroup(abonentGroup);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAbonentGroupAsync(Abonent abonent, int id, Group group)
        {
            abonent.UpdateAbonentGroup(id, group);
            await db.SaveChangesAsync();
        }
    }
}
