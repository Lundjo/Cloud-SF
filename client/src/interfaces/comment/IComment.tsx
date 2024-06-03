/**
 * Represents a comment on a post.
 */
interface IComment {
  /**
   * The unique identifier of the comment.
   */
  Id: string;

  /**
   * The author of the comment.
   */
  Author: string;

  /**
   * The unique identifier of the post for which the comment has been made.
   */
  PostId: string;

  /**
   * The content of the comment.
   */
  Content: string;
}

export default IComment;
