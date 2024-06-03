import axios, { AxiosResponse } from "axios";
import IUser from "../../../interfaces/auth/sign_up/IUser";
import { API_ENDPOINT } from "../../../App";

/**
 * Signs up a user by sending their information to the signup endpoint.
 * @param user The user object containing signup information.
 * @returns A Promise that resolves to a boolean indicating whether the signup was successful.
 */
const SignUpService = async (user: IUser): Promise<boolean> => {
  try {
    const response: AxiosResponse = await axios.post(
      API_ENDPOINT + "auth/signup",
      user,
      {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      }
    );

    if (response.status === 200 || response.status === 204) {
      return true; // Return true if signup is successful
    } else {
      return false; // Return false if signup fails
    }
  } catch {
    return false; // Return false if an error occurs during signup
  }
};

export default SignUpService;
