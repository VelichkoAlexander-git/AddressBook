﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    public class GroupAddress
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public int UserId { get; protected set; }
        public virtual User User { get; protected set; }

        protected GroupAddress()
        { }

        public static Result<GroupAddress> Create(string name)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(name)) { errors.Add(" Name is required"); }

            if (errors.Any())
            {
                return Result<GroupAddress>.Fail(errors);
            }

            var newGroupAddress = new GroupAddress
            {
                Name = name
            };
            return Result<GroupAddress>.Success(newGroupAddress);
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
            return string.Format($"Group Addres : {Name}");
        }
    }
}
