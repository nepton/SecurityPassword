// using System;
// using Microsoft.Extensions.DependencyInjection;
// using Security.Infrastructure.VerificationCodes;
// using Security.Infrastructure.VerificationCodes.Implements.Rfc6238;
//
// namespace Security.Infrastructure.Passwords.Implements
// {
//     /// <summary>
//     /// 服务注册
//     /// </summary>
//     public static class ServiceDependencyInjection
//     {
//         /// <summary>
//         /// 
//         /// 添加密码加密服务
//         /// </summary>
//         /// <param name="services"></param>
//         /// <param name="configure"></param>
//         /// <returns></returns>
//         public static IServiceCollection AddPasswordEncryptionService(this IServiceCollection services, Action<PasswordEncryptionConfig> configure = null)
//         {
//             var config = new PasswordEncryptionConfig();
//             configure?.Invoke(config);
//
//             services.AddSingleton(config);
//             services.AddTransient<IVerificationCodeProvider, Rfc6238VerificationCodeProvider>();
//
//             return services;
//         }
//     }
// }
