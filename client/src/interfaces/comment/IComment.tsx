/**
 * Represents a comment on a post.
 */
interface IComment {
  /**
   * The unique identifier of the comment.
   */
  id: string;

  /**
   * The author of the comment.
   */
  author: string;

  /**
   * The unique identifier of the post for which the comment has been made.
   */
  postId: string;

  /**
   * The content of the comment.
   */
  content: string;
}

export default IComment;
