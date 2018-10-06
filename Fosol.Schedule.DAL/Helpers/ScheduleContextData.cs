using Fosol.Core.Extensions.Collection;
using Fosol.Core.Extensions.Enumerable;
using Fosol.Schedule.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Schedule.DAL.Helpers
{
    public static class ScheduleContextData
    {
        public static void Init(ModelBuilder modelBuilder)
        {
            var users = new[] {
                new User("jeremymfoster@hotmail.com") { Id = 2 },
                new User("mattsbennett@gmail.com") { Id = 1 }
            };

            var subscription = new Subscription("Free", "A free subscription for all users.");
            var account = new Account(users[0], AccountKind.Personal, subscription);
            var admin = new AccountRole(account, "Administrator", AccountPrivilege.Administrator | AccountPrivilege.Basic);

            users.ForEach(user =>
            {
                account.AccountUsers.Add(new AccountUser(account, users[0]));
                account.AccountUsers.Add(new AccountUser(account, users[1]));
                user.AccountUsers.Add(new AccountUser(account, users[0]));
                user.AccountUsers.Add(new AccountUser(account, users[1]));
                user.UserAccountRoles.Add(new UserAccountRole(users[0], admin));
            });

            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Account>().HasData(account);
            modelBuilder.Entity<UserInfo>().HasData(
                new UserInfo(users[0], "Jeremy", "Foster") { Gender = Gender.Male },
                new UserInfo(users[1], "Matthew", "Bennett") { Gender = Gender.Male }
                );
            modelBuilder.Entity<ContactInfo>().HasData(
                new ContactInfo(users[0], "Personal", ContactInfoType.Mobile, ContactInfoCategory.Personal, "7789774786"),
                new ContactInfo(users[1], "Personal", ContactInfoType.Mobile, ContactInfoCategory.Personal, "1231234567")
                );
            modelBuilder.Entity<Address>().HasData(
                new Address(users[0], "949 Tayberry Terrace", "Victoria", "BC", "V9C0E4", "CAN", ContactInfoCategory.Personal),
                new Address(users[1], "9274 Baylis Place", "Victoria", "BC", "", "CAN", ContactInfoCategory.Personal)
                );
            modelBuilder.Entity<Calendar>().HasData(
                GenerateData.CreateCalendar(1),
                GenerateData.CreateCalendar(2)
                );
        }
    }
}
