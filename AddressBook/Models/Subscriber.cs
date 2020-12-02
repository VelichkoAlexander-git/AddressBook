using System;
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
    public class Subscriber
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


        internal virtual ICollection<Phone> PhoneInternal { get; set; }
        internal virtual ICollection<Address> AddressInternal { get; set; }
        internal virtual ICollection<SubscriberGroup> GroupInternal { get; set; }

        protected Subscriber()
        {
            PhoneInternal = new List<Phone>();
            AddressInternal = new List<Address>();
            GroupInternal = new List<SubscriberGroup>();
        }

        public static Result<Subscriber> Create(string firstName, string middleName, string lastName, DateTime? dateOfBirth, byte[] photo, Sex sex, string mail)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(middleName) && string.IsNullOrEmpty(lastName)) errors.Add("Invalid customer name");


            if (errors.Any())
            {
                return Result<Subscriber>.Fail(errors);
            }

            var result = new Subscriber
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Photo = photo,
                Sex = sex,
                Mail = mail
            };

            return Result<Subscriber>.Success(result);
        }

        public Result<bool> AddPhone(GroupPhone groupPhone, string number)
        {
            var errors = new List<string>();

            if (Phones.Any(phone => phone.Number.Equals(number, StringComparison.OrdinalIgnoreCase))) errors.Add("This number already exists");

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            var result = Phone.Create(groupPhone, number);
            PhoneInternal.Add(result.Value);
            return Result<bool>.Success(true);
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

        public Result<bool> AddAddress(GroupAddress groupAddress, string information)
        {
            var errors = new List<string>();

            if (Addresses.Any(address => address.Information.Equals(information, StringComparison.OrdinalIgnoreCase))) errors.Add("This address already exists");

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            var result = Address.Create(groupAddress, information);
            AddressInternal.Add(result.Value);
            return Result<bool>.Success(true);
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

        public Result<bool> AddGroup(string name)
        {
            var errors = new List<string>();

            if (Groups.Any(group => group.Name.Equals(name, StringComparison.OrdinalIgnoreCase))) errors.Add("This group already exists");

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            var result = Group.Create(name);
            //GroupInternal.Add(result.Value);
            return Result<bool>.Success(true);
        }
        public Result<bool> RemoveGroup(Group groupToDelete)
        {
            var errors = new List<string>();

            if (groupToDelete is null) errors.Add(nameof(groupToDelete));
            if (Groups.All(group => !group.Name.Equals(groupToDelete.Name, StringComparison.OrdinalIgnoreCase))) errors.Add("Not exists");

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            //GroupInternal.Remove(groupToDelete);
            return Result<bool>.Success(true);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Name : {FirstName}");
            sb.AppendLine($"Surname : {MiddleName}");
            sb.AppendLine($"Patronymic : {LastName}");
            sb.AppendLine($"Phones : {Phones}");
            sb.AppendLine($"Addresses : {Addresses}");
            sb.AppendLine($"Group : {Groups}");
            sb.AppendLine($"Gender : {Sex}");
            sb.AppendLine($"DateBirth : {DateOfBirth}");
            sb.Append($"Mail : {Mail}");

            return sb.ToString();
        }
    }
}
