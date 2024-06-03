/**
 * Represents a user object.
 */
interface IUser {
  /** The first name of the user. */
  FirstName: string;

  /** The last name of the user. */
  LastName: string;

  /** The address of the user. */
  Address: string;

  /** The city of the user. */
  City: string;

  /** The country of the user. */
  Country: string;

  /** The phone number of the user. */
  Phone: string;

  /** The email address of the user. */
  Email: string;

  /** The password of the user. */
  Password: string;

  /** The URL of the user's image blob. */
  ImageBlobUrl: string;

  /** The partition key of the user. */
  PartitionKey: string;

  /** The row key of the user. */
  RowKey: string;

  /** The timestamp of the user record. */
  Timestamp: string;

  /** The ETag of the user record. */
  ETag: string;
}

export default IUser;
