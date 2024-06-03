/**
 * Represents the properties of post statistics component.
 */
interface IPostStatsProps {
  /**
   * The total count of upvotes and downvotes for the post.
   */
  upvotesDownvotesCount: number;

  /**
   * The number of comments on the post.
   */
  numberOfComments: number;
}

export default IPostStatsProps;
