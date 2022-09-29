namespace Doulex.SaltedPassword
{
    /// <summary>
    /// salted password service
    /// </summary>
    public interface ISaltedPassword
    {
        /// <summary>
        /// Creates a final password based on the plaintext entered by the user
        /// </summary>
        /// <param name="inputPassword">Prepare the plaintext password to create the final password</param>
        /// <returns>the program returns the final password</returns>
        string CreatePassword(string inputPassword);

        /// <summary>
        /// check the validity of the plaintext password
        /// </summary>
        /// <param name="inputPassword">specifies the plaintext password to be checked</param>
        /// <param name="storedPassword">stored password</param>
        /// <returns>Return True if the two passed passwords match, false otherwise</returns>
        bool VerifyPassword(string inputPassword, string storedPassword);

        /// <summary>
        /// Check that the stored password is in the correct format
        /// </summary>
        /// <param name="storedPassword"></param>
        /// <returns></returns>
        bool IsStoredPasswordFormatCorrect(string? storedPassword);
    }
}
