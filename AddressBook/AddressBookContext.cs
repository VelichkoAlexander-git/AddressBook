using AddressBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook
{
    public class AddressBookContext : DbContext
    {
        public AddressBookContext(DbContextOptions<AddressBookContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public virtual DbSet<User> Users { get; set; }
        //public virtual DbSet<Abonent> Abonents { get; set; }
        //public virtual DbSet<GroupPhone> GroupPhones { get; set; }
        //public virtual DbSet<GroupAddress> GroupAddresses { get; set; }
        //public virtual DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("UsersTable").HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasMany(u => u.AbonentInternal).WithOne(s => s.User).HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.GroupAddressInternal).WithOne(s => s.User).HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.GroupInternal).WithOne(s => s.User).HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.GroupPhoneInternal).WithOne(s => s.User).HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Abonent>().ToTable("AbonentsTable").HasKey(s => s.Id);
            modelBuilder.Entity<Abonent>().Ignore(s => s.Addresses);
            modelBuilder.Entity<Abonent>().Ignore(s => s.Phones);
            modelBuilder.Entity<Abonent>().Ignore(s => s.Groups);

            modelBuilder.Entity<Group>().ToTable("GroupsTable").HasKey(g => g.Id);
            modelBuilder.Entity<Group>().Ignore(g => g.Abonents);

            modelBuilder.Entity<AbonentGroup>().ToTable("AbonentsGroups").HasKey(sg => sg.Id);
            //modelBuilder.Entity<AbonentGroup>().ToTable("AbonentsGroups").HasKey(sg => new { sg.GroupId, sg.AbonentId });
            modelBuilder.Entity<AbonentGroup>().HasOne(sg => sg.Abonent).WithMany(s => s.GroupInternal).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<AbonentGroup>().HasOne(sg => sg.Group).WithMany(s => s.AbonentGroups).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GroupPhone>().ToTable("GroupPhonesTable").HasKey(gp => gp.Id);
            modelBuilder.Entity<GroupAddress>().ToTable("GroupAddressesTable").HasKey(ga => ga.Id);

            modelBuilder.Entity<Phone>().ToTable("PhoneTable").HasKey(p => p.Id);
            modelBuilder.Entity<Phone>().HasOne(a => a.GroupPhone).WithMany().HasForeignKey(ph => ph.GroupPhoneId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Address>().ToTable("AddressTable").HasKey(a => a.Id);
            modelBuilder.Entity<Address>().HasOne(a => a.GroupAddress).WithMany().HasForeignKey(a => a.GroupAddressId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Abonent>().HasMany(s => s.AddressInternal).WithOne(o => o.Abonent).HasForeignKey(o => o.AbonentId);
            modelBuilder.Entity<Abonent>().HasMany(s => s.PhoneInternal).WithOne(o => o.Abonent).HasForeignKey(o => o.AbonentId);
        }

        public Abonent GetAbonent(int UserId, int AbonentId)
        {
            var Abonent = Users.Find(UserId).AbonentInternal.ToList().Find(s => s.Id == AbonentId);
            Entry(Abonent).Collection(s => s.AddressInternal).Load();
            Entry(Abonent).Collection(s => s.PhoneInternal).Load();
            Entry(Abonent).Collection(s => s.GroupInternal).Load();
            return Abonent;
        }

        public User GetUser(int id)
        {
            var user = Users.Find(id);
            if (user != null)
            {
                Entry(user).Collection(s => s.AbonentInternal).Load();
                Entry(user).Collection(s => s.GroupAddressInternal).Load();
                Entry(user).Collection(s => s.GroupInternal).Load();
                Entry(user).Collection(s => s.GroupPhoneInternal).Load();
                return user;
            }
            return null;
        }

        public Result<bool> UpdateUser(int id, string login, string password)
        {
            var user = Users.FirstOrDefault(g => g.Id == id);
            if (user != null)
            {
                var updateResult = user.Update(login, password);
                if (!updateResult.Succeeded)
                {
                    return Result<bool>.Fail(updateResult.Errors);
                }
                return Result<bool>.Success(true);
            }
            return Result<bool>.Fail("User not found");
        }

        public virtual DbSet<AddressBook.Models.Abonent> Abonent { get; set; }

        public virtual DbSet<AddressBook.Models.GroupPhone> GroupPhone { get; set; }

        public virtual DbSet<AddressBook.Models.GroupAddress> GroupAddress { get; set; }

        public virtual DbSet<AddressBook.Models.Group> Group { get; set; }

        public DbSet<AddressBook.Models.Address> Address { get; set; }

        public DbSet<AddressBook.Models.Phone> Phone { get; set; }

        public DbSet<AddressBook.Models.AbonentGroup> AbonentGroup { get; set; }
    }
}
