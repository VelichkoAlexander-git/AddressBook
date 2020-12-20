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

        public async Task<Result<bool>> AddPhoneAsync(int userId, PhoneDto phoneDto)
        {
            var user = db.GetUser(userId);

            var phoneGroup = user.GroupPhoneInternal.Find(gp => gp.Id == phoneDto.GroupPhoneId);
            if (phoneGroup == null)
            {
                return Result<bool>.Success(false);
            }

            var newPhone = Phone.Create(phoneGroup, phoneDto.Number);
            if (!newPhone.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            var abonent = user.AbonentInternal.Find(a => a.Id == phoneDto.AbonentId);
            if (abonent != null)
            {
                abonent.AddPhone(newPhone.Value);
                await db.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            return Result<bool>.Success(false);
        }
    }
}
