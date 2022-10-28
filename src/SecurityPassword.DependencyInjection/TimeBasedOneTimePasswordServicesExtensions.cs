using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecurityPassword.OneTimePassword;

namespace SecurityPassword.DependencyInjection;

public static class TimeBasedOneTimePasswordServicesExtensions
{
    /// <summary>
    /// Adds the time based one time password services.
    /// https://www.rfc-editor.org/rfc/rfc6238 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddTimeBasedOneTimePassword(this IServiceCollection services, IConfiguration configuration)
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
    /// Adds the time based one time password services.
    /// https://www.rfc-editor.org/rfc/rfc6238 
    /// </summary>
    /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> for adding services.</param>
    /// <param name="configureOptions">A delegate to configure the <see cref="T:ThingsboardNetFlurlOptions" />.</param>
    /// <returns></returns>
    public static IServiceCollection AddTimeBasedOneTimePassword(this IServiceCollection services, Action<TotpOptions> configureOptions)
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
        services.AddTransient<ITotpProvider, TotpProvider>();
    }
}
