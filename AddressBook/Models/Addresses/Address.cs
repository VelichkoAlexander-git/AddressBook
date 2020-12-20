using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    public class Address
    {
        public int Id { get; protected set; }
        public int? GroupAddressId { get; protected set; }
        public virtual GroupAddress GroupAddress { get; protected set; }
        public int AbonentId { get; protected set; }
        public virtual Abonent Abonent { get; protected set; }
        public string Information { get; protected set; }

        protected Address()
        { }

        public static Result<Address> Create(GroupAddress groupAddress, string information)
        {
            var errors = new List<string>();

            if (groupAddress is null) errors.Add(nameof(groupAddress));
            if (string.IsNullOrEmpty(information)) errors.Add("Information is required");

            if (errors.Any())
            {
                return Result<Address>.Fail(errors);
            }

            var newAddress = new Address
            {
                GroupAddress = groupAddress,
                Information = information
            };
            return Result<Address>.Success(newAddress);
        }
        public override string ToString()
        {
            return string.Format($"Address | Info : {Information}, {GroupAddress}");
        }
    }
}
