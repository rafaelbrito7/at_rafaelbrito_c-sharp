using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Person
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get; private set; }
        public DateTime Birthday { get; private set; }

        public Person(string firstName, string lastName, DateTime birthday)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            FullName = $"{firstName} {lastName}";
            Birthday = birthday;
        }

        public Person(Guid id, string firstName, string lastName, DateTime birthday)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            FullName = $"{firstName} {lastName}";
            Birthday = birthday;
        }

        public override string ToString()
        {
            return $"{FullName} - {Birthday}";
        }

        public int DaysForBirthday()
        {
            DateTime birthday = new DateTime(DateTime.Now.Year, Birthday.Month, Birthday.Day);

            DateTime today = DateTime.Today;
            DateTime next = birthday.AddYears(today.Year - birthday.Year);

            if (next < today)
                next = next.AddYears(1);

            return (next - today).Days;
        }
    }
}
