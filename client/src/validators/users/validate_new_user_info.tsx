import IUser from "../../interfaces/users/user/IUser";

/**
 * Validates user data for update.
 * @param user - The user object to validate.
 * @returns An array of error messages indicating fields that failed validation.
 */
const ValidateUpdateData = (user: IUser): string[] => {
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
   
    if (!user.password.trim()) {
      errors.push("password");
    } else if (user.password.trim().length < 6) {
      errors.push("password");
    }
  
    return errors;
};

export { ValidateUpdateData };