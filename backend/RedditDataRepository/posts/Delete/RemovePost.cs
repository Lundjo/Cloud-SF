using Microsoft.WindowsAzure.Storage.Table;
using RedditDataRepository.blobs.images;
using RedditDataRepository.classes.Posts;

namespace RedditDataRepository.posts.Delete
{
    /// <summary>
    /// Provides methods to delete a post from an Azure Storage Table.
    /// </summary>
    public class DeletePost
    {
        /// <summary>
        /// Deletes a post from the specified Azure Storage Table by its ID.
        /// </summary>
        /// <param name="table">The CloudTable object representing the Azure Storage Table.</param>
        /// <param name="postId">The ID of the post to be deleted.</param>
        /// <returns>A Task representing the asynchronous operation. Returns true if the operation was successful; otherwise, false.</returns>
        public static async Task<bool> Execute(CloudTable table, string postId)
        {
            // Check if table is null
            if (table == null)
                return false;

            try
            {
                // Create a TableOperation object to retrieve the post by its ID
                TableOperation retrieveOperation = TableOperation.Retrieve<Post>("Post", postId);
                TableResult retrieveResult = await table.ExecuteAsync(retrieveOperation);

                // If the post is found, delete it
                if (retrieveResult.Result != null)
                {
                    Post post = (Post)retrieveResult.Result;

                    // If post has image delete it from blob storage
                    if (post.HasImage)
                    {
                        await AzureBlobStorage.RemoveFileFromBlobStorage(post.ImageBlobUrl);
                    }

                    TableOperation deleteOperation = TableOperation.Delete(post);
                    TableResult deleteResult = await table.ExecuteAsync(deleteOperation);
                    return deleteResult.HttpStatusCode == 204;
                }
                else
                {
                    // Post not found
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
