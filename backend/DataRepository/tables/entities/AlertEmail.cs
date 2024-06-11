using Microsoft.WindowsAzure.Storage.Table;
using System.Runtime.Serialization;

namespace DataRepository.tables.entities
{
    [DataContract]
    public class AlertEmail : TableEntity
    {
        [DataMember]
        public string Email { get; set; }

        public AlertEmail(string email, string id)
        {
            PartitionKey = "AlertEmail";
            RowKey = id;
            Email = email;
        }

        public AlertEmail()
        {
        }
    }
}
