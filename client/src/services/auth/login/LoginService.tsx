import axios, { AxiosResponse } from "axios";
import IToken from "../../../interfaces/auth/jwt/IToken";
import { API_ENDPOINT } from "../../../App";
import ILogin from "../../../interfaces/auth/login/ILogin";

/**
 * Logs in a user and returns the JWT token if successful.
 * @param credentials The user's login credentials.
 * @returns A Promise that resolves to a IToken indicating whether the login was successful.
 */
const LoginService = async (credentials: ILogin): Promise<IToken | null> => {
  try {
    const response: AxiosResponse<IToken> = await axios.post(
      API_ENDPOINT + "auth/login",
      credentials
    );

    if (response.status === 200) {
      return { token: response.data.token };
    } else {
      return null;
    }
  } catch {
    return null;
  }
};

export default LoginService;
