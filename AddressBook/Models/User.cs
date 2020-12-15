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
            SubscriberInternal = new List<Subscriber>();
            GroupPhoneInternal = new List<GroupPhone>();
            GroupAddressInternal = new List<GroupAddress>();
            GroupInternal = new List<Group>();
        }
        public static Result<User> Register(string login, string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(login)) { errors.Add("Логин не может быть пустым"); }
            if (string.IsNullOrEmpty(password)) { errors.Add("Пароль не может быть пустым"); }

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
        #endregion

        #region Subscriber
        internal List<Subscriber> SubscriberInternal { get; set; }
        public IEnumerable<Subscriber> Subscribers => SubscriberInternal;
        public Result<bool> AddSubscriber(string firstName, string middleName, string lastName, DateTime? dateOfBirth, byte[] photo, Sex sex, string mail)
        {
            var errors = new List<string>();

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            var result = Subscriber.Create(firstName, middleName, lastName, dateOfBirth, photo, sex, mail);
            SubscriberInternal.Add(result.Value);
            return Result<bool>.Success(true);
        }

        public Result<bool> RemoveSubscriber(Subscriber subscriberToDelete)
        {
            var errors = new List<string>();

            if (subscriberToDelete is null) errors.Add(nameof(subscriberToDelete));

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            SubscriberInternal.Remove(subscriberToDelete);
            return Result<bool>.Success(true);
        }


        #endregion

        #region GroupAddress
        internal List<GroupAddress> GroupAddressInternal { get; set; }
        public IEnumerable<GroupAddress> GroupAddresses => GroupAddressInternal;
        public Result<bool> AddGroupAddress(string name)
        {
            var errors = new List<string>();

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            var result = GroupAddress.Create(name);
            GroupAddressInternal.Add(result.Value);
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
        #endregion

        #region GroupPhone
        internal List<GroupPhone> GroupPhoneInternal { get; set; }
        public IEnumerable<GroupPhone> GroupPhones => GroupPhoneInternal;
        public Result<bool> AddGroupPhone(string name)
        {
            var errors = new List<string>();

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            var result = GroupPhone.Create(name);
            GroupPhoneInternal.Add(result.Value);
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
        #endregion

        #region Group
        internal List<Group> GroupInternal { get; set; }
        public IEnumerable<Group> Groups => GroupInternal;
        public Result<bool> AddGroup(string name)
        {
            var errors = new List<string>();

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            var result = Group.Create(name);
            GroupInternal.Add(result.Value);
            return Result<bool>.Success(true);
        }
        public Result<bool> AddGroup(Group group)
        {
            GroupInternal.Add(group);

            return Result<bool>.Success(true);
        }

        public Result<bool> UpdateGroup(int id, string name)
        {
            //GroupInternal.Find(g => g.Id == id).Name = name;

            return Result<bool>.Success(true);
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

        public Result<bool> RemoveGroup(int id)
        {
            var errors = new List<string>();

            if (!GroupInternal.Any(g => g.Id == id)) errors.Add(nameof(Group));

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            var item = GroupInternal.Find(u => u.Id == id);
            GroupInternal.Remove(item);
            return Result<bool>.Success(true);
        }
        #endregion
    }
}
