using Microsoft.WindowsAzure.Storage.Table;

namespace DataRepository.classes.Logs
{
    public class EmailLog : TableEntity
    {
        public DateTime TimeOfEmails { get; set; }
        public string CommentId { get; set; }
        public int NumOfEmails { get; set; }
        public EmailLog(DateTime timeOfEmails, string commentId, int numOfEmails)
        {
            PartitionKey = "EmailLog";
            RowKey = Guid.NewGuid().ToString();
            TimeOfEmails = timeOfEmails;
            CommentId = commentId;
            NumOfEmails = numOfEmails;
        }
    }
}
