using Fosol.Core.Extensions.Enumerable;
using Fosol.Schedule.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

            var subscription = new Subscription("Free", "A free subscription for all users.") { Id = 1 };
            var account = new Account(users[0], AccountKind.Personal, subscription) { Id = 1 };
            var admin = new AccountRole(account, "Administrator", AccountPrivilege.Administrator | AccountPrivilege.Basic) { Id = 1 };

            modelBuilder.Entity<Subscription>().HasData(subscription);
            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Account>().HasData(account);

            users.ForEach(user =>
            {
                modelBuilder.Entity<AccountUser>().HasData(new AccountUser(account, user));
            });

            modelBuilder.Entity<UserAccountRole>().HasData(new UserAccountRole(users[0], admin));

            modelBuilder.Entity<UserInfo>().HasData(
                new UserInfo(users[0], "Jeremy", "Foster") { Gender = Gender.Male },
                new UserInfo(users[1], "Matthew", "Bennett") { Gender = Gender.Male }
                );
            modelBuilder.Entity<ContactInfo>().HasData(
                new ContactInfo(users[0], "Personal", ContactInfoType.Mobile, ContactInfoCategory.Personal, "7789774786") { Id = 1 },
                new ContactInfo(users[1], "Personal", ContactInfoType.Mobile, ContactInfoCategory.Personal, "1231234567") { Id = 2 }
                );
            modelBuilder.Entity<Address>().HasData(
                new Address(users[0], "949 Tayberry Terrace", "Victoria", "BC", "V9C0E4", "CAN", ContactInfoCategory.Personal) { Id = 1 },
                new Address(users[1], "9274 Baylis Place", "Victoria", "BC", "", "CAN", ContactInfoCategory.Personal) { Id = 2 }
                );
            modelBuilder.Entity<Calendar>().HasData(
                //GenerateData.CreateEcclesialCalendar(null),
                //GenerateData.CreateEcclesialCalendar(null)
                );
        }
    }
}
