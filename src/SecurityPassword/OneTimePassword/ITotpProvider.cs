namespace SecurityPassword.OneTimePassword
{
    /// <summary>
    /// One-time Password Algorithm (TOTP) interface
    /// see TOTP https://www.rfc-editor.org/rfc/rfc6238
    /// </summary>
    public interface ITotpProvider
    {
        /// <summary>
        /// Generate a one time password for the given time and modifier
        /// </summary>
        /// <param name="modifier">Can be null or mobile number, Email etc</param>
        /// <param name="time">Generates CODE for the specified time</param>
        /// <returns></returns>
        string Generate(string? modifier, DateTime time);

        /// <summary>
        /// Generate a one time password for the modifier, using the current time
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        string Generate(string? modifier);

        /// <summary>
        /// Verify a one time password for the given time and modifier
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="code"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        bool Verify(string? modifier, string code, DateTime time);

        /// <summary>
        /// Verify a one time password for the modifier, based the current time
        /// </summary>
        /// <param name="modifier">The extern hash data</param>
        /// <param name="code">The code to be verify</param>
        /// <returns></returns>
        bool Verify(string? modifier, string code);
    }
}
