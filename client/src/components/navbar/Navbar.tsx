import React from "react";
import { useNavigate } from "react-router-dom";
import AccountInformation from "./AccountInformation";
import SearchBar from "../search/SearchBar";
import useAuth from "../../contexts/use_auth/UseAuth";
import ISearchBarQueryProps from "../../interfaces/search/ISearchBarQuery";

const Navbar: React.FC<ISearchBarQueryProps> = ({setQuery}) => {
  const navigate = useNavigate();
  const { isLoggedIn } = useAuth();

  return (
    <div className="sticky top-0 z-10 bg-white border-b border-gray-200">
      <div className="mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between h-16">
          <div className="flex items-center">
            {/* Logo */}
            <a href="/">
              <img
                className="block lg:hidden h-8 w-auto"
                src="/reddit.svg"
                alt="logo"
              />
              <img
                className="hidden lg:block h-8 w-auto"
                src="/reddit.svg"
                alt="logo"
              />
            </a>
            <a href="/" className="ml-2 text-xl font-semibold">Le Reddit</a>
          </div>
          <div className="flex items-center gap-8">
            {/* Search Bar */}
            <SearchBar
              placeholder="Search Le Reddit..."
              onSearch={(query) => {
                // Handle search functionality
                setQuery(query);
              }}
            />
            <div className="flex items-center gap-x-1">
              {!isLoggedIn ? (
                <div>
                  <button
                    className="px-4 py-1.5 font-bold text-primary-600 uppercase transition-all rounded-lg mr-2 select-none hover:bg-primary-900/10 active:bg-primary-600/20 disabled:pointer-events-none disabled:opacity-50 disabled:shadow-none"
                    type="button"
                    onClick={() => navigate("/login")}
                  >
                    Log In
                  </button>
                  <button
                    className="px-4 py-1.5 text-white font-semibold uppercase transition-all bg-gradient-to-tr from-primary-600 to-primary-600 hover:from-primary-700 hover:to-primary-700 active:from-primary-800 active:to-primary-800 disabled:pointer-events-none disabled:opacity-50 disabled:shadow-none rounded-lg"
                    type="button"
                    onClick={() => navigate("/signup")}
                  >
                    Sign up
                  </button>
                </div>
              ) : (
                <AccountInformation />
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Navbar;
