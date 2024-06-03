import axios, { AxiosResponse } from "axios";
import { API_ENDPOINT } from "../../../App";

const ReadNumberOfCommentsByPostId = async (
  id: string
): Promise<number> => {
  try {
    const response: AxiosResponse = await axios.get(
      API_ENDPOINT + `comment/count/${id}`
    );

    if (response.status === 200 || response.status === 204) {
      return response.data;
    } else {
      return 0;
    }
  } catch {
    return 0;
  }
};

export default ReadNumberOfCommentsByPostId;
