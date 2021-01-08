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
    }
}
