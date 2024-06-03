import React from "react";

const NoComments: React.FC = () => {
  return (
    <div className="flex px-0 py-0" id="empty-comments-banner">
      <div className="flex-shrink-0 w-20 mr-12">
        <img
          src="/images/thinking-snoo.png"
          alt="Thinking Snoo"
          className="w-16 h-28"
          loading="lazy"
        />
      </div>
      <div className="flex-grow">
        <p className="text-2xl font-semibold mb-4">Be the first to comment</p>
        <p className="text-gray-600">
          Nobody's responded to this post yet.
          <br />
          Add your thoughts and get the conversation going.
        </p>
      </div>
    </div>
  );
};

export default NoComments;
