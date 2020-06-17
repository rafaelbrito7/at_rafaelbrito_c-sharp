using System;
using System.Collections.Generic;
using System.Globalization;
using Models;
using Repositories;
using ReturnStatus;

namespace Services
{
    public class PeopleService
    {
        private static List<Person> people = PeopleRepository.Index();

        public static PersonAdded AddNewPerson(string firstName, string lastName, string birthday)
        {
            string[] formats = { "MM/dd/yyyy" };
            try
            {
                if (firstName == "" | lastName == "") throw new Exception("Preencha todos os campos!");

                DateTime birthdayConverted;

                DateTime.TryParseExact(birthday, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);

                birthdayConverted = DateTime.ParseExact(birthday, formats, CultureInfo.InvariantCulture);

                Person person = new Person(firstName, lastName, birthdayConverted);

                bool AddNewPersonResult = PeopleRepository.New(person);

                return new PersonAdded { Status = AddNewPersonResult,
                    Message = AddNewPersonResult ? "Pessoa adicionada com sucesso!": "Pessoa não adicionada!"};

            } catch(Exception exception)
            {
                Console.Clear();
                return new PersonAdded { Status = false, Message = exception.Message };
            }
        }

        public static bool DeleteOne(Guid id)
        {
            try
            {
                int index = people.FindIndex(person => person.Id == id);

                if (!PeopleRepository.Delete(index)) throw new Exception();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public static PeopleFound GetAll()
        {
            try
            {
                if (people.Count == 0) throw new Exception("Não existem pessoas cadastradas!");

                return new PeopleFound { Status = true, People = people };
            }
            catch (Exception exception)
            {
                return new PeopleFound { Status = false, Message = exception.Message };
            }
        }

        public static PeopleFound GetBirthdayPeople()
        {
            try
            {
                var birthdayPeople = people.FindAll(person => person.DaysForBirthday() == 0);

                if (birthdayPeople.Count == 0) throw new Exception("Ninguém faz aniversário hoje!");

                return new PeopleFound { Status = true, People = birthdayPeople };
            }
            catch(Exception exception)
            {
                return new PeopleFound { Status = false, Message = exception.Message };
            }
        }

        public static PeopleFound GetPersonByName(string name)
        {
            try
            {
                if (name == "") throw new Exception("Por favor, insira um nome!");

                var peopleFound = people.FindAll(person => person.FullName.Contains(name));
                
                if (peopleFound.Count == 0) throw new Exception("Pessoa não encontrada!");

                return new PeopleFound { Status = true, People = peopleFound, Message = "Busca realizada com sucesso!"};
            }
            catch (Exception exception)
            {
                return new PeopleFound { Status = false, Message = exception.Message };
            }
        }
    }
}
