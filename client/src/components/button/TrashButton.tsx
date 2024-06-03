import React from "react";

interface TrashButtonProps {
  onClick: () => void;
}

const TrashButton: React.FC<TrashButtonProps> = ({ onClick }) => {
  return (
    <button
      type="button"
      onClick={onClick}
      className="items-end p-2 mr-2 h-10 w-10 pt-4 text-red-600 font-semibold hover:text-red-500"
    >
      {/* SVG Trash Icon */}
      <svg
        viewBox="0 0 24 24"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M4 7H20"
          stroke="currentColor"
          strokeWidth={2}
          strokeLinecap="round"
          strokeLinejoin="round"
        />
        <path
          d="M6 10L7.70141 19.3578C7.87432 20.3088 8.70258 21 9.66915 21H14.3308C15.2974 21 16.1257 20.3087 16.2986 19.3578L18 10"
          stroke="currentColor"
          strokeWidth={2}
          strokeLinecap="round"
          strokeLinejoin="round"
        />
        <path
          d="M9 5C9 3.89543 9.89543 3 11 3H13C14.1046 3 15 3.89543 15 5V7H9V5Z"
          stroke="currentColor"
          strokeWidth={2}
          strokeLinecap="round"
          strokeLinejoin="round"
        />
      </svg>
    </button>
  );
};

export default TrashButton;
