# What is this
This code provider generate verification code for phone number or email and check it back from user. 
ensure user has owned his phone number or email

# How to use

A. Register to dependency injection

register to services at startup

```C#
services.AddRfc6238ValidationCodeProvider(options =>
{
    options.EffectiveInSeconds = 300;
    options.CodeLength         = 6;
    options.SecurityToken      = "{869639E9-257C-4415-9A5F-0747EFA179E2}";
});
```

You also can register to services and pull the argument from appsettings.json

1. configure in appsettings.json
```json
"VerificationCode": {
    "securityToken": "<Your security token text>",
    "effectiveInSeconds": 180,
    "codeLength": 6
  }
```

2. add follow code snippet into startup.cs
```C#
services.AddRfc6238ValidationCodeProvider(
    options => Configuration.GetSection("VerificationCode").Bind(options));
```

B. Declare `ITotpProvider` where your want
