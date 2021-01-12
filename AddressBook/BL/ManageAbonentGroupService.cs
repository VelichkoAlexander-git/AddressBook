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
            if (!abonent.Groups.Any(p => p.Id == group.Id))
            {
                var newAddress = AbonentGroup.Create(abonent, group);
                if (newAddress.Succeeded)
                {
                    abonent.AddAbonentGroup(newAddress.Value);
                    await db.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Fail(newAddress.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such AbonentGroup exists" });
        }

        public async Task<Result<bool>> DeleteAbonentGroupAsync(User user, Abonent abonent, Group abonentGroup)
        {
            if (!user.Abonents.Any(a => a.Groups == abonentGroup))
            {
                abonent.RemoveAbonentGroup(abonentGroup);
                await db.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            return Result<bool>.Fail(new string[] { "AbbonentGroup not found" });
        }
    }
}
