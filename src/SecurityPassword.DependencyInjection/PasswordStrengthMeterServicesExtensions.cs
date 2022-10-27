using System;
using Microsoft.Extensions.DependencyInjection;
using SecurityPassword.PasswordStrength;

namespace Thingsboard.Net.Flurl.DependencyInjection;

public static class PasswordStrengthMeterServicesExtensions
{
    /// <summary>
    /// Adds the password strength meter services.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddPasswordStrengthMeter(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddImplements();
        return services;
    }

    private static void AddImplements(this IServiceCollection services)
    {
        services.AddTransient<IPasswordStrengthMeter, PasswordStrengthMeter>();
    }
}
