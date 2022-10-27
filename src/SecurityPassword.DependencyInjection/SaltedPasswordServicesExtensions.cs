using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecurityPassword.OneTimePassword;
using SecurityPassword.SaltedPassword;

namespace Thingsboard.Net.Flurl.DependencyInjection;

public static class SaltedPasswordServicesExtensions
{
    /// <summary>
    /// Adds the salted password services.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddSaltedPassword(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        services.Configure<TotpOptions>(configuration);
        services.AddImplements();
        return services;
    }

    /// <summary>
    /// Adds the salted password services.
    /// </summary>
    /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> for adding services.</param>
    /// <param name="configureOptions">A delegate to configure the <see cref="T:TotpOptions" />.</param>
    /// <returns></returns>
    public static IServiceCollection AddThingsboardNet(this IServiceCollection services, Action<TotpOptions> configureOptions)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        if (configureOptions == null)
            throw new ArgumentNullException(nameof(configureOptions));

        services.Configure(configureOptions);
        services.AddImplements();
        return services;
    }

    private static void AddImplements(this IServiceCollection services)
    {
        services.AddTransient<ISaltedPasswordService, SaltedPasswordService>();
    }
}
