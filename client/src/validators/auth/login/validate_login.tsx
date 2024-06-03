import ILogin from "../../../interfaces/auth/login/ILogin";

/**
 * Validates the login data.
 * @param user The user login data to be validated.
 * @returns An array of error messages indicating validation failures.
 */
const ValudateLoginData = (user: ILogin): string[] => {
    const errors: string[] = [];
  
    // Check if required fields are empty
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
  
    return errors;
  };
  
  export { ValudateLoginData };
  