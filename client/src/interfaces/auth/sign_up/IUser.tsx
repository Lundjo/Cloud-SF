/**
 * Represents a user object with various properties.
 */
export default interface IUser {
  /**
   * The first name of the user.
   */
  firstName: string;

  /**
   * The last name of the user.
   */
  lastName: string;

  /**
   * The address of the user.
   */
  address: string;

  /**
   * The city of the user.
   */
  city: string;

  /**
   * The country of the user.
   */
  country: string;

  /**
   * The phone number of the user.
   */
  phone: string;

  /**
   * The email address of the user.
   */
  email: string;

  /**
   * The password of the user.
   */
  password: string;

  /**
   * User's profile image.
   */
  image: File | null;
}