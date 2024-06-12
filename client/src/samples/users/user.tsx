import IUser from "../../interfaces/users/user/IUser";

/**
 * Represents a user object with various properties.
 */
const emptyUser: IUser = {
  firstName: "",
  lastName: "",
  address: "",
  city: "",
  country: "",
  phone: "",
  email: "",
  password: "",
  imageBlobUrl: "",
  timestamp: "",
  partitionKey: "User",
  rowKey: "User",
  eTag: ""
};

export default emptyUser;
