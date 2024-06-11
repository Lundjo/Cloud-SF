using DataRepository.classes.Posts;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataRepository.posts.Create
{
    /// <summary>
    /// Provides methods to insert a post entity into an Azure Storage Table.
    /// </summary>
    public class InsertPost
    {
        /// <summary>
        /// Inserts a post entity into the specified Azure Storage Table asynchronously.
        /// </summary>
        /// <param name="table">The Azure Storage Table to insert the post into.</param>
        /// <param name="post">The post entity to insert into the table.</param>
        /// <returns>
        /// A task representing the asynchronous operation. Returns <c>true</c> if the insertion was successful; otherwise, <c>false</c>.
        /// </returns>
        public static async Task<bool> Execute(CloudTable table, Post post)
        {
            // Check if post or table is null
            if (post == null || table == null)
                return false;

            try
            {
                // Create a TableOperation object to insert or replace the entity
                TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(post);

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
