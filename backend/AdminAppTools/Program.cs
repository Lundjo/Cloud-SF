using DataRepository;
using DataRepository.tables;

namespace AdminAppTools
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Executor executor = new Executor();

            Console.WriteLine("Enter password to proceed:");
            string password = Console.ReadLine();

            if (password != "opako")
            {
                Console.WriteLine("Incorrect password. Exiting...");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("Welcome Admin!");
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. List all emails");
                Console.WriteLine("2. Add a new email");
                Console.WriteLine("3. Delete an existing email");
                Console.WriteLine("4. Exit");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await executor.ListAllEmails();
                        break;
                    case "2":
                        await executor.AddEmail();
                        break;
                    case "3":
                        await executor.RemoveEmail();
                        break;
                    case "4":
                        exit = true;
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
