using Microsoft.WindowsAzure.Storage.Table;

namespace RedditDataRepository.classes.Comments
{
    /// <summary>
    /// Represents a comment in the Reddit data repository.
    /// </summary>
    public class Comment : TableEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the comment.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the author of the comment.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the ID of the post the comment is associated with.
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        public Comment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class with the specified author, post ID, and content.
        /// </summary>
        /// <param name="author">The author of the comment.</param>
        /// <param name="postId">The ID of the post the comment is associated with.</param>
        /// <param name="content">The content of the comment.</param>
        public Comment(string author, string postId, string content)
        {
            Id = Guid.NewGuid().ToString();
            PartitionKey = "Comment";
            RowKey = Id;
            Author = author;
            PostId = postId;
            Content = content;
        }
    }
}
