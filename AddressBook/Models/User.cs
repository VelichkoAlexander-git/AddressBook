using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    public class User
    {
        #region User
        public int Id { get; protected set; }
        public string Login { get; protected set; }
        public string Password { get; protected set; }

        protected User()
        {
            AbonentInternal = new List<Abonent>();
            GroupPhoneInternal = new List<GroupPhone>();
            GroupAddressInternal = new List<GroupAddress>();
            GroupInternal = new List<Group>();
        }
        public static Result<User> Register(string login, string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(login)) { errors.Add("Login is required"); }
            if (string.IsNullOrEmpty(password)) { errors.Add("Password is required"); }

            if (errors.Any())
            {
                return Result<User>.Fail(errors);
            }
            var newUser = new User()
            {
                Login = login,
                Password = password
            };

            return Result<User>.Success(newUser);
        }
        public Result<bool> Update(string login, string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(login)) { errors.Add("Login is required"); }
            if (string.IsNullOrEmpty(password)) { errors.Add("Password is required"); }

            if (!errors.Any())
            {
                Login = login;
                Password = password;
                return Result<bool>.Success(true);
            }
            return Result<bool>.Fail(errors);
        }
        #endregion

        #region Abonent
        protected virtual List<Abonent> AbonentInternal { get; set; }
        public IEnumerable<Abonent> Abonents => AbonentInternal;

        public Result<bool> AddAbonent(Abonent abonent)
        {
            AbonentInternal.Add(abonent);
            return Result<bool>.Success(true);
        }

        public Result<bool> RemoveAbonent(Abonent AbonentToDelete)
        {
            var errors = new List<string>();

            if (AbonentToDelete is null) errors.Add(nameof(AbonentToDelete));

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            AbonentInternal.Remove(AbonentToDelete);
            return Result<bool>.Success(true);
        }

        public Result<bool> UpdateAbonent(int id, string firstName, string middleName, string lastName, Sex sex, DateTime? dateOfBirth, byte[] photo, string mail)
        {
            var Abonent = AbonentInternal.FirstOrDefault(g => g.Id == id);
            if (Abonent != null)
            {
                var updateResult = Abonent.Update(firstName, middleName, lastName, sex, dateOfBirth, photo, mail);
                if (!updateResult.Succeeded)
                {
                    return Result<bool>.Fail(updateResult.Errors);
                }
                return Result<bool>.Success(true);
            }
            return Result<bool>.Fail("Abonent not found");
        }
        #endregion

        #region GroupAddress
        protected virtual List<GroupAddress> GroupAddressInternal { get; set; }
        public IEnumerable<GroupAddress> GroupAddresses => GroupAddressInternal;

        public Result<bool> AddGroupAddress(GroupAddress groupAddress)
        {
            GroupAddressInternal.Add(groupAddress);
            return Result<bool>.Success(true);
        }

        public Result<bool> RemoveGroupAddress(GroupAddress groupAddressToDelete)
        {
            var errors = new List<string>();

            if (groupAddressToDelete is null) errors.Add(nameof(groupAddressToDelete));

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            GroupAddressInternal.Remove(groupAddressToDelete);
            return Result<bool>.Success(true);
        }

        public Result<bool> UpdateGroupAddress(int id, string name)
        {
            var groupAddress = GroupAddressInternal.FirstOrDefault(g => g.Id == id);
            if (groupAddress != null)
            {
                var updateResult = groupAddress.Update(name);
                if (!updateResult.Succeeded)
                {
                    return Result<bool>.Fail(updateResult.Errors);
                }
                return Result<bool>.Success(true);
            }
            return Result<bool>.Fail("Group address not found");
        }
        #endregion

        #region GroupPhone
        protected virtual List<GroupPhone> GroupPhoneInternal { get; set; }
        public IEnumerable<GroupPhone> GroupPhones => GroupPhoneInternal;

        public Result<bool> AddGroupPhone(GroupPhone groupPhone)
        {
            GroupPhoneInternal.Add(groupPhone);

            return Result<bool>.Success(true);
        }

        public Result<bool> RemoveGroupPhone(GroupPhone groupPhoneToDelete)
        {
            var errors = new List<string>();

            if (groupPhoneToDelete is null) errors.Add(nameof(groupPhoneToDelete));

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            GroupPhoneInternal.Remove(groupPhoneToDelete);
            return Result<bool>.Success(true);
        }

        public Result<bool> UpdateGroupPhone(int id, string name)
        {
            var groupPhone = GroupPhoneInternal.FirstOrDefault(g => g.Id == id);
            if (groupPhone != null)
            {
                var updateResult = groupPhone.Update(name);
                if (!updateResult.Succeeded)
                {
                    return Result<bool>.Fail(updateResult.Errors);
                }
                return Result<bool>.Success(true);
            }
            return Result<bool>.Fail("Group phone not found");
        }
        #endregion

        #region Group
        protected virtual List<Group> GroupInternal { get; set; }
        public IEnumerable<Group> Groups => GroupInternal;

        public Result<bool> AddGroup(Group group)
        {
            GroupInternal.Add(group);

            return Result<bool>.Success(true);
        }

        public Result<bool> UpdateGroup(int id, string name)
        {
            var group = GroupInternal.FirstOrDefault(g => g.Id == id);
            if (group != null)
            {
                var updateResult = group.Update(name);
                if (!updateResult.Succeeded)
                {
                    return Result<bool>.Fail(updateResult.Errors);
                }
                return Result<bool>.Success(true);
            }
            return Result<bool>.Fail("Group not found");
        }

        public Result<bool> RemoveGroup(Group groupToDelete)
        {
            var errors = new List<string>();

            if (groupToDelete is null) errors.Add(nameof(groupToDelete));

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            GroupInternal.Remove(groupToDelete);
            return Result<bool>.Success(true);
        }
        #endregion
    }
}
