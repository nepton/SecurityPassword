namespace Doulex.SaltedPassword
{
    /// <summary>
    /// password strength score
    /// </summary>
    public interface IPasswordStrengthRank
    {
        /// <summary>
        /// Calculate the strength of the password. The total score is 5
        /// </summary>
        /// <param name="strInputPassword"></param>
        /// <returns></returns>
        int ComputePasswordStrength(string strInputPassword);
    }
}