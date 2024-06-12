import axios from "axios";
import { API_ENDPOINT } from "../../../App";
import IUser from "../../../interfaces/users/user/IUser";

/**
 * Retrieve user data by email using an asynchronous HTTP GET request.
 * @param {string} email - The email address of the user to retrieve.
 * @param {string | null} token - Optional. The JWT token for authentication.
 * @returns {Promise<IUser | null>} A Promise that resolves with the user data if successful, or null if an error occurs.
 */
const GetUserByEmail = async (email: string, token: string | null = null): Promise<IUser | null> => {
    try {
        // Define headers object to hold authorization header
        const headers: { Authorization?: string } = {};

        // Add authorization header if token is provided
        if (token) {
            headers.Authorization = `Bearer ${token}`;
        }

        // Send a GET request to the specified API endpoint with the provided email and optional authorization header
        const response = await axios.get(API_ENDPOINT + `user/get/${email}/`, { headers });
        
        // If the request is successful, return the response data
        return response.data;
    } catch {
        // If an error occurs during the request, return null
        return null;
    }
};

export default GetUserByEmail;
