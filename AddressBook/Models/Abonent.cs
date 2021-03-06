﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    public enum Sex
    {
        Undefined = 0,
        Female = 1,
        Male = 2
    }
    public class Abonent
    {
        public int Id { get; protected set; }
        public string FirstName { get; protected set; }
        public string MiddleName { get; protected set; }
        public string LastName { get; protected set; }
        public Sex Sex { get; protected set; }
        public DateTime? DateOfBirth { get; protected set; }
        public byte[] Photo { get; protected set; }
        public string Mail { get; protected set; }
        public IEnumerable<Phone> Phones => PhoneInternal;
        public IEnumerable<Address> Addresses => AddressInternal;
        public IEnumerable<Group> Groups => GroupInternal.Select(g => g.Group);

        public int UserId { get; protected set; }
        public virtual User User { get; protected set; }


        protected virtual ICollection<Phone> PhoneInternal { get; set; }
        protected virtual ICollection<Address> AddressInternal { get; set; }
        protected virtual ICollection<AbonentGroup> GroupInternal { get; set; }

        protected Abonent()
        {
            PhoneInternal = new List<Phone>();
            AddressInternal = new List<Address>();
            GroupInternal = new List<AbonentGroup>();
        }

        public static Result<Abonent> Create(string firstName, string middleName, string lastName, DateTime? dateOfBirth, byte[] photo, Sex sex, string mail)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(middleName) && string.IsNullOrEmpty(lastName)) errors.Add("Invalid customer name");


            if (errors.Any())
            {
                return Result<Abonent>.Fail(errors);
            }

            var result = new Abonent
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Photo = photo,
                Sex = sex,
                Mail = mail
            };

            return Result<Abonent>.Success(result);
        }

        public Result<bool> Update(string firstName, string middleName, string lastName, Sex sex, DateTime? dateOfBirth, byte[] photo, string mail)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(middleName) && string.IsNullOrEmpty(lastName)) errors.Add("Invalid customer name");

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Sex = sex;
            DateOfBirth = dateOfBirth;
            Photo = photo;
            Mail = mail;

            return Result<bool>.Success(true);
        }

        public Result<bool> AddPhone(Phone phone)
        {
            PhoneInternal.Add(phone);
            return Result<bool>.Success(true);
        }
        public Result<bool> UpdatePhone(int id, GroupPhone groupPhone, string number)
        {
            var phone = PhoneInternal.FirstOrDefault(g => g.Id == id);
            if (phone != null)
            {
                var updateResult = phone.Update(groupPhone, number);
                if (!updateResult.Succeeded)
                {
                    return Result<bool>.Fail(updateResult.Errors);
                }
                return Result<bool>.Success(true);
            }
            return Result<bool>.Fail("Phone not found");
        }
        public Result<bool> RemovePhone(Phone phoneToDelete)
        {
            var errors = new List<string>();

            if (phoneToDelete is null) errors.Add(nameof(phoneToDelete));
            if (Phones.All(phone => !phone.Number.Equals(phoneToDelete.Number, StringComparison.OrdinalIgnoreCase))) errors.Add("Not exists");

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            PhoneInternal.Remove(phoneToDelete);
            return Result<bool>.Success(true);
        }

        public Result<bool> AddAddress(Address address)
        {
            AddressInternal.Add(address);
            return Result<bool>.Success(true);
        }
        public Result<bool> UpdateAddress(int id, GroupAddress groupAddress, string information)
        {
            var address = AddressInternal.FirstOrDefault(g => g.Id == id);
            if (address != null)
            {
                var updateResult = address.Update(groupAddress, information);
                if (!updateResult.Succeeded)
                {
                    return Result<bool>.Fail(updateResult.Errors);
                }
                return Result<bool>.Success(true);
            }
            return Result<bool>.Fail("Address not found");
        }
        public Result<bool> RemoveAddress(Address addressToDelete)
        {
            var errors = new List<string>();

            if (addressToDelete is null) errors.Add(nameof(addressToDelete));
            if (Addresses.All(address => !address.Information.Equals(addressToDelete.Information, StringComparison.OrdinalIgnoreCase))) errors.Add("Not exists");

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            AddressInternal.Remove(addressToDelete);
            return Result<bool>.Success(true);
        }

        public Result<bool> AddAbonentGroup(AbonentGroup abonentGroup)
        {
            GroupInternal.Add(abonentGroup);
            return Result<bool>.Success(true);
        }
        public Result<bool> RemoveAbonentGroup(Group groupToDelete)
        {
            var errors = new List<string>();

            if (groupToDelete is null) errors.Add(nameof(groupToDelete));

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            var abonentGroup = GroupInternal.FirstOrDefault(ag => ag.Group == groupToDelete);
            if (abonentGroup != null)
            {
                GroupInternal.Remove(abonentGroup);
            }
            return Result<bool>.Success(true);
        }
        //public Result<bool> UpdateAbonentGroup(int id, Group group)
        //{
        //    var address = GroupInternal.FirstOrDefault(g => g.AbonentId == id);
        //    if (address != null)
        //    {
        //        var updateResult = address.Update(this, group);
        //        if (!updateResult.Succeeded)
        //        {
        //            return Result<bool>.Fail(updateResult.Errors);
        //        }
        //        return Result<bool>.Success(true);
        //    }
        //    return Result<bool>.Fail("Address not found");
        //}
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Name : {FirstName}");
            sb.Append($"Surname : {MiddleName}");
            sb.Append($"Patronymic : {LastName}");
            sb.Append($"Phones : {Phones}");
            sb.Append($"Addresses : {Addresses}");
            sb.Append($"Group : {Groups}");
            sb.Append($"Gender : {Sex}");
            sb.Append($"DateBirth : {DateOfBirth}");
            sb.Append($"Mail : {Mail}");

            return sb.ToString();
        }
    }
}
