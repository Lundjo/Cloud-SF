/**
 * Provides a custom hook to access the authentication context values.
 * @returns An object containing authentication context values such as token, isLoggedIn, and isTokenValid.
 */
import { useContext } from "react";
import IAuthContextType from "../../interfaces/auth/auth_context/IAuthContextType";
import { AuthContext } from "../auth/AuthContext";

const useAuth = (): IAuthContextType => useContext(AuthContext);

export default useAuth;