using DataRepository.classes.Comments;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataRepository.comments.Read
{
    public class CountComments
    {
        public static async Task<List<Comment>> Execute(CloudTable table, string postId)
        {
            var query = new TableQuery<Comment>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Comment"),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("PostId", QueryComparisons.Equal, postId)));

            List<Comment> comments = new List<Comment>();
            var queryResult = await table.ExecuteQuerySegmentedAsync(query, null);
            comments.AddRange(queryResult.Results);


            return comments;
        }
    }
}
