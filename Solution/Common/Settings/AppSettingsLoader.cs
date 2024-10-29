using Microsoft.Extensions.Configuration;

namespace Common.Configurations.Base;

public static class AppSettingsLoader
{
    private static readonly IConfiguration Configuration;

    static AppSettingsLoader()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static T GetSettings<T>(string sectionName) where T : new()
    {
        var settings = new T();
        Configuration.GetSection(sectionName).Bind(settings);
        return settings;
    }
}