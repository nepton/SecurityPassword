using System.Text.RegularExpressions;

namespace PasswordUtility;

public class PasswordStrengthRank : IPasswordStrengthRank
{
    /// <summary>
    /// Calculate the strength of the password. The total score is 5
    /// </summary>
    /// <param name="strInputPassword"></param>
    /// <returns></returns>
    public int ComputePasswordStrength(string strInputPassword)
    {
        if (strInputPassword == null) throw new ArgumentNullException(nameof(strInputPassword));
        
        int result = 0;

        string[] strPatterns =
        {
            @"[0-9]",
            @"[A-Z]",
            @"[a-z]",
            @"[^a-zA-Z0-9]",
        };

        if (!string.IsNullOrEmpty(strInputPassword))
        {
            foreach (var strPattern in strPatterns)
            {
                if (Regex.IsMatch(strInputPassword, strPattern))
                {
                    result += 1;
                }
            }

            if (strInputPassword.Length >= 8)
            {
                result += 1;
            }
        }

        return result;
    }
}
