using AddressBook.DTO;
using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.BL
{
    public class ManagePhonesService
    {
        private readonly AddressBookContext db;

        public ManagePhonesService(AddressBookContext GroupContext)
        {
            db = GroupContext;
        }

        public async Task<Result<bool>> AddPhoneAsync(Abonent abonent, GroupPhone phoneGroup, string number)
        {
            var newPhone = Phone.Create(phoneGroup, number);
            if (!newPhone.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            abonent.AddPhone(newPhone.Value);
            await db.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task DeletePhoneAsync(Abonent abonent, Phone phone)
        {
            abonent.RemovePhone(phone);

            await db.SaveChangesAsync();
        }

        public async Task UpdatePhoneAsync(Abonent abonent, int phoneId, GroupPhone groupPhone, string number)
        {
            abonent.UpdatePhone(phoneId, groupPhone, number);

            await db.SaveChangesAsync();
        }

        public Result<PhoneDto> GetPhone(Abonent abonent, GroupPhone phoneGroup, string number)
        {
            var address = abonent.Phones.FirstOrDefault(g => g.GroupPhoneId == phoneGroup.Id
                                                                 && g.Number == number);
            if (address != null)
            {
                return Result<PhoneDto>.Success(new PhoneDto()
                {
                    Id = address.Id,
                    AbonentId = address.AbonentId,
                    GroupPhoneId = address.GroupPhoneId,
                    Number = address.Number
                });
            }
            else
            {
                return Result<PhoneDto>.Fail(new string[] { "Address not found" });
            }
        }
    }
}
