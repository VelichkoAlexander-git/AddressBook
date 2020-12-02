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
        }
        public virtual DbSet<User> Users { get; set; }
        //public virtual DbSet<Subscriber> Subscribers { get; set; }
        //public virtual DbSet<GroupPhone> GroupPhones { get; set; }
        //public virtual DbSet<GroupAddress> GroupAddresses { get; set; }
        //public virtual DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("UsersTable").HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasMany(u => u.SubscriberInternal).WithOne(s => s.User).HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.GroupAddressInternal).WithOne(s => s.User).HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.GroupInternal).WithOne(s => s.User).HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.GroupPhoneInternal).WithOne(s => s.User).HasForeignKey(s => s.UserId);

            modelBuilder.Entity<GroupPhone>().ToTable("GroupPhonesTable").HasKey(gp => gp.Id);
            modelBuilder.Entity<GroupAddress>().ToTable("GroupAddressesTable").HasKey(ga => ga.Id);
            modelBuilder.Entity<Group>().ToTable("GroupsTable").HasKey(g => g.Id);

            modelBuilder.Entity<Phone>().ToTable("PhoneTable").HasKey(p => p.Id);
            modelBuilder.Entity<Phone>().HasOne(a => a.GroupPhone).WithMany().HasForeignKey(ph => ph.GroupPhoneId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Address>().ToTable("AddressTable").HasKey(a => a.Id);
            modelBuilder.Entity<Address>().HasOne(a => a.GroupAddress).WithMany().HasForeignKey(a => a.GroupAddressId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<SubscriberGroup>().ToTable("SubscribersGroups").HasKey(sg => new { sg.GroupId, sg.SubscriberId });
            modelBuilder.Entity<SubscriberGroup>().HasOne(sg => sg.Subscriber).WithMany(s => s.GroupInternal).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<SubscriberGroup>().HasOne(sg => sg.Group).WithMany(s => s.SubscriberGroups).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Subscriber>().ToTable("SubscribersTable").HasKey(s => s.Id);
            modelBuilder.Entity<Subscriber>().Ignore(s => s.Addresses);
            modelBuilder.Entity<Subscriber>().Ignore(s => s.Phones);
            modelBuilder.Entity<Subscriber>().Ignore(s => s.Groups);

            modelBuilder.Entity<Group>().Ignore(g => g.Subscribers);

            modelBuilder.Entity<Subscriber>().HasMany(s => s.AddressInternal).WithOne(o => o.Subscriber).HasForeignKey(o => o.SubscriberId);
            modelBuilder.Entity<Subscriber>().HasMany(s => s.PhoneInternal).WithOne(o => o.Subscriber).HasForeignKey(o => o.SubscriberId);
        }

        public Subscriber GetSubscriber(int UserId, int SubscriberId)
        {
            var subscriber = Users.Find(UserId).SubscriberInternal.ToList().Find(s => s.Id == SubscriberId);
            Entry(subscriber).Collection(s => s.AddressInternal).Load();
            Entry(subscriber).Collection(s => s.PhoneInternal).Load();
            Entry(subscriber).Collection(s => s.GroupInternal).Load();
            return subscriber;
        }

        public User GetUser(int id)
        {
            var user = Users.Find(id);
            if (user != null)
            {
                Entry(user).Collection(s => s.SubscriberInternal).Load();
                Entry(user).Collection(s => s.GroupAddressInternal).Load();
                Entry(user).Collection(s => s.GroupInternal).Load();
                Entry(user).Collection(s => s.GroupPhoneInternal).Load();
                return user;
            }
            return null;
        }

        public DbSet<AddressBook.Models.Subscriber> Subscriber { get; set; }

        public DbSet<AddressBook.Models.GroupPhone> GroupPhone { get; set; }

        public DbSet<AddressBook.Models.GroupAddress> GroupAddress { get; set; }

        public DbSet<AddressBook.Models.Group> Group { get; set; }
    }
}
