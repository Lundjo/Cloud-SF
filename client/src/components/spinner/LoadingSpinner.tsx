import React, { CSSProperties } from "react";

export interface IBackground {
  background?: string;
  minH?: string;
}

const LoadingSpinner: React.FC<IBackground> = ({ background = 'bg-white', minH = '100vh' }) => {
  const containerStyles: CSSProperties = {
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    minHeight: minH === undefined ? '100vh' : minH,
    backdropFilter: "blur(10px)", // Blur background
    backgroundColor: "rgba(255, 255, 255, 0.5)", // Adjust the opacity of the background
  };

  const spinnerStyles: CSSProperties = {
    width: "50px",
    height: "50px",
    border: "6px solid transparent",
    borderTop: "6px solid #ff5f0a",
    borderRadius: "50%",
    animation: "spin 2s linear infinite",
    zIndex: 1000,
  };

  return (
    <div style={containerStyles} className={background}>
      <div style={spinnerStyles}></div>
    </div>
  );
};

export default LoadingSpinner;
