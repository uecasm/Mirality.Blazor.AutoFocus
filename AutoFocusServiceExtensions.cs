using Microsoft.Extensions.DependencyInjection;

namespace Mirality.Blazor.AutoFocus;

/// <summary>DI service registration extensions</summary>
public static class AutoFocusServiceExtensions
{
    /// <summary>Adds auto-focus services.</summary>
    /// <param name="services">The services collection</param>
    public static void AddAutoFocus(this IServiceCollection services)
    {
        services.AddScoped<AutoFocusService>();
    }
}
