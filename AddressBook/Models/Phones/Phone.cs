using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    public class Phone
    {
        public int Id { get; protected set; }
        public int? GroupPhoneId { get; protected set; }
        public virtual GroupPhone GroupPhone { get; protected set; }
        public int AbonentId { get; protected set; }
        public virtual Abonent Abonent { get; protected set; }
        public string Number { get; protected set; }

        protected Phone()
        { }

        public static Result<Phone> Create(GroupPhone groupPhone, string number)
        {
            var errors = new List<string>();

            if (groupPhone is null) errors.Add(nameof(groupPhone));
            if (string.IsNullOrEmpty(number)) errors.Add("Number is required");

            if (errors.Any())
            {
                return Result<Phone>.Fail(errors);
            }

            var newPhone = new Phone
            {
                GroupPhone = groupPhone,
                Number = number
            };
            return Result<Phone>.Success(newPhone);
        }

        public override string ToString()
        {
            return string.Format($"Phone | Num : {GroupPhone}, {Number}");
        }
    }
}
