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
            if (!abonent.Phones.Any(p => p.Number == number))
            {
                var newPhone = Phone.Create(phoneGroup, number);
                if (newPhone.Succeeded)
                {
                    abonent.AddPhone(newPhone.Value);
                    await db.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Fail(newPhone.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such Phone exists" });
        }

        public async Task DeletePhoneAsync(Abonent abonent, Phone phone)
        {
            abonent.RemovePhone(phone);

            await db.SaveChangesAsync();
        }

        public async Task<Result<bool>> UpdatePhoneAsync(Abonent abonent, int phoneId, GroupPhone groupPhone, string number)
        {
            if (!abonent.Phones.Any(p => p.Number == number))
            {
                var answer = abonent.UpdatePhone(phoneId, groupPhone, number);
                if (answer.Succeeded)
                {
                    await db.SaveChangesAsync();
                }
                return Result<bool>.Fail(answer.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such Phone exists" });
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
            return Result<PhoneDto>.Fail(new string[] { "Phone not found" });
        }
    }
}
