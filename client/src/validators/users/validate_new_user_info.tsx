import IUser from "../../interfaces/users/user/IUser";

/**
 * Validates user data for update.
 * @param user - The user object to validate.
 * @returns An array of error messages indicating fields that failed validation.
 */
const ValidateUpdateData = (user: IUser): string[] => {
    const errors: string[] = [];
  
    // Check if required fields are empty
    if (!user.FirstName.trim()) {
      errors.push("first name");
    }
    if (!user.LastName.trim()) {
      errors.push("last name");
    }
    if (!user.Address.trim()) {
      errors.push("address");
    }
    if (!user.City.trim()) {
      errors.push("city");
    }
    if (!user.Country.trim()) {
      errors.push("country");
    }
    if (!user.Phone.trim()) {
      errors.push("phone number");
    } else if (!/^\d{10}$/.test(user.Phone.trim())) {
      errors.push("phone number");
    } else if (!user.Phone.trim().startsWith("06")) {
      errors.push("phone number");
    }
   
    if (!user.Password.trim()) {
      errors.push("password");
    } else if (user.Password.trim().length < 6) {
      errors.push("password");
    }
  
    return errors;
};

export { ValidateUpdateData };