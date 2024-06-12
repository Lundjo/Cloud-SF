import axios, { AxiosResponse } from "axios";
import { API_ENDPOINT } from "../../../App";

const DeleteCommentService = async (
  commentId: string,
  token: string
): Promise<boolean> => {
  try {
    const response: AxiosResponse = await axios.delete(
      API_ENDPOINT + `comment/delete/${commentId}/`,
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

export default DeleteCommentService;
