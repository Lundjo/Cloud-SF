using Microsoft.WindowsAzure.Storage.Table;
using System.Runtime.Serialization;

namespace RedditDataRepository.tables.entities
{
    [DataContract]
    public class HealthCheck : TableEntity
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Service { get; set; }

        public HealthCheck(string dateTime, string status, string service)
        {
            PartitionKey = "HealthCheck";
            RowKey = dateTime;
            Status = status;
            Service = service;
        }

        public HealthCheck()
        {
        }
    }
}
