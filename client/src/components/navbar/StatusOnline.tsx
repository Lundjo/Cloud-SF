import React from "react";

interface StatusOnlineProps {
  profileImageUrl: string;
  isOnline: boolean;
}

const StatusOnline: React.FC<StatusOnlineProps> = ({
  profileImageUrl,
  isOnline,
}) => {
  return (
    <div className="relative inline-block mr-2">
      <img
        src={profileImageUrl}
        alt="Profile"
        className="w-8 h-8 rounded-full"
      />
      {isOnline && (
        <span className="absolute bottom-0 right-0 inline-flex items-center justify-center w-3 h-3 rounded-full bg-emerald-600 border-2 border-white"></span>
      )}
    </div>
  );
};

export default StatusOnline;
