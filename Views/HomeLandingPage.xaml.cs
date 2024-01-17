using Microsoft.Windows.Security.AccessControl;

namespace NeoBox.Views;

public sealed partial class HomeLandingPage : Page
{
    public string ApplicationInfo { get; set; }

    public string ApplicationName { get; }

    public string ApplicationVersion { get; }

    public HomeLandingViewModel ViewModel { get; }

    public HomeLandingPage()
    {
        ViewModel = App.GetService<HomeLandingViewModel>();
        this.InitializeComponent();
        ApplicationInfo = $"{App.Current.AppName} v{App.Current.AppVersion}";
        ApplicationName = $"{App.Current.AppName}";
        ApplicationVersion = $"Version {App.Current.AppVersion}";
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        allLandingPage.GetData(ViewModel.JsonNavigationViewService.DataSource);
        allLandingPage.OrderBy(i => i.Title);
    }
}

