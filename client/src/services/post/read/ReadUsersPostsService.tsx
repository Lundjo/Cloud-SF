import axios, { AxiosResponse } from "axios";
import IPost from "../../../interfaces/post/view/IPost";
import { API_ENDPOINT } from "../../../App";

const GetUsersPostsService = async (id: string | undefined, search: string | undefined, sort: number | undefined, time: Date | undefined, email: string, token: string): Promise<IPost[] | null> => {
  try {
    const response: AxiosResponse = await axios.get(API_ENDPOINT + `post/${id}/${search}/userPosts/${sort}/${time?.getTime()}/${encodeURI(email)}/`,
    {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "multipart/form-data",
      },
    });

    if (response.status === 200 || response.status === 204) {
      return response.data;
    } else {
      return null;
    }
  } catch {
    return null;
  }
};

export default GetUsersPostsService;
