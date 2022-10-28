# SecurityPassword
[![Build status](https://ci.appveyor.com/api/projects/status/qsc7d2uwxopdx2d8?svg=true)](https://ci.appveyor.com/project/nepton/securitypassword)
[![CodeQL](https://github.com/nepton/SecurityPassword/actions/workflows/codeql.yml/badge.svg)](https://github.com/nepton/SecurityPassword/actions/workflows/codeql.yml)
![GitHub issues](https://img.shields.io/github/issues/nepton/SecurityPassword.svg)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/nepton/SecurityPassword/blob/master/LICENSE)

The salted password is used to encrypt the user password. In order to prevent the encrypted password from being guessed,
the Salt processing is added after the encrypted password, so that the same password will generate a different encrypted
content each time

## Nuget packages

| Name                                 | Version                                                                                                                                                   | Downloads                                                                                                                                                  |
|--------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------|
| SecurityPassword                     | [![nuget](https://img.shields.io/nuget/v/SecurityPassword.svg)](https://www.nuget.org/packages/SecurityPassword/)                                         | [![stats](https://img.shields.io/nuget/dt/SecurityPassword.svg)](https://www.nuget.org/packages/SecurityPassword/)                                         |
| SecurityPassword.DependencyInjection | [![nuget](https://img.shields.io/nuget/v/SecurityPassword.DependencyInjection.svg)](https://www.nuget.org/packages/SecurityPassword.DependencyInjection/) | [![stats](https://img.shields.io/nuget/dt/SecurityPassword.DependencyInjection.svg)](https://www.nuget.org/packages/SecurityPassword.DependencyInjection/) |

## How does it work
The plain text password is hashed by SHA-256. Then we add a salt to the password and hash it again. This salt will be at the front of the hash.

```mermaid
graph LR
    Password:123456 --> 7c4a8d09c... --> 32787a9c9... --> 32787a9c9...+Salt
    Salt--> 32787a9c9...                              
```

## How to use
Add nuget reference
```
PM> Install-Package SecurityPassword
```

add these code to your project

```C#
// Encrypt the password
var saltedPasswordService = new SaltedPasswordService();
var encryption     = saltedPasswordService.CreatePassword("ThisIsThePassword");

// Verify the password
var result = saltedPasswordService.VerifyPassword("ThisIsThePassword", encryption);
```
