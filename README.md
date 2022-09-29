The salted password is used to encrypt the user password. In order to prevent the encrypted password from being guessed,
the Salt processing is added after the encrypted password, so that the same password will generate a different encrypted
content each time

# How to use
Add nuget reference
```
Doulex.SaltedPassword
```

# Example
```
```C#
// Encrypt the password
var saltedPassword = new SaltedPassword();
var encryption     = saltedPassword.CreatePassword("ThisIsThePassword");

// Verify the password
var result = saltedPassword.VerifyPassword("ThisIsThePassword", encryption);
```
