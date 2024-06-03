import IUser from "../../../interfaces/auth/sign_up/IUser";

/**
 * Validates the user object to ensure all required fields are present and meet certain criteria.
 * Returns an array of error messages, empty if no errors are found.
 * @param user The user object to validate
 * @returns Array of error messages
 */
const ValidateSignupData = (user: IUser): string[] => {
  const errors: string[] = [];

  // Check if required fields are empty
  if (!user.firstName.trim()) {
    errors.push("first name");
  }
  if (!user.lastName.trim()) {
    errors.push("last name");
  }
  if (!user.address.trim()) {
    errors.push("address");
  }
  if (!user.city.trim()) {
    errors.push("city");
  }
  if (!user.country.trim()) {
    errors.push("country");
  }
  if (!user.phone.trim()) {
    errors.push("phone number");
  } else if (!/^\d{10}$/.test(user.phone.trim())) {
    errors.push("phone number");
  } else if (!user.phone.trim().startsWith("06")) {
    errors.push("phone number");
  }
  if (!user.email.trim()) {
    errors.push("email");
  } else if (!/\S+@\S+\.\S+/.test(user.email.trim())) {
    errors.push("email");
  }
  if (!user.password.trim()) {
    errors.push("password");
  } else if (user.password.trim().length < 6) {
    errors.push("password");
  }

  if(user.image == null) {
    errors.push("image");
  }

  return errors;
};
export { ValidateSignupData };
