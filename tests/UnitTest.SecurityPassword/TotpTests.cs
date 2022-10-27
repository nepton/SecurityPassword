using SecurityPassword.OneTimePassword;

namespace UnitTest.Doulex.PasswordUtility;

public class TotpTests
{
    [Fact]
    public void TestTotp()
    {
        var totp = new TotpProvider(options =>
        {
            options.SecurityToken      = "JBSWY3DPEHPK3PXP";
            options.EffectiveInSeconds = 30;
            options.CodeLength         = 6;
            options.HashType           = HashAlgorithmType.SHA1;
        });
        var code  = totp.Generate("TEST1");
        var check = totp.Verify("TEST1", code);

        Assert.True(check);
    }

    [Fact]
    public void TestTotpWideTimeRange()
    {
        var time                   = DateTime.Now;
        var effectiveTimeInSeconds = 30;

        var totp = new TotpProvider(options =>
        {
            options.SecurityToken      = "JBSWY3DPEHPK3PXP";
            options.EffectiveInSeconds = 30;
            options.CodeLength         = 6;
            options.HashType           = HashAlgorithmType.SHA1;
        });
        var code      = totp.Generate("TEST1", time);
        var afterTime = totp.Verify("TEST1", code, time.AddSeconds(effectiveTimeInSeconds));
        Assert.True(afterTime);

        var beforeTime = totp.Verify("TEST1", code, time.AddSeconds(-effectiveTimeInSeconds));
        Assert.True(beforeTime);
    }

    [Fact]
    public void TestTotpOutOfTime()
    {
        var time                   = DateTime.Now;
        var effectiveTimeInSeconds = 30;

        var totp = new TotpProvider(options =>
        {
            options.SecurityToken      = "JBSWY3DPEHPK3PXP";
            options.EffectiveInSeconds = 30;
            options.CodeLength         = 6;
            options.HashType           = HashAlgorithmType.SHA1;
        });
        var code      = totp.Generate("TEST1", time);
        var afterTime = totp.Verify("TEST1", code, time.AddSeconds(effectiveTimeInSeconds * 2));
        Assert.False(afterTime);

        var beforeTime = totp.Verify("TEST1", code, time.AddSeconds(-effectiveTimeInSeconds * 2));
        Assert.False(beforeTime);
    }

    [Fact]
    public void TestCodeLength()
    {
        for (int i = 2; i <= 8; i++)
        {
            var totp = new TotpProvider(options =>
            {
                options.SecurityToken      = "JBSWY3DPEHPK3PXP";
                options.EffectiveInSeconds = 30;
                options.CodeLength         = i;
                options.HashType           = HashAlgorithmType.SHA1;
            });
            var code = totp.Generate("TEST1");

            Assert.Equal(i, code.Length);
        }
    }
}
