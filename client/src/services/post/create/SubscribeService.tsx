import axios, { AxiosResponse } from "axios";
import { API_ENDPOINT } from "../../../App";
import toast from "react-hot-toast";

const Subscribe = async (
  PostId: string,
  email: string,
  token: string
) => {
  try {
    const response: AxiosResponse = await axios.get(
      API_ENDPOINT + `post/${PostId}/${encodeURI(email)}/subscribe/`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "multipart/form-data",
        },
      }
    );

    if (response.status === 200 || response.status === 204) {
      toast.success("You have been subscribed to the post!");
      return true;
    } else {
      toast.error("You already subscribed to the post!");
      return false;
    }
  } catch {
    return false;
  }
};

export default Subscribe;