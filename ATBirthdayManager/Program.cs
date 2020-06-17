using System;
using Services;
using Models;
using ReturnStatus;
using System.Globalization;

namespace ATBirthdayManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowBirthdayPeople();
            MainMenu();
            Console.WriteLine("FIM");
        }

        public static void MainMenu()
        {
            char optionSelected;

            Console.WriteLine("Gerenciamento de Aniversários");
            Console.WriteLine("1 - Pesquisar pessoas");
            Console.WriteLine("2 - Adicionar nova pessoa");
            Console.WriteLine("3 - Editar Pessoa");
            Console.WriteLine("4 - Excluir Pessoa");
            Console.WriteLine("5 - Sair");
            Console.Write("Escolha a Opção: ");
            optionSelected = Console.ReadLine().ToCharArray()[0];
                
            switch(optionSelected)
            {
                case '1':
                    FindPersonByName();
                    break;
                case '2':
                    NewPerson();
                    break;
                case '3':
                    EditPerson();
                    break;
                case '4':
                    DeletePerson();
                    break;
                case '5':
                    break;
                default:
                    WrongMenuOption();
                    break;
            } 
        }

        public static void ShowBirthdayPeople()
        {
            PeopleFound birthdayPeople = PeopleService.GetBirthdayPeople();

            if (!birthdayPeople.Status)
            {
                Console.WriteLine(birthdayPeople.Message);
            } else
            {
                Console.WriteLine("Aniversariantes do dia");
                foreach (var person in birthdayPeople.People)
                {
                    Console.WriteLine($"{person.FullName}");
                }
            }
        }

        public static void NewPerson()
        {
            string firstName = ReadString("Digite o nome: "); ;
            string lastName = ReadString("Digite o sobrenome: ");
            string birthday = ReadString("Digite o aniversário no formato (MM/dd/yyyy): ");

            PersonAdded addNewPersonResult = PeopleService.AddNewPerson(firstName, lastName, birthday);

            EndMethod(addNewPersonResult.Message);
        }

        public static void DeletePerson()
        {
            try
            {
                string name = ReadString("Digite o nome da pessoa que deseja pesquisar: ");

                Person chosenPerson = ShowPeopleFound(name);

                PeopleService.DeleteOne(chosenPerson.Id);

                EndMethod("Pessoa removida com sucesso!");
            }
            catch (Exception exception)
            {
                EndMethod(exception.Message);
            }
        }

        public static void EditPerson()
        {
            try
            {
                string name = ReadString("Digite o nome da pessoa que deseja pesquisar: ");

                Person chosenPerson = ShowPeopleFound(name);

                string firstName = ReadString("Digite o nome: "); ;
                string lastName = ReadString("Digite o sobrenome: ");
                string birthday = ReadString("Digite o aniversário no formato (MM/dd/yyyy): ");

                while (ValidateDate(birthday) == false)
                {
                    Console.WriteLine("Data inserida incorretamente!");
                    birthday = ReadString("Digite o aniversário no formato (MM/dd/yyyy): ");
                }

                PeopleService.DeleteOne(chosenPerson.Id);

                PeopleService.AddNewPerson(firstName, lastName, birthday);

                EndMethod("Pessoa editada com sucesso!");
            }
            catch (Exception exception)
            {
                EndMethod(exception.Message);
            }
        }

        public static void FindPersonByName()
        {
            try
            {
                string name = ReadString("Digite o nome da pessoa que deseja pesquisar: ");

                Person chosenPerson = ShowPeopleFound(name);

                var chosenPersonsBirthday = chosenPerson.DaysForBirthday();

                if (chosenPersonsBirthday == 0)
                {
                    Console.WriteLine($"É aniversário de {chosenPerson.FullName}! Deseje Parabéns!\n");
                }
                else
                {
                    Console.WriteLine($"{chosenPerson.ToString()}  | Dias restantes para o aniversário: {chosenPerson.DaysForBirthday()}\n");
                }
                MainMenu();
            }
            catch (Exception exception)
            {
                EndMethod(exception.Message);
            }
        }

        public static Person ShowPeopleFound(string name)
        {
            PeopleFound peopleFound = PeopleService.GetPersonByName(name);
            if (peopleFound.Status == false) throw new Exception(peopleFound.Message);

            int contador = 1;
            foreach (var person in peopleFound.People)
            {
                Console.WriteLine($"{contador} - {person.FullName}");
                contador++;
            }

            int option = int.Parse(ReadString("Escolha uma pessoa: "));
            if (option < 1 | option > peopleFound.People.Count)
            {
                Console.WriteLine("Opção Inválida");
                MainMenu();
            }

            return peopleFound.People[option - 1];
        }

        public static string ReadString(string msg)
        {
            Console.Write(msg);
            return Console.ReadLine();
        }

        public static bool ValidateDate(string birthday)
        {
            string[] formats = { "MM/dd/yyyy" };

            bool validDate = false;
            
            if (DateTime.TryParseExact(birthday, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                validDate = true;

            return validDate;
        }

        public static DateTime ConvertBirthdayToDateTime(string birthday)
        {
            string[] formats = { "MM/dd/yyyy" };

            DateTime birthdayConverted = DateTime.ParseExact(birthday, formats, CultureInfo.InvariantCulture);

            return birthdayConverted;
        }

        public static void EndMethod(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            MainMenu();
        }

        public static void WrongMenuOption()
        {
            Console.Clear();
            Console.WriteLine("Opção Inválida!");
            Console.WriteLine("Escolha novamente...");
            MainMenu();
        }
    }
}

