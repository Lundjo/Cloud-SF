using Microsoft.WindowsAzure.Storage.Table;

namespace RedditDataRepository.classes.Posts
{
    /// <summary>
    /// Represents a post entity stored in Azure Table Storage.
    /// </summary>
    public class Post : TableEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the post.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the author of the post.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the title of the post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the post.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the URL of the image associated with the post.
        /// </summary>
        public string ImageBlobUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the post has an associated image.
        /// </summary>
        public bool HasImage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Post"/> class.
        /// </summary>
        public Post() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Post"/> class using form data from a multipart form.
        /// </summary>
        /// <param name="provider">The multipart form data provider containing the form fields.</param>
        public Post(MultipartFormDataStreamProvider provider)
        {
            Id = Guid.NewGuid().ToString();

            // Set the partition key to "Post"
            PartitionKey = "Post";

            // Set the row key to the post ID
            RowKey = Id;

            Title = provider.FormData["title"];
            Content = provider.FormData["content"];
            Author = provider.FormData["author"];
            HasImage = bool.Parse(provider.FormData["hasImage"]);
        }
    }
}
