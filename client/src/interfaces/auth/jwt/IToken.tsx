/**
 * Represents a token object containing a JWT token.
 */
interface IToken {
    /**
     * The JWT token string.
     */
    token: string | null;

    /**
     * The user email.
     */
    email?: string;
}

export default IToken;
