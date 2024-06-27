using System;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

using SteamAccountsHub.Avalonia.ViewModels.Bases;

namespace SteamAccountsHub.Avalonia;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        string name = data.GetType().FullName!.Replace("ViewModel", "View");
        Type? type = Type.GetType(name);
        if (type is not null)
            return Activator.CreateInstance(type) as Control;
        return new TextBlock { Text = $"Not found: {name}" };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
