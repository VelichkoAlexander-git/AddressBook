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

        public static Result<AbonentGroup> Create(Abonent abonent, Group group)
        {
            var errors = new List<string>();

            if (abonent is null) errors.Add(nameof(abonent));
            if (group is null) errors.Add(nameof(group));

            if (errors.Any())
            {
                return Result<AbonentGroup>.Fail(errors);
            }

            var newAbonentGroup = new AbonentGroup
            {
                Group = group,
                GroupId = group.Id,
                Abonent = abonent,
                AbonentId = abonent.Id
            };
            return Result<AbonentGroup>.Success(newAbonentGroup);
        }

        public Result<bool> Update(Abonent abonent, Group group)
        {
            var errors = new List<string>();

            if (abonent is null) errors.Add(nameof(abonent));
            if (group is null) errors.Add(nameof(group));

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            Group = group;
            GroupId = group.Id;
            Abonent = abonent;
            AbonentId = abonent.Id;

            return Result<bool>.Success(true);
        }
    }
}
