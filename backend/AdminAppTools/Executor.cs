using DataRepository;
using DataRepository.tables;
using DataRepository.tables.entities;

namespace AdminAppTools
{
    public class Executor
    {
        private static AlertEmailRepository repository = new AlertEmailRepository();

        public async Task AddEmail()
        {
            Console.WriteLine();
            Console.WriteLine("Enter ID: ");
            string id = Console.ReadLine();
            Console.WriteLine("Enter Email address: ");
            string email = Console.ReadLine();

            AlertEmailDTO alertEmail = new AlertEmailDTO(id, email);

            Console.WriteLine();
            if (await repository.CreateAsync(new AlertEmail(alertEmail.Email, alertEmail.Id)))
            {
                Console.WriteLine("Email added successfully.");
            }
            else
            {
                Console.WriteLine("Error adding email!");
            }
        }

        public async Task ListAllEmails()
        {
            var alertEmails = await repository.ReadAllAsync();
            alertEmails.Select(alertEmail => new AlertEmailDTO(alertEmail.RowKey, alertEmail.Email)).ToList();

            Console.WriteLine("\n========================== ALL EMAILS =========================== ");
            foreach (var email in alertEmails)
            {
                Console.WriteLine($"ID: {email.RowKey}, Email: {email.Email}");
            }
            Console.WriteLine("===================================================================");
        }

        public async Task RemoveEmail()
        {
            Console.WriteLine("\nEnter email ID: ");
            string id = Console.ReadLine();

            Console.WriteLine();
            if (await repository.DeleteAsync(id))
            {
                Console.WriteLine("Email removed successfully.");
            }
            else
            {
                Console.WriteLine("Email with entered ID does not exist!");
            }
        }
    }
}
