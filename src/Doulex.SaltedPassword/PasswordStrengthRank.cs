using System.Text.RegularExpressions;

namespace Doulex.SaltedPassword;

public class PasswordStrengthRank : IPasswordStrengthRank
{
    /// <summary>
    /// 计算密码强度, 总分5分
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
