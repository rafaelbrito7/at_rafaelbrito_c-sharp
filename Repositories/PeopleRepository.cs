using System;
using Models;
using System.IO;
using System.Collections.Generic;

namespace Repositories
{
    public class PeopleRepository
    {
        private static List<Person> people = new List<Person>();

        public static List<Person> Index()
        {
            CreateFile();

            string[] csvData = GetCsvData();

            for (int i = 0; i < csvData.Length - 1; i++)
            {
                string[] personData = csvData[i].Split(',');

                Guid id = Guid.Parse(personData[0]);
                string firstName = personData[1];
                string lastName = personData[2];
                DateTime birthday = DateTime.Parse(personData[3]);

                Person person = new Person(id, firstName, lastName, birthday);

                people.Add(person);
            }

            return people;
        }

        public static bool New(Person person)
        {
            try
            {
                people.Add(person);

                string fileName = GetFileName();
                string csvData = $"{person.Id},{person.FirstName},{person.LastName},{person.Birthday},{person.DaysForBirthday()};";

                File.AppendAllText(fileName, csvData);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Delete(int index)
        {
            try
            {
                people.RemoveAt(index);

                string fileName = GetFileName();
                File.WriteAllText(fileName, "");

                foreach (var person in people)
                {
                    string csvData = $"{person.Id},{person.FirstName},{person.LastName},{person.Birthday},{person.DaysForBirthday()};";
                    File.AppendAllText(fileName, csvData);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void CreateFile()
        {
            string fileName = GetFileName();

            FileStream file;
            if (!File.Exists(fileName))
            {
                file = File.Create(fileName);
                file.Close();
            }
        }

        private static string GetFileName()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = @"/friendsData.txt";
            return path + fileName;
        }

        private static string[] GetCsvData()
        {
            string fileName = GetFileName();
            string result = File.ReadAllText(fileName);

            string[] resultSplitted = result.Split(';');

            return resultSplitted;
        }
    }
}
