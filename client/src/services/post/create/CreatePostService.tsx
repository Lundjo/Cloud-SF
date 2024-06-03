import axios, { AxiosResponse } from "axios";
import { API_ENDPOINT } from "../../../App";
import ICreatePost from "../../../interfaces/post/create/ICreatePost";

/**
 * Creates a new post on the server.
 * @param post The data for the post to be created.
 * @param token The authorization token for the user.
 * @returns A Promise that resolves with the ID of the created post if successful, otherwise an empty string.
 */
const CreatePostService = async (
  post: ICreatePost,
  token: string
): Promise<string> => {

  try {
    const response: AxiosResponse = await axios.post(
      API_ENDPOINT + "post/create",
      post,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "multipart/form-data",
        },
      }
    );

    if (response.status === 200 || response.status === 204) {
      return response.data;
    } else {
      return "";
    }
  } catch {
    return "";
  }
};

export default CreatePostService;
