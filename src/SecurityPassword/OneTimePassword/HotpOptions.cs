namespace SecurityPassword.OneTimePassword
{
    /// <summary>
    /// The options for the TOTP generator.
    /// </summary>
    public class HotpOptions
    {
        /// <summary>
        /// The signed security key, which is a string
        /// </summary>
        public string? SecurityToken { get; set; }

        /// <summary>
        /// Length of verification code, [2,8]
        /// </summary>
        public int CodeLength { get; set; } = 6;

        /// <summary>
        /// The has algorithm type
        /// </summary>
        public HashAlgorithmType HashType { get; set; } = HashAlgorithmType.SHA1;
    }
}
