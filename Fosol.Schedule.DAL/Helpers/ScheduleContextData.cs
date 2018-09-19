using Fosol.Schedule.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.DAL.Helpers
{
    public static class ScheduleContextData
    {
        public static void Init(ModelBuilder modelBuilder)
        {
            var users = new[] {
                new User("mattsbennett@gmail.com") { Id = 1 },
                new User("jeremymfoster@hotmail.com") { Id = 2 }
            };

            modelBuilder.Entity<Account>().HasData(users);

            modelBuilder.Entity<UserInfo>().HasData(
                new UserInfo(users[0], "Matthew", "Bennett") { Gender = Gender.Male },
                new UserInfo(users[0], "Jeremy", "Foster") { Gender = Gender.Male }
                );

            modelBuilder.Entity<ContactInfo>().HasData(
                new ContactInfo(users[0], "Personal", ContactInfoType.Mobile, ContactInfoCategory.Personal, "1231234567"),
                new ContactInfo(users[1], "Personal", ContactInfoType.Mobile, ContactInfoCategory.Personal, "7789774786")
                );

            modelBuilder.Entity<Address>().HasData(
                new Address(users[0], "9274 Baylis Place", "Victoria", "BC", "", "CAN", ContactInfoCategory.Personal),
                new Address(users[1], "949 Tayberry Terrace", "Victoria", "BC", "V9C0E4", "CAN", ContactInfoCategory.Personal)
                );

            modelBuilder.Entity<Calendar>().HasData(
                GenerateData.CreateCalendar(1),
                GenerateData.CreateCalendar(2)
                );
        }
    }
}
