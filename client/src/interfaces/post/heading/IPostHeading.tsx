/**
 * Represents the properties of a post heading component.
 */
interface IPostHeading {
  /**
   * The URL of the image associated with the post.
   */
  imageBlobUrl: string;

  /**
   * The author of the post.
   */
  author: string;

  /**
   * Indicates whether the heading is for a comment.
   */
  isCommentHeading?: boolean;

  /**
   * Indicates whether the post is in preview mode.
   */
  isPreviewMode?: boolean;
}

export default IPostHeading;
