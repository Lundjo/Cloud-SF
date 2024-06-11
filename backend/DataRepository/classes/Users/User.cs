using Microsoft.WindowsAzure.Storage.Table;

namespace DataRepository.classes.Users
{
    /// <summary>
    /// Represents a registered user entity in the storage table.
    /// </summary>
    public class User : TableEntity
    {
        /// <summary>
        /// Gets or sets the first name of the registered user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the registered user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the address of the registered user.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the city of the registered user.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the country of the registered user.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the registered user.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the email address of the registered user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the registered user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the URL of the image blob associated with the registered user.
        /// </summary>
        public string ImageBlobUrl { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class using data from a <see cref="MultipartFormDataStreamProvider"/>.
        /// </summary>
        /// <param name="provider">The <see cref="MultipartFormDataStreamProvider"/> containing the form data.</param>
        public User(MultipartFormDataStreamProvider provider)
        {
            // Set the partition key to "User"
            PartitionKey = "User";

            // Set the row key to the email address provided in the form data
            RowKey = provider.FormData["email"];

            // Set other properties based on form data
            FirstName = provider.FormData["firstName"];
            LastName = provider.FormData["lastName"];
            Address = provider.FormData["address"];
            City = provider.FormData["city"];
            Country = provider.FormData["country"];
            Phone = provider.FormData["phone"];
            Email = provider.FormData["email"];
            Password = provider.FormData["password"];
        }
    }
}
