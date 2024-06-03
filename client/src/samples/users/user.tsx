import IUser from "../../interfaces/users/user/IUser";

/**
 * Represents a user object with various properties.
 */
const emptyUser: IUser = {
  FirstName: "",
  LastName: "",
  Address: "",
  City: "",
  Country: "",
  Phone: "",
  Email: "",
  Password: "",
  ImageBlobUrl: "",
  Timestamp: "",
  PartitionKey: "User",
  RowKey: "User",
  ETag: ""
};

export default emptyUser;
