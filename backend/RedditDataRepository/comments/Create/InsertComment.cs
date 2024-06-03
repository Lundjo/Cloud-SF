using Microsoft.WindowsAzure.Storage.Table;
using RedditDataRepository.classes.Comments;

namespace RedditDataRepository.comments.Create
{
    /// <summary>
    /// Provides methods to insert a comment into an Azure Storage Table.
    /// </summary>
    public class InsertComment
    {
        /// <summary>
        /// Inserts a comment into the specified Azure Storage Table.
        /// </summary>
        /// <param name="table">The CloudTable object representing the Azure Storage Table.</param>
        /// <param name="comment">The Comment object to be inserted.</param>
        /// <returns>A Task representing the asynchronous operation. Returns true if the operation was successful; otherwise, false.</returns>
        public static async Task<bool> Execute(CloudTable table, Comment comment)
        {
            // Check if comment or table is null
            if (comment == null || table == null)
                return false;

            try
            {
                // Create a TableOperation object to insert or replace the entity
                TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(comment);

                // Execute the operation asynchronously
                TableResult result = await table.ExecuteAsync(insertOrReplaceOperation);

                // Check if the operation was successful
                return result.HttpStatusCode == 200 || result.HttpStatusCode == 204;
            }
            catch
            {
                // An error occurred during insertion
                return false;
            }
        }
    }
}
