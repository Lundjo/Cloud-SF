namespace API.Models.comment
{
    /// <summary>
    /// Represents a comment in the new request.
    /// </summary>
    public class CreateComment
    {
        /// <summary>
        /// Gets or sets the unique identifier of the comment.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the author of the comment.
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// Gets or sets the ID of the post the comment is associated with.
        /// </summary>
        public string postId { get; set; }

        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateComment"/> class.
        /// </summary>
        public CreateComment()
        {
        }
    }
}