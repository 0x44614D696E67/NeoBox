using System;
using Microsoft.UI.Xaml.Media;
using Microsoft.Win32;
using NeoBox.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NeoBox.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }
    public IThemeService ThemeService;

    private List<string> Cats = new List<string>()
    {
        "XiaoXiao"
    };

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        this.InitializeComponent();
        appTitleBar.Window = App.currentWindow;
        ViewModel.JsonNavigationViewService.Initialize(NavView, NavFrame);
        ViewModel.JsonNavigationViewService.ConfigJson("Assets/NavViewMenu/AppData.json");

        var WinUIManager = WinUIEx.WindowManager.Get(App.currentWindow);
        WinUIManager.MinWidth = 900;
        WinUIManager.MinHeight = 610;
        WinUIManager.Width = 1025;
        WinUIManager.Height = 610;
    }

    private void SetTheme()
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize");
        if (key != null)
        {
            int theme = (int)key.GetValue("AppsUseLightTheme", -1);
            if (theme == 0) //暗黑模式
            {
            }
            else if (theme == 1) //亮色模式
            {
            }
            else //调用失败后兜底
            {
            }
            key.Close();
        }
    }

    private void appTitleBar_BackButtonClick(object sender, RoutedEventArgs e)
    {
        if (NavFrame.CanGoBack)
        {
            NavFrame.GoBack();
        }
    }

    private void appTitleBar_PaneButtonClick(object sender, RoutedEventArgs e)
    {
        NavView.IsPaneOpen = !NavView.IsPaneOpen;
    }

    private void NavFrame_Navigated(object sender, NavigationEventArgs e)
    {
        appTitleBar.IsBackButtonVisible = NavFrame.CanGoBack;
    }

    private void ThemeButton_Click(object sender, RoutedEventArgs e)
    {
        var element = App.currentWindow.Content as FrameworkElement;

        if (element.ActualTheme == ElementTheme.Light)
        {
            element.RequestedTheme = ElementTheme.Dark;
        }
        else if (element.ActualTheme == ElementTheme.Dark)
        {
            element.RequestedTheme = ElementTheme.Light;
        }
    }

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        /*
        var viewModel = NavFrame.GetPageViewModel();
        if (viewModel != null && viewModel is ITitleBarAutoSuggestBoxAware titleBarAutoSuggestBoxAware)
        {
            titleBarAutoSuggestBoxAware.OnAutoSuggestBoxTextChanged(sender, args);
        }*/

        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");

            foreach (var cat in Cats)
            {
                var found = splitText.All((key) =>
                {
                    return cat.ToLower().Contains(key);
                });
                if (found)
                {
                    suitableItems.Add(cat);
                }
            }

            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No Results Found");
            }
            sender.ItemsSource = suitableItems;
        }
    }

    private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        var viewModel = NavFrame.GetPageViewModel();
        if (viewModel != null && viewModel is ITitleBarAutoSuggestBoxAware titleBarAutoSuggestBoxAware)
        {
            titleBarAutoSuggestBoxAware.OnAutoSuggestBoxQuerySubmitted(sender, args);
        }
    }
}

