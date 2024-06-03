import IUser from "../../../auth/sign_up/IUser";

/**
 * Represents the data structure for updating user information.
 * Extends the IUser interface with additional properties for database metadata.
 */
interface IUpdateUser extends IUser {
  /** The URL of the user's image blob. */
  imageBlobUrl: string;

  /** The partition key of the user in the database. */
  PartitionKey: string;

  /** The row key of the user in the database. */
  RowKey: string;

  /** The timestamp of the user record in the database. */
  Timestamp: string;

  /** The ETag (entity tag) of the user record in the database. */
  ETag: string;

  /** Indicates whether a new image has been uploaded by the user. */
  newImage: boolean;
}

export default IUpdateUser;
