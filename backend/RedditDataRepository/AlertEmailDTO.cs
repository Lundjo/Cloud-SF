using System.Runtime.Serialization;

namespace RedditDataRepository
{
    [DataContract]
    public class AlertEmailDTO
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Email { get; set; }

        public AlertEmailDTO(string id, string email)
        {
            Id = id;
            Email = email;
        }

        public override string ToString()
        {
            return $"{Id}: {Email}";
        }
    }
}
