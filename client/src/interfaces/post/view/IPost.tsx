import IComment from "../../comment/IComment";

/**
 * Represents the properties of a post.
 */
interface IPost {
  /**
   * The unique identifier of the post.
   */
  id: string;

  /**
   * The author of the post.
   */
  author: string;

  /**
   * The title of the post.
   */
  title: string;

  /**
   * The content of the post.
   */
  content: string;

  /**
   * Indicates whether the post has an associated image.
   */
  hasImage: boolean;

  /**
   * The URL of the image associated with the post.
   */
  imageBlobUrl: string;

  /**
   * The list of comments associated with the post.
   */
  comments: IComment[];
}

export default IPost;
