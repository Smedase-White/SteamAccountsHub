using Avalonia;
using Avalonia.Controls;

namespace SteamAccountsHub.Avalonia.Utils;

//Класс, созданный для нормальной работы с ужасным кодом авалонии
public static class ResourceExtension
{
    public static T? GetResource<T>(string key) where T : class
    {
        Application.Current!.TryGetResource(key, out object? resource);
        return resource as T;
    }

    public static T? FindResource<T>(string key) where T : class
    {
        Application.Current!.TryFindResource(key, out object? resource);
        return resource as T;
    }
}
