using RedditDataRepository.classes.Comments;

namespace API.Models.post
{
    /// <summary>
    /// Represents a model for retrieving post details.
    /// </summary>
    public class GetPost
    {
        /// <summary>
        /// The unique identifier of the post.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The author of the post.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The title of the post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The content of the post.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Indicates whether the post has an associated image.
        /// </summary>
        public bool HasImage { get; set; }

        /// <summary>
        /// The URL of the image associated with the post.
        /// </summary>
        public string ImageBlobUrl { get; set; }

        /// <summary>
        /// The list of comments associated with the post.
        /// </summary>
        public List<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        /// Default constructor for GetPost.
        /// </summary>
        public GetPost() { }

        /// <summary>
        /// Parameterized constructor for GetPost.
        /// </summary>
        /// <param name="id">The unique identifier of the post.</param>
        /// <param name="author">The author of the post.</param>
        /// <param name="title">The title of the post.</param>
        /// <param name="content">The content of the post.</param>
        /// <param name="hasImage">Indicates whether the post has an associated image.</param>
        /// <param name="imageBlobUrl">The URL of the image associated with the post.</param>
        /// <param name="comments">The list of comments associated with the post.</param>
        public GetPost(string id, string author, string title, string content, bool hasImage, string imageBlobUrl, List<Comment> comments)
        {
            Id = id;
            Author = author;
            Title = title;
            Content = content;
            HasImage = hasImage;
            ImageBlobUrl = imageBlobUrl;
            Comments = comments;
        }
    }
}
