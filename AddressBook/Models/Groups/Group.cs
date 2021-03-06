﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    public class Group
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }

        public int UserId { get; protected set; }
        public virtual User User { get; protected set; }

        public IEnumerable<Abonent> Abonents => AbonentGroups.Select(sg => sg.Abonent);
        protected virtual ICollection<AbonentGroup> AbonentGroups { get; set; }
        protected Group()
        {
            AbonentGroups = new List<AbonentGroup>();
        }

        public static Result<Group> Create(string name)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(name)) { errors.Add(" Name is required"); }

            if (errors.Any())
            {
                return Result<Group>.Fail(errors);
            }

            var newGroup = new Group
            {
                Name = name
            };
            return Result<Group>.Success(newGroup);
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
            return string.Format($"Group : {Name}");
        }
    }
}
