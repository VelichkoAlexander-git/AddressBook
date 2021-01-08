using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBook.DTO;
using AddressBook.Models;

namespace AddressBook.BL
{
    public class ManageUsersService
    {

        private readonly AddressBookContext db;

        public ManageUsersService(AddressBookContext usersContext)
        {
            db = usersContext;
        }
        public async Task<Result<bool>> AddUserAsync(UserDto user)
        {
            if (db.Users.Any(u => u.Login == user.Login))
            {
                return Result<bool>.Success(false);
            }
            // validate data
            var newUser = User.Register(user.Login, user.Password);
            if (!newUser.Succeeded)
            {
                return Result<bool>.Success(false);
            }

            // perform additional actions
            CreateGroup(newUser.Value, "Семья");
            CreateGroup(newUser.Value, "Рабочий");
            CreateGroup(newUser.Value, "Друзья");
            CreateGroup(newUser.Value, "Общие");

            CreateGroupPhone(newUser.Value, "Домашний");
            CreateGroupPhone(newUser.Value, "Рабочий");
            CreateGroupPhone(newUser.Value, "Мобильный");

            CreateGroupAddress(newUser.Value, "Домашний");
            CreateGroupAddress(newUser.Value, "Рабочий");

            db.Users.Add(newUser.Value);
            await db.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        private Result<bool> CreateGroup(User user, string str)
        {
            Result<Group> group;
            group = Group.Create(str);
            if (group.Succeeded)
            {
                if (user.AddGroup(group.Value).Succeeded)
                {
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Success(false);
            }
            return Result<bool>.Success(false);
        }

        private Result<bool> CreateGroupAddress(User user, string str)
        {
            Result<GroupAddress> group;
            group = GroupAddress.Create(str);
            if (group.Succeeded)
            {
                if (user.AddGroupAddress(group.Value).Succeeded)
                {
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Success(false);
            }
            return Result<bool>.Success(false);
        }

        private Result<bool> CreateGroupPhone(User user, string str)
        {
            Result<GroupPhone> group;
            group = GroupPhone.Create(str);
            if (group.Succeeded)
            {
                if (user.AddGroupPhone(group.Value).Succeeded)
                {
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Success(false);
            }
            return Result<bool>.Success(false);
        }

        public Result<bool> UpdateUserAsync(User user, string login, string password)
        {
            var updateResult = user.Update(login, password);
            if (!updateResult.Succeeded)
            {
                return Result<bool>.Fail(updateResult.Errors);
            }
            return Result<bool>.Success(true);
        }

        public User GetUser(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                db.Entry(user).Collection(s => s.Abonents).Load();
                db.Entry(user).Collection(s => s.GroupAddresses).Load();
                db.Entry(user).Collection(s => s.Groups).Load();
                db.Entry(user).Collection(s => s.GroupPhones).Load();
                return user;
            }
            return null;
        }
    }
}
