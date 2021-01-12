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
        public async Task<Result<bool>> AddAbonentAsync(User user, string firstName, string middleName, string lastName, DateTime? dateOfBirth, byte[] photo, Sex sex, string mail)
        {
            var newAbonent = Abonent.Create(firstName,
                            middleName,
                            lastName,
                            dateOfBirth,
                            photo,
                            sex,
                            mail);
            if (!newAbonent.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            user.AddAbonent(newAbonent.Value);
            await db.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public Abonent GetAbonent(int UserId, int AbonentId)
        {
            var Abonent = db.Users.Find(UserId).Abonents.ToList().Find(s => s.Id == AbonentId);
            db.Entry(Abonent).Collection(s => s.Addresses).Load();
            db.Entry(Abonent).Collection(s => s.Phones).Load();
            db.Entry(Abonent).Collection(s => s.Groups).Load();
            return Abonent;
        }

        public async Task DeleteAbonentAsync(User user, Abonent abonent)
        {
            user.RemoveAbonent(abonent);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAbonentAsync(User user, int id, string firstName, string middleName, string lastName, DateTime? dateOfBirth, byte[] photo, Sex sex, string mail)
        {
            user.UpdateAbonent(id, firstName, middleName, lastName, sex, dateOfBirth, photo, mail);
            await db.SaveChangesAsync();
        }

        public Abonent GetAbonent(User user, int id)
        {
            var abonent = user.Abonents.FirstOrDefault(a => a.Id == id);
            if (abonent != null)
            {
                db.Entry(abonent).Collection(s => s.Phones).Load();
                db.Entry(abonent).Collection(s => s.Addresses).Load();
                db.Entry(abonent).Collection("GroupInternal").Load();
                return abonent;
            }
            return null;
        }

        public Result<AbonentDto> GetAbonent(User user,string firstName, string middleName, string lastName)
        {
            var abonent = user.Abonents.FirstOrDefault(g => g.FirstName == firstName
                                                                 && g.MiddleName == middleName
                                                                 && g.LastName == lastName);
            if (abonent != null)
            {
                return Result<AbonentDto>.Success(new AbonentDto()
                {
                    Id = abonent.Id,
                    UserId = abonent.UserId,
                    FirstName = abonent.FirstName,
                    MiddleName = abonent.MiddleName,
                    LastName = abonent.LastName,
                    DateOfBirth = abonent.DateOfBirth,
                    Photo = abonent.Photo,
                    Sex = abonent.Sex,
                    Mail = abonent.Mail
                });
            }
            else
            {
                return Result<AbonentDto>.Fail(new string[] { "Abonent not found" });
            }
        }
    }
}
