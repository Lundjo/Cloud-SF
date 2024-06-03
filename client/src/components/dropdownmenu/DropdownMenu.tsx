import React, { useState } from "react";

const DropdownMenu: React.FC<{ setSort: (sort: number) => void }> = ({ setSort }) => {
  const [isOpen, setIsOpen] = useState(false);

  const toggleMenu = () => {
    setIsOpen(!isOpen);
  };

  const handleOptionSelect = (option: string) => {
    if (option === "New") {
      setSort(0);
    } else if (option === "A-Z") {
        setSort(1);
    } else {
        setSort(2);
    }
    setIsOpen(!isOpen);
  };

  return (
    <div className="relative inline-block text-left">
      <button
        type="button"
        className="inline-flex justify-center w-full rounded-md border shadow-sm px-4 py-2 bg-transparent text-sm font-medium text-gray-700 focus:outline-none focus:ring-offset-2 focus:ring-offset-gray-100"
        onClick={toggleMenu}
      >
        Sort
        <svg
          className="-mr-1 ml-2 h-5 w-5"
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 20 20"
          fill="currentColor"
          aria-hidden="true"
        >
          <path
            fillRule="evenodd"
            d="M6.293 7.293a1 1 0 011.414 0L10 9.586l2.293-2.293a1 1 0 111.414 1.414l-3 3a1 1 0 01-1.414 0l-3-3a1 1 0 010-1.414z"
            clipRule="evenodd"
          />
        </svg>
      </button>

      {isOpen && (
        <div
          className="origin-top-right absolute right-0 mt-2 w-56 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 focus:outline-none"
          role="menu"
          aria-orientation="vertical"
          aria-labelledby="options-menu"
        >
          <div className="py-1" role="none">
          <button
              className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:text-gray-900 w-full text-left"
              role="menuitem"
              onClick={() => handleOptionSelect("New")}
            >
              New
            </button>
            <button
              className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:text-gray-900 w-full text-left"
              role="menuitem"
              onClick={() => handleOptionSelect("A-Z")}
            >
              A-Z
            </button>
            <button
              className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:text-gray-900 w-full text-left"
              role="menuitem"
              onClick={() => handleOptionSelect("Z-A")}
            >
              Z-A
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default DropdownMenu;