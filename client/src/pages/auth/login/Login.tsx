/**
 * Login Component
 *
 * This component provides a login form for users to enter their email and password.
 *
 * It uses the ILogin interface to define the shape of the form data and defaultLogin
 * for initializing the form with default values.
 *
 * @returns JSX.Element
 */
import React, { useEffect, useState } from "react";
import defaultLogin from "../../../samples/auth/login/login";
import ILogin from "../../../interfaces/auth/login/ILogin";
import Navbar from "../../../components/navbar/Navbar";
import { ValudateLoginData } from "../../../validators/auth/login/validate_login";
import LoginService from "../../../services/auth/login/LoginService";
import { useNavigate } from "react-router-dom";
import useAuth from "../../../contexts/use_auth/UseAuth";
import IToken from "../../../interfaces/auth/jwt/IToken";
import { saveTokenToLocalstorage } from "../../../services/jwt/JWTTokenizationService";
import { jwtDecode } from "jwt-decode";
import LoadingSpinner from "../../../components/spinner/LoadingSpinner";
import ISearchBarQueryProps from "../../../interfaces/search/ISearchBarQuery";

const Login: React.FC <ISearchBarQueryProps>= ({query, setQuery}) => {
  // State variables to manage form data and error messages
  const [formData, setFormData] = useState<ILogin>(defaultLogin);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const navigate = useNavigate();
  const { isLoggedIn, setToken, setEmail } = useAuth();
  const [loading, setLoading] = useState<boolean>(true);
  const [submitting, setSubmitting] = useState<boolean>(false);

  useEffect(() => {
    setLoading(true);

    if (isLoggedIn) {
      navigate("/");
    }

    setLoading(false);
  }, [isLoggedIn, navigate]);

  // Function to handle changes in form inputs
  const handleChange = (
    event: React.ChangeEvent<HTMLInputElement>,
    key: keyof ILogin
  ) => {
    setFormData({
      ...formData,
      [key]: event.target.value,
    });
  };

  // Function to handle form submission
  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    setSubmitting(true);

    // Handle form submission, API calls, etc.
    // Reset errors
    setErrorMessage("");

    // Client-Side data verification
    const errors: string[] = ValudateLoginData(formData);

    if (errors.length === 0) {
      // Call API
      const token: IToken | null = await LoginService(formData);

      if (token) {
        // Set token and change logged in state
        saveTokenToLocalstorage(token);
        setToken(token);

        // set email address
        const decodedToken: IToken = jwtDecode(token.token ?? "");
        setEmail(decodedToken.email ?? "");

        navigate("/");
      } else {
        setErrorMessage("Entered credentials are incorrect.");
      }
    } else {
      // Show all errors
      setErrorMessage((prevErrorMessage) => {
        let newErrorMessage = prevErrorMessage + "Check next fields: ";
        errors.forEach((error, index) => {
          newErrorMessage += error;
          if (index !== errors.length - 1) {
            newErrorMessage += ", ";
          }
        });
        return newErrorMessage;
      });
    }

    setSubmitting(false);
  };

  // Render the login form
  return (
    <>
      {loading ? (
        <LoadingSpinner />
      ) : (
        <>
          <Navbar query={query} setQuery={setQuery}/>
          <section className="relative flex flex-wrap lg:h-screen lg:items-center overflow-hidden ease-in-out duration-700">
            <div className="w-full px-4 py-12 sm:px-6 sm:py-16 lg:w-1/2 lg:px-8 lg:py-24">
              <div className="mx-auto max-w-lg text-center">
                <h1 className="text-2xl font-bold sm:text-3xl text-primary-600">
                  Dive Into Anything
                </h1>

                <p className="mt-4 px-4 text-gray-500">
                  Le Reddit is home to thousands of communities, endless
                  conversation, and real connection. There's a community for
                  you.
                </p>
              </div>

              <form
                onSubmit={handleSubmit}
                className="mx-auto mb-0 mt-8 max-w-md space-y-4"
              >
                <div>
                  <label htmlFor="email" className="sr-only">
                    Email
                  </label>

                  <div className="relative">
                    <input
                      type="email"
                      autoFocus
                      value={formData.email}
                      onChange={(e) => handleChange(e, "email")}
                      className="w-full rounded-lg border-2 border-reddit-400 focus:border-reddit-600 focus:outline-none focus:ring-0 focus:border-primary-500 p-3 pe-12 text-sm shadow-sm"
                      placeholder="Email"
                    />

                    <span className="absolute inset-y-0 end-0 grid place-content-center px-4">
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        className="size-4 text-gray-400"
                        fill="none"
                        viewBox="0 0 24 24"
                        stroke="currentColor"
                      >
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          strokeWidth="2"
                          d="M16 12a4 4 0 10-8 0 4 4 0 008 0zm0 0v1.5a2.5 2.5 0 005 0V12a9 9 0 10-9 9m4.5-1.206a8.959 8.959 0 01-4.5 1.207"
                        />
                      </svg>
                    </span>
                  </div>
                </div>

                <div>
                  <label htmlFor="password" className="sr-only">
                    Password
                  </label>

                  <div className="relative">
                    <input
                    disabled={loading}
                      type="password"
                      value={formData.password}
                      onChange={(e) => handleChange(e, "password")}
                      className="w-full rounded-lg border-2 border-reddit-400 focus:border-reddit-600 focus:outline-none focus:ring-0 focus:border-primary-500 p-3 pe-12 text-sm shadow-sm"
                      placeholder="Password"
                    />

                    <span className="absolute inset-y-0 end-0 grid place-content-center px-4">
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        className="size-4 text-gray-400"
                        fill="none"
                        viewBox="0 0 24 24"
                        stroke="currentColor"
                      >
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          strokeWidth="2"
                          d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"
                        />
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          strokeWidth="2"
                          d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z"
                        />
                      </svg>
                    </span>
                  </div>
                </div>

                {errorMessage && (
                  <p className="mt-4 text-primary-600">{errorMessage}</p>
                )}

                <div className="flex items-center justify-between">
                  <p className="text-sm text-gray-500">
                    No account?
                    <a
                      className="text-primary-600 hover:text-primary-700"
                      href="/signup"
                    >
                      &nbsp;Sign up
                    </a>
                  </p>

                  <button
                    type="submit"
                    disabled={submitting}
                    className="select-none rounded-lg bg-gradient-to-tr from-primary-600 to-primary-600 py-2 px-6 text-center align-middle font-sans text-sm font-bold text-white shadow-md shadow-primary-900/10 transition-all hover:shadow-lg hover:shadow-primary-900/20 active:opacity-[0.85] disabled:pointer-events-none disabled:opacity-50 disabled:shadow-none inline-block"
                  >
                    {submitting ? "Loggin in..." : "Login"}
                  </button>
                </div>
              </form>
            </div>

            <div className="hidden lg:block relative h-64 w-full sm:h-96 lg:h-full lg:w-1/2">
              <img
                alt=""
                src="/images/login.jpg"
                className="absolute inset-0 h-full w-full object-cover"
              />
            </div>
          </section>
        </>
      )}
    </>
  );
};

export default Login;