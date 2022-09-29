namespace Doulex.SaltedPassword.Tests;

public class SaltedPasswordTests
{
    [Fact]
    public void TestSamePasswordWillGotDifferentResult()
    {
        var password       = "ThisIsThePassword";
        var saltedPassword = new SaltedPassword();
        var encrypted1     = saltedPassword.CreatePassword(password);
        var encrypted2     = saltedPassword.CreatePassword(password);
        Assert.NotNull(encrypted1);
        Assert.NotEqual(encrypted1, encrypted2);
    }

    [Fact]
    public void TestIsStoredPasswordFormatCorrected()
    {
        var password       = "ThisIsThePassword";
        var saltedPassword = new SaltedPassword();
        var encrypted1     = saltedPassword.CreatePassword(password);
        Assert.True(saltedPassword.IsStoredPasswordFormatCorrect(encrypted1));
        
        // assert when password is null
        Assert.False(saltedPassword.IsStoredPasswordFormatCorrect(null!));
    }
    
    [Fact]
    public void TestPasswordCanBeVerified()
    {
        var password       = "ThisIsThePassword";
        var saltedPassword = new SaltedPassword();
        var encrypted1     = saltedPassword.CreatePassword(password);
        Assert.True(saltedPassword.VerifyPassword(password, encrypted1));
    }

    [Fact]
    public void TestEmptyPasswordCanBeVerified()
    {
        var password       = "";
        var saltedPassword = new SaltedPassword();
        var encrypted1     = saltedPassword.CreatePassword(password);
        Assert.NotEmpty(encrypted1);
        Assert.True(saltedPassword.VerifyPassword(password, encrypted1));
    }

    [Fact]
    public void TestPlainTextPasswordCanBeVerified()
    {
        var password = "ThisIsThePassword";
        var saltedPassword = new SaltedPassword(new SaltedPasswordOptions
        {
            PlainPasswordComparable = true
        });

        Assert.True(saltedPassword.VerifyPassword(password, password));
    }

    [Fact]
    public void TestPlainTextPasswordCannotBeVerified()
    {
        var password = "ThisIsThePassword";
        var saltedPassword = new SaltedPassword(new SaltedPasswordOptions
        {
            PlainPasswordComparable = false
        });

        Assert.False(saltedPassword.VerifyPassword(password, password));
    }

    [Fact]
    public void TestNullPassword()
    {
        var saltedPassword = new SaltedPassword();
        Assert.Throws<ArgumentNullException>(() => saltedPassword.CreatePassword(null!));
        Assert.Throws<ArgumentNullException>(() => saltedPassword.VerifyPassword(null!, ""));
        Assert.Throws<ArgumentNullException>(() => saltedPassword.VerifyPassword("", null!));
    }
}
