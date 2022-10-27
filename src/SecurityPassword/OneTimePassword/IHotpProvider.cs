namespace SecurityPassword.OneTimePassword
{
    /// <summary>
    /// One-time Password Algorithm (HOTP) interface
    /// See HOTP https://www.rfc-editor.org/rfc/rfc4226
    /// </summary>
    public interface IHotpProvider
    {
        /// <summary>
        /// Generate a one time password for the given time and modifier
        /// </summary>
        /// <param name="modifier">Can be null or mobile number, Email etc</param>
        /// <param name="counter">The counter for the one time password</param>
        /// <returns></returns>
        string Generate(string? modifier, long counter);

        /// <summary>
        /// Verify a one time password for the given time and modifier
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="code"></param>
        /// <param name="counter">The counter for the one time password</param>
        /// <returns></returns>
        bool Verify(string? modifier, string code, long counter);
    }
}
