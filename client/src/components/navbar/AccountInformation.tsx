import React, { useEffect, useState } from "react";
import StatusOnline from "./StatusOnline";
import useAuth from "../../contexts/use_auth/UseAuth";
import { removeTokenFromLocalStorage } from "../../services/jwt/JWTTokenizationService";
import { useNavigate } from "react-router-dom";
import GetProfilePictureByEmailService from "../../services/users/profile/GetProfilePictureService";

const AccountInformation: React.FC = () => {
  const [isOpen, setIsOpen] = useState(false);
  const { email, setEmail, setToken, isLoggedIn, token } = useAuth();
  const [profilePicture, setProfilePicture] = useState<string>("");
  const [pageLoaded, setPageLoaded] = useState<boolean>(false);
  const navigate = useNavigate();

  useEffect(() => {
    setPageLoaded(true);
  }, []);

  useEffect(() => {
    if (pageLoaded && !isLoggedIn) {
      navigate("/");
    }

    const fetchData = async () => {
      try {
        if (email) {
          const image: string = await GetProfilePictureByEmailService(
            email
          );

          if (image !== "") {
            setProfilePicture(image)
          }
        }
      } catch (error) {
        navigate("/404");
      } finally {
        setPageLoaded(true);
      }
    };

    fetchData();
  }, [email, token, navigate, isLoggedIn, pageLoaded, setEmail, setToken]);

  const toggleDropdown = () => {
    setIsOpen(!isOpen);
  };

  const SignOut = () => {
    setEmail("");
    removeTokenFromLocalStorage();
    setToken(null);
    navigate("/");
    location.reload();
  };


  return (
    <div className="relative inline-block text-left">
      <div>
        <button
          type="button"
          className="inline-flex justify-center w-full rounded-md border shadow-sm px-4 py-2 bg-transparent text-sm font-medium text-gray-700 focus:outline-none focus:ring-offset-2 focus:ring-offset-gray-100"
          id="options-menu"
          aria-haspopup="true"
          aria-expanded="true"
          onClick={toggleDropdown}
        >
          <div className="flex items-center h-8">
            <StatusOnline profileImageUrl={profilePicture} isOnline={true} />
            <div className="ml-2">
              <div>{email}</div>
              <div className="text-gray-500 text-left font-demibold text-xs">
                u/{email?.split('@')[0]}
              </div>
            </div>
          </div>
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
      </div>

      {isOpen && (
        <div
          className="origin-top-right absolute right-0 mt-6 w-56 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 focus:outline-none bg-opacity-90 backdrop-blur-2xl backdrop-saturate-200"
          role="menu"
          aria-orientation="vertical"
          aria-labelledby="options-menu"
        >
          <div className="py-0" role="none">
            {/* Dropdown items */}
            <a
            href="/create"
              className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-200 hover:text-gray-900 rounded-t-md"
              role="menuitem"
              onClick={toggleDropdown}
            >
              <svg
                className="h-6 w-6 mr-2 inline-block"
                viewBox="0 0 20 20"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
              >
                <g id="SVGRepo_bgCarrier" strokeWidth={0} />
                <g
                  id="SVGRepo_tracerCarrier"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
                <g id="SVGRepo_iconCarrier">
                  {" "}
                  <path
                    fill="#087d1f"
                    fillRule="evenodd"
                    d="M9 17a1 1 0 102 0v-6h6a1 1 0 100-2h-6V3a1 1 0 10-2 0v6H3a1 1 0 000 2h6v6z"
                  />{" "}
                </g>
              </svg>
              Create
            </a>
            <a
              href="/profile"
              className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-200"
              role="menuitem"
              onClick={toggleDropdown}
            >
              <svg
                className="h-5 w-5 mr-2 inline-block text-blue-500"
                viewBox="0 0 20 20"
                version="1.1"
                xmlns="http://www.w3.org/2000/svg"
                xmlnsXlink="http://www.w3.org/1999/xlink"
                fill="currentColor"
              >
                <g id="SVGRepo_bgCarrier" strokeWidth={0} />
                <g
                  id="SVGRepo_tracerCarrier"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
                <g id="SVGRepo_iconCarrier">
                  {" "}
                  <defs> </defs>{" "}
                  <g
                    id="Page-1"
                    stroke="none"
                    strokeWidth={1}
                    fill="none"
                    fillRule="evenodd"
                  >
                    {" "}
                    <g
                      id="Dribbble-Light-Preview"
                      transform="translate(-380.000000, -2119.000000)"
                      fill="currentColor"
                    >
                      {" "}
                      <g
                        id="icons"
                        transform="translate(56.000000, 160.000000)"
                      >
                        {" "}
                        <path
                          d="M338.083123,1964.99998 C338.083123,1962.79398 336.251842,1960.99998 334,1960.99998 C331.748158,1960.99998 329.916877,1962.79398 329.916877,1964.99998 C329.916877,1967.20599 331.748158,1968.99999 334,1968.99999 C336.251842,1968.99999 338.083123,1967.20599 338.083123,1964.99998 M341.945758,1979 L340.124685,1979 C339.561214,1979 339.103904,1978.552 339.103904,1978 C339.103904,1977.448 339.561214,1977 340.124685,1977 L340.5626,1977 C341.26898,1977 341.790599,1976.303 341.523154,1975.662 C340.286989,1972.69799 337.383888,1970.99999 334,1970.99999 C330.616112,1970.99999 327.713011,1972.69799 326.476846,1975.662 C326.209401,1976.303 326.73102,1977 327.4374,1977 L327.875315,1977 C328.438786,1977 328.896096,1977.448 328.896096,1978 C328.896096,1978.552 328.438786,1979 327.875315,1979 L326.054242,1979 C324.778266,1979 323.773818,1977.857 324.044325,1976.636 C324.787453,1973.27699 327.107688,1970.79799 330.163906,1969.67299 C328.769519,1968.57399 327.875315,1966.88999 327.875315,1964.99998 C327.875315,1961.44898 331.023403,1958.61898 334.733941,1959.04198 C337.422678,1959.34798 339.650022,1961.44698 340.05323,1964.06998 C340.400296,1966.33099 339.456073,1968.39599 337.836094,1969.67299 C340.892312,1970.79799 343.212547,1973.27699 343.955675,1976.636 C344.226182,1977.857 343.221734,1979 341.945758,1979 M337.062342,1978 C337.062342,1978.552 336.605033,1979 336.041562,1979 L331.958438,1979 C331.394967,1979 330.937658,1978.552 330.937658,1978 C330.937658,1977.448 331.394967,1977 331.958438,1977 L336.041562,1977 C336.605033,1977 337.062342,1977.448 337.062342,1978"
                          id="profile_round-[#509f81]"
                        >
                          {" "}
                        </path>{" "}
                      </g>{" "}
                    </g>{" "}
                  </g>{" "}
                </g>
              </svg>
              Profile
            </a>

            
            <hr className=" bg-primary-500 h-0.5" />
            <button
              className="w-full block items-start text-start px-4 py-2 text-sm text-gray-700 rounded-b-md hover:bg-gray-200 hover:text-gray-900"
              role="menuitem"
              onClick={() => { SignOut(); }}
            >
              <svg
                className="h-6 w-6 mr-2 inline-block"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <g id="SVGRepo_iconCarrier">
                  {" "}
                  <path
                    d="M10 12H20M20 12L17 9M20 12L17 15"
                    stroke="#b12525"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />{" "}
                  <path
                    d="M4 12C4 7.58172 7.58172 4 12 4M12 20C9.47362 20 7.22075 18.8289 5.75463 17"
                    stroke="#b12525"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                  />{" "}
                </g>
              </svg>
              <p className="inline-block">Sign Out</p>
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default AccountInformation;
