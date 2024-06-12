import axios, { AxiosResponse } from "axios";
import IUser from "../../../interfaces/users/user/IUser";
import { API_ENDPOINT } from "../../../App";
import IUpdateUser from "../../../interfaces/users/user/update/IUpdateUser";

/**
 * Updates user information.
 * @param user - The user object containing updated information.
 * @param image - The new image file for the user (optional).
 * @param token - The authentication token.
 * @returns A boolean indicating whether the update was successful.
 */
const UpdateUserInformationService = async (
  user: IUser,
  image: File | null,
  token: string
): Promise<boolean> => {
  try {
    // Convert from user data to RegisteredUser
    const updateUser: IUpdateUser = {
      firstName: user.firstName,
      lastName: user.lastName,
      address: user.address,
      city: user.city,
      country: user.country,
      phone: user.phone,
      email: user.email,
      password: user.password,
      image: image,
      imageBlobUrl: user.imageBlobUrl,
      PartitionKey: user.partitionKey,
      RowKey: user.rowKey,
      Timestamp: user.timestamp,
      ETag: user.eTag,
      newImage: image?.name ? true : false,
    };

    const response: AxiosResponse = await axios.post(
      API_ENDPOINT + "user/update",
      updateUser,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "multipart/form-data",
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

export default UpdateUserInformationService;
