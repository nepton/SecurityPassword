namespace SecurityPassword.SaltedPassword
{
    /// <summary>
    /// password encryption options
    /// </summary>
    public class SaltedPasswordOptions
    {
        /// <summary>
        /// Specifies whether plaintext passwords are allowed to be stored
        /// </summary>
        public bool PlainPasswordComparable { get; set; }
    }
}
