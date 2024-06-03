using Microsoft.WindowsAzure.Storage.Table;
using RedditDataRepository.classes.Comments;

namespace RedditDataRepository.comments.Delete
{
    /// <summary>
    /// Provides methods to delete a comment from an Azure Storage Table.
    /// </summary>
    public class DeleteComment
    {
        /// <summary>
        /// Deletes a comment from the specified Azure Storage Table by its ID.
        /// </summary>
        /// <param name="table">The CloudTable object representing the Azure Storage Table.</param>
        /// <param name="commentId">The ID of the comment to be deleted.</param>
        /// <returns>A Task representing the asynchronous operation. Returns true if the operation was successful; otherwise, false.</returns>
        public static async Task<bool> Execute(CloudTable table, string commentId)
        {
            // Check if table is null
            if (table == null)
                return false;

            try
            {
                // Create a TableOperation object to retrieve the comment by its ID
                TableOperation retrieveOperation = TableOperation.Retrieve<Comment>("Comment", commentId);
                TableResult retrieveResult = await table.ExecuteAsync(retrieveOperation);

                // If the comment is found, delete it
                if (retrieveResult.Result != null)
                {
                    Comment comment = (Comment)retrieveResult.Result;
                    TableOperation deleteOperation = TableOperation.Delete(comment);
                    TableResult deleteResult = await table.ExecuteAsync(deleteOperation);
                    return deleteResult.HttpStatusCode == 204;
                }
                else
                {
                    // Comment not found
                    return false;
                }
            }
            catch
            {
                // An error occurred during deletion
                return false;
            }
        }
    }
}