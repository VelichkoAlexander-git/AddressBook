using AddressBook.DTO;
using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.BL
{
    public class ManageAbonentService
    {
        private readonly AddressBookContext db;

        public ManageAbonentService(AddressBookContext GroupContext)
        {
            db = GroupContext;
        }
        public async Task<Result<bool>> AddAbonentAsync(AbonentDto abonentDto)
        {
            var user = db.GetUser(abonentDto.UserId);
            if (user != null)
            {
                var newAbonent = Abonent.Create(abonentDto.FirstName,
                                abonentDto.MiddleName,
                                abonentDto.LastName,
                                abonentDto.DateOfBirth,
                                abonentDto.Photo,
                                abonentDto.Sex,
                                abonentDto.Mail);
                
                if (newAbonent.Succeeded)
                {
                    user.AddAbonent(newAbonent.Value);
                    await db.SaveChangesAsync();

                    return Result<bool>.Success(true);
                }
                return Result<bool>.Success(false);
            }
            return Result<bool>.Success(false);
        }
    }
}
