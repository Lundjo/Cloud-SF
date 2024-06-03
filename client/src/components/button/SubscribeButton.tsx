import React from 'react';

interface SubscribeButtonProps {
  onClick: () => void; // Function to handle the click event
}

const SubscribeButton: React.FC<SubscribeButtonProps> = ({ onClick }) => {
  return (
    <button
      className="flex items-center justify-center w-8 h-8 mt-4 mr-3 rounded-full bg-gray-500/20 text-gray-700 hover:bg-gray-500/40 hover:text-gray-600 focus:outline-none"
      onClick={onClick}
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="24"
        height="24"
        viewBox="0 0 24 24"
        fill="none"
      >
        <path
          d="M4 12H20M12 4V20"
          stroke="currentColor"
          strokeWidth={2}
          strokeLinecap="round"
          strokeLinejoin="round"
        />
      </svg>
    </button>
  );
};

export default SubscribeButton;
