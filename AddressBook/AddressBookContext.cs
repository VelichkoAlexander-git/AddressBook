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
            modelBuilder.Entity<User>().HasMany(u => u.Abonents).WithOne(s => s.User).HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.GroupAddresses).WithOne(s => s.User).HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.Groups).WithOne(s => s.User).HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.GroupPhones).WithOne(s => s.User).HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Abonent>().ToTable("AbonentsTable").HasKey(s => s.Id);
            modelBuilder.Entity<Abonent>().Ignore(s => s.Addresses);
            modelBuilder.Entity<Abonent>().Ignore(s => s.Phones);
            modelBuilder.Entity<Abonent>().Ignore(s => s.Groups);

            modelBuilder.Entity<Group>().ToTable("GroupsTable").HasKey(g => g.Id);
            modelBuilder.Entity<Group>().Ignore(g => g.Abonents);

            modelBuilder.Entity<AbonentGroup>().ToTable("AbonentsGroups").HasKey(sg => sg.Id);
            modelBuilder.Entity<AbonentGroup>().HasOne(sg => sg.Abonent).WithMany("Groups").HasForeignKey(sg => sg.GroupId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<AbonentGroup>().HasOne(sg => sg.Group).WithMany("Abonents").HasForeignKey(sg => sg.AbonentId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GroupPhone>().ToTable("GroupPhonesTable").HasKey(gp => gp.Id);
            modelBuilder.Entity<GroupAddress>().ToTable("GroupAddressesTable").HasKey(ga => ga.Id);

            modelBuilder.Entity<Phone>().ToTable("PhoneTable").HasKey(p => p.Id);
            modelBuilder.Entity<Phone>().HasOne(a => a.GroupPhone).WithMany().HasForeignKey(ph => ph.GroupPhoneId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Address>().ToTable("AddressTable").HasKey(a => a.Id);
            modelBuilder.Entity<Address>().HasOne(a => a.GroupAddress).WithMany().HasForeignKey(a => a.GroupAddressId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Abonent>().HasMany(s => s.Addresses).WithOne(o => o.Abonent).HasForeignKey(o => o.AbonentId);
            modelBuilder.Entity<Abonent>().HasMany(s => s.Phones).WithOne(o => o.Abonent).HasForeignKey(o => o.AbonentId);
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
