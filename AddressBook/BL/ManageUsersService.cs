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
        public async Task<Result<bool>> AddUserAsync(string login, string password)
        {
            if (!db.Users.Any(u => u.Login == login))
            {
                // validate data
                var newUser = User.Register(login, password);
                if (newUser.Succeeded)
                {
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
                return Result<bool>.Fail(newUser.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such User exists" });
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

        public async Task<Result<bool>> UpdateUserAsync(User user, string login, string password)
        {
            if (!db.Users.Any(u => u.Login == login && u.Id != user.Id))
            {
                var updateResult = user.Update(login, password);
                if (updateResult.Succeeded)
                {
                    await db.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Fail(updateResult.Errors);
            }
            return Result<bool>.Fail(new string[] { "Such User exists" });
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
        public Result<UserDto> GetUser(string login, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            if (user != null)
            {
                return Result<UserDto>.Success(new UserDto()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Password = user.Password
                });
            }
            return Result<UserDto>.Fail(new string[] { "User not found" });
        }

        public async Task DeleteUserAsync(User user)
        {
            db.Users.Remove(user);
            await db.SaveChangesAsync();
        }
    }
}
