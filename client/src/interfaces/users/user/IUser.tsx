/**
 * Represents a user object.
 */
interface IUser {
  /** The first name of the user. */
  firstName: string;

  /** The last name of the user. */
  lastName: string;

  /** The address of the user. */
  address: string;

  /** The city of the user. */
  city: string;

  /** The country of the user. */
  country: string;

  /** The phone number of the user. */
  phone: string;

  /** The email address of the user. */
  email: string;

  /** The password of the user. */
  password: string;

  /** The URL of the user's image blob. */
  imageBlobUrl: string;

  /** The partition key of the user. */
  partitionKey: string;

  /** The row key of the user. */
  rowKey: string;

  /** The timestamp of the user record. */
  timestamp: string;

  /** The ETag of the user record. */
  eTag: string;
}

export default IUser;
