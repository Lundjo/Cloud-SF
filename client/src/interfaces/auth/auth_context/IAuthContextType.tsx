import IToken from "../jwt/IToken";

/**
 * Represents the shape of the authentication context.
 */
interface IAuthContextType {
  /**
   * The authentication token.
   */
  token: IToken | null;

  /**
   * The user email.
   */
  email: string | null;

  /**
   * Function to set the authentication token.
   * @param token The authentication token to set.
   */
  setToken: (token: IToken | null) => void;

  /**
   * Function to set the user email.
   * @param email The email to set.
   */
  setEmail: (email: string) => void;

  /**
   * Boolean indicating whether the user is logged in or not.
   */
  isLoggedIn: boolean;

  /**
   * Boolean indicating whether the authentication token is valid or not.
   */
  isTokenValid: boolean;
}

export default IAuthContextType;
