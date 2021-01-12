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

            abonent.AddAbonentGroup(newAddress.Value);
            await db.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAbonentGroupAsync(User user, Abonent abonent, Group abonentGroup)
        {
            if (!user.Abonents.Any(a => a.Groups == abonentGroup))
            {
                abonent.RemoveAbonentGroup(abonentGroup);
                await db.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            return Result<bool>.Success(false);
        }

        public Result<AbonentGroupDto> GetAbonentGroup(User user, Abonent abonent, int groupId)
        {
            var groupAbonent = user.Groups.FirstOrDefault(g => g.Id == groupId && g.Abonents == abonent);
            if (groupAbonent != null)
            {
                return Result<AbonentGroupDto>.Success(new AbonentGroupDto()
                {
                    AbonentId = abonent.Id,
                    GroupId = groupAbonent.Id
                });
            }
            else
            {
                return Result<AbonentGroupDto>.Fail(new string[] { "Abonent Group not found" });
            }
        }
    }
}
