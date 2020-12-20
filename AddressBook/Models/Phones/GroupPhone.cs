using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    public class GroupPhone
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public int UserId { get; protected set; }
        public virtual User User { get; protected set; }

        protected GroupPhone()
        { }

        public static Result<GroupPhone> Create(string name)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(name)) { errors.Add(" Name is required"); }

            if (errors.Any())
            {
                return Result<GroupPhone>.Fail(errors);
            }

            var newGroupPhone = new GroupPhone
            {
                Name = name
            };

            return Result<GroupPhone>.Success(newGroupPhone);
        }

        public Result<bool> Update(string name)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(name)) { errors.Add("Name is required"); }

            if (errors.Any())
            {
                return Result<bool>.Fail(errors);
            }

            Name = name;

            return Result<bool>.Success(true);
        }

        public override string ToString()
        {
            return string.Format($"Group Phone : {Name}");
        }
    }
}
