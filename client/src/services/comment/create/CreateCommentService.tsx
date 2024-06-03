import axios, { AxiosResponse } from "axios";
import { API_ENDPOINT } from "../../../App";
import IComment from "../../../interfaces/comment/IComment";

const CreateCommentService = async (
  comment: IComment,
  token: string
): Promise<boolean> => {

  try {
    const response: AxiosResponse = await axios.post(
      API_ENDPOINT + "comment/create",
      comment,
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

export default CreateCommentService;
