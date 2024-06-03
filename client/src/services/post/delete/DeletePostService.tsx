import axios, { AxiosResponse } from "axios";
import { API_ENDPOINT } from "../../../App";

const DeletePostService = async (
  postId: string,
  token: string
): Promise<boolean> => {
  try {
    const response: AxiosResponse = await axios.delete(
      API_ENDPOINT + `post/delete/?postId=${postId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );

    if (response.status === 200 || response.status === 204) {
      return true;
    } else {
      return false;
    }
  } catch {
    return false;
  }
};

export default DeletePostService;
