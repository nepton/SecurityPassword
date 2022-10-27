namespace SecurityPassword.OneTimePassword
{
    /// <summary>
    /// The options for the TOTP generator.
    /// </summary>
    public class TotpOptions
    {
        /// <summary>
        /// The signed security key, which is a string
        /// </summary>
        public string? SecurityToken { get; set; }

        /// <summary>
        /// Validity period of the verification code
        /// </summary>
        public int EffectiveInSeconds { get; set; } = 180;

        /// <summary>
        /// Length of verification code, [2,8]
        /// </summary>
        public int CodeLength { get; set; } = 6;

        /// <summary>
        /// The has algorithm type
        /// </summary>
        public HashAlgorithmType HashType { get; set; } = HashAlgorithmType.SHA1;

        /// <summary>
        /// A time step for backward validation 
        /// </summary>
        public int TimeStepBackward { get; set; } = 1;

        /// <summary>
        /// A time step for forward validation
        /// </summary>
        public int TimeStepForward { get; set; } = 1;
    }
}
