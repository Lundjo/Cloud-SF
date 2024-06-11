using DataRepository.classes.Posts;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataRepository.Posts.Read
{
    public class ReadSubscriptions
    {
        public static async Task<List<string>> Run(CloudTable table, string postId)
        {
            List<string> emails = new List<string>();

            var filter = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Subscription"),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("PostId", QueryComparisons.Equal, postId)
            );

            var tableQuery = new TableQuery<Subscription>().Where(filter);
            var result = await table.ExecuteQuerySegmentedAsync(tableQuery, null);

            foreach (var sub in result)
            {
                emails.Add(sub.Email);
            }

            return emails;
        }
    }
}
