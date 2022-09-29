namespace Doulex.SaltedPassword.Tests;

/// <summary>
/// Password strength rank tests
/// </summary>
public class PasswordStrengthRankTests
{
    [Fact]
    public void TestPasswordStrength()
    {
        var tests = new Dictionary<string, int>()
        {
            [""]             = 0,
            ["123"]          = 1,
            ["abc"]          = 1,
            ["!@#"]          = 1,
            ["123abc"]       = 2,
            ["abcABC"]       = 2,
            ["1!a"]          = 3,
            ["1!aA"]         = 4,
            ["123!@#abcABC"] = 5,
        };

        var rank = new PasswordStrengthRank();
        foreach (var test in tests)
        {
            Assert.Equal(test.Value, rank.ComputePasswordStrength(test.Key));
        }
    }

    [Fact]
    public void TestNullPassword()
    {
        var rank = new PasswordStrengthRank();
        Assert.Throws<ArgumentNullException>(() => rank.ComputePasswordStrength(null!));
    }
}
