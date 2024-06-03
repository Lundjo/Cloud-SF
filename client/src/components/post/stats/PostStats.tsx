import React, { useEffect } from "react";
import IPostStatsProps from "../../../interfaces/post/stats/IPostStatsProp";
import { useNavigate } from "react-router-dom";

const PostStats: React.FC<IPostStatsProps & { onUpvote: () => void; onDownvote: () => void; isUpvoted: boolean; isDownvoted: boolean; postId: string }> = ({
  upvotesDownvotesCount,
  numberOfComments,
  onUpvote,
  onDownvote,
  isUpvoted, 
  isDownvoted,
  postId,
}) => {
  const navigate = useNavigate();

  const redirectToPost = () => {
    navigate(`/post/${postId}`);
  };

  useEffect(() => {
    if (!isUpvoted || !isDownvoted) {
      localStorage.removeItem('postVoteStatus');
    }
  }, [isUpvoted, isDownvoted]);

  useEffect(() => {
    const voteStatus = localStorage.getItem('postVoteStatus');
    if (voteStatus) {
      localStorage.removeItem('postVoteStatus');
    }
  }, []);

  return (
    <div className="flex items-center space-x-4 mt-4 -mb-4">
      {/* Upvote Button */}
      <button  className={`flex items-center justify-center w-8 h-8 rounded-full hover:text-orange-500 hover:bg-gray-300 ${
          isUpvoted ? 'text-orange-500 bg-primary-200' : ' bg-gray-200 text-gray-500'
        } focus:outline-none`}
        onClick={onUpvote}>
        <svg
          xmlns="http://www.w3.org/2000/svg"
          className="h-4 w-4"
          viewBox="0 0 20 20"
          fill="currentColor"
        >
          <path
            fillRule="evenodd"
            d="M10 18a1 1 0 0 1-1-1v-11.06l-3.72 3.72a1 1 0 1 1-1.414-1.415l5.5-5.5a1 1 0 0 1 1.414 0l5.5 5.5a1 1 0 1 1-1.414 1.415L11 6.94V17a1 1 0 0 1-1 1z"
            clipRule="evenodd"
          />
        </svg>
      </button>
      {/* Like Count */}
      <span className="text-primary-500 font-semibold">
        {upvotesDownvotesCount}
      </span>
      {/* Downvote Button */}
      <button className={`flex items-center justify-center w-8 h-8 rounded-full hover:text-blue-500 hover:bg-gray-300 ${
          isDownvoted ? 'text-blue-500 bg-blue-200' : ' bg-gray-200 text-gray-500'
        } focus:outline-none`}
        onClick={onDownvote}>
        <svg
          xmlns="http://www.w3.org/2000/svg"
          className="h-4 w-4"
          viewBox="0 0 20 20"
          fill="currentColor"
        >
          <path
            fillRule="evenodd"
            d="M10 2a1 1 0 0 1 1 1v11.059l3.72-3.72a1 1 0 1 1 1.414 1.415l-5.5 5.5a1 1 0 0 1-1.415 0l-5.5-5.5a1 1 0 1 1 1.414-1.415L9 14.059V3a1 1 0 0 1 1-1z"
            clipRule="evenodd"
          />
        </svg>
      </button>
      {/* Comment Count */}
      <div onClick={redirectToPost} className="flex items-center bg-gray-200 rounded-full px-2 py-1 hover:bg-gray-200/70 hover:text-gray-600 cursor-pointer">
        <svg
          className="text-gray-500 mr-2 h-5 w-5"
          viewBox="0 0 32 32"
          version="1.1"
          xmlns="http://www.w3.org/2000/svg"
          xmlnsXlink="http://www.w3.org/1999/xlink"
        >
          <g
            id="Page-1"
            stroke="none"
            strokeWidth="1"
            fill="none"
            fillRule="evenodd"
          >
            <g
              id="Icon-Set"
              transform="translate(-308.000000, -255.000000)"
              fill="currentColor"
            >
              <path
                d="M327.494,279.633 L324,284 L320.506,279.633 C314.464,278.355 309.992,273.863 309.992,268.501 C309.992,262.146 316.264,256.994 324,256.994 C331.736,256.994 338.008,262.146 338.008,268.501 C338.008,273.863 333.536,278.355 327.494,279.633 L327.494,279.633 Z M324,255 C315.163,255 308,261.143 308,268.72 C308,274.969 312.877,280.232 319.542,281.889 L324,287.001 L328.459,281.889 C335.123,280.232 340,274.969 340,268.72 C340,261.143 332.837,255 324,255 L324,255 Z"
                id="comment-4"
              ></path>
            </g>
          </g>
        </svg>
        <span className="text-gray-600">{numberOfComments}&nbsp;</span>
      </div>
    </div>
  );
};

export default PostStats;
