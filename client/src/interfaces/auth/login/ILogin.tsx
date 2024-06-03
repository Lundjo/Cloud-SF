/**
 * Interface representing user login credentials.
 */
interface ILogin {
    /**
     * Email address of the user.
     */
    email: string;
    
    /**
     * Password of the user.
     */
    password: string;
}

export default ILogin;