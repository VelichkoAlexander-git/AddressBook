using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    public class AbonentGroup
    {
        public int Id { get; protected set; }
        public int AbonentId { get; protected set; }
        public virtual Abonent Abonent { get; protected set; }
        public int GroupId { get; protected set; }
        public virtual Group Group { get; protected set; }

        public static Result<AbonentGroup> Create(int abonentId, int groupId)
        {
            var errors = new List<string>();

            if (abonentId < 0) errors.Add("abonentId < 0");
            if (groupId < 0) errors.Add("groupId < 0");

            if (errors.Any())
            {
                return Result<AbonentGroup>.Fail(errors);
            }

            var newAbonentGroup = new AbonentGroup
            {
                AbonentId = abonentId,
                GroupId = groupId
            };
            return Result<AbonentGroup>.Success(newAbonentGroup);
        }
    }
}
