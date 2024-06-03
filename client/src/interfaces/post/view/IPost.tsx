import IComment from "../../comment/IComment";

/**
 * Represents the properties of a post.
 */
interface IPost {
  /**
   * The unique identifier of the post.
   */
  Id: string;

  /**
   * The author of the post.
   */
  Author: string;

  /**
   * The title of the post.
   */
  Title: string;

  /**
   * The content of the post.
   */
  Content: string;

  /**
   * Indicates whether the post has an associated image.
   */
  HasImage: boolean;

  /**
   * The URL of the image associated with the post.
   */
  ImageBlobUrl: string;

  /**
   * The list of comments associated with the post.
   */
  Comments: IComment[];
}

export default IPost;
