import React from "react";
import { useNavigate } from "react-router-dom";
import IPostHeading from "../../../interfaces/post/heading/IPostHeading";

const PostHeading: React.FC<IPostHeading> = ({ imageBlobUrl, author, isCommentHeading, isPreviewMode }) => {
  const navigate = useNavigate();

  return (
    <div className="p-4">
      <div className="flex items-center space-x-4">
        {!isCommentHeading && !isPreviewMode &&
        <button
          onClick={() => {
            navigate("/");
          }}
          className="flex items-center space-x-2 text-black bg-gray-300/50 p-2 rounded-full"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            className="w-6 h-6"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M15 19l-7-7 7-7"
            />
          </svg>
        </button>}
        <img
          className="w-8 h-8 rounded-full"
          src={imageBlobUrl}
          alt="Profile"
        />
        <div>
        {!isCommentHeading && <p className="text-sm font-semibold">r/cloud</p> }
        {!isCommentHeading ?
          <p className="text-xs text-gray-500">{author}</p>
          :
          <p className="text-sm text-black font-semibold">{author}</p>
        }
        </div>
      </div>
    </div>
  );
};

export default PostHeading;
