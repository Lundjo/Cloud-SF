/**
 * Represents a post entity.
 */
interface ICreatePost {
  /**
   * Gets or sets the title of the post.
   */
  title: string;

  /**
   * Gets or sets the author of the post.
   */
  author: string;

  /**
   * Gets or sets the content of the post.
   */
  content: string;

  /**
   * Post's profile image.
   */
  image: File | null;

  /**
   * Gets or sets a value indicating whether the post has an associated image.
   */
  hasImage: boolean;
}

export default ICreatePost;
