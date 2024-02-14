using NeoBox.ViewModels;
using Windows.Storage;

namespace NeoBox;
public partial class App : Application
{
    public static Window currentWindow = Window.Current;
    public IServiceProvider Services { get; }
    public new static App Current => (App)Application.Current;
    public string AppVersion { get; set; } = AssemblyInfoHelper.GetAssemblyVersion();
    public string AppName { get; set; } = "NeoBox";
    private static string StringsFolderPath { get; set; } = string.Empty;

    public static T GetService<T>() where T : class
    {
        if ((App.Current as App)!.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public App()
    {
        Services = ConfigureServices();
        this.InitializeComponent();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<IJsonNavigationViewService>(factory =>
        {
            var json = new JsonNavigationViewService();
            json.ConfigDefaultPage(typeof(HomeLandingPage));
            json.ConfigSettingsPage(typeof(SettingsPage));

            return json;
        });

        // Window
        services.AddTransient<MainViewModel>();

        // Search Page
        services.AddTransient<SearchViewModel>();

        // Nav Show
        services.AddTransient<HomeLandingViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<PythonViewModel>();

        // Settings Sub Pages
        services.AddTransient<GeneralSettingViewModel>();
        services.AddTransient<ThemeSettingViewModel>();
        services.AddTransient<AppUpdateSettingViewModel>();
        services.AddTransient<AboutUsSettingViewModel>();

        // UserControls
        services.AddTransient<BreadCrumbBarViewModel>();

        return services.BuildServiceProvider();
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        currentWindow = new Window();

        currentWindow.AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
        currentWindow.AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;

        if (currentWindow.Content is not Frame rootFrame)
        {
            currentWindow.Content = rootFrame = new Frame();
        }

        rootFrame.Navigate(typeof(MainPage));

        currentWindow.Title = currentWindow.AppWindow.Title = $"{AppName} v{AppVersion}";
        currentWindow.AppWindow.SetIcon("Assets/icon.ico");

        currentWindow.Activate();

        await InitializeLocalizer("en-US");
    }

    private async Task InitializeLocalizer(params string[] languages)
    {
        // Initialize a "Strings" folder in the "LocalFolder" for the packaged app.
        if (PackageHelper.IsPackaged)
        {
            // Create string resources file from app resources if doesn't exists.
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder stringsFolder = await localFolder.CreateFolderAsync(
                "Strings",
                CreationCollisionOption.OpenIfExists);
            string resourceFileName = "Resources.resw";
            foreach (var item in languages)
            {
                await LocalizerBuilder.CreateStringResourceFileIfNotExists(stringsFolder, item, resourceFileName);
            }

            StringsFolderPath = stringsFolder.Path;
        }
        else
        {
            // Initialize a "Strings" folder in the executables folder.
            StringsFolderPath = Path.Combine(AppContext.BaseDirectory, "Strings");
            var stringsFolder = await StorageFolder.GetFolderFromPathAsync(StringsFolderPath);
        }

        ILocalizer localizer = await new LocalizerBuilder()
            .AddStringResourcesFolderForLanguageDictionaries(StringsFolderPath)
            .SetOptions(options =>
            {
                options.DefaultLanguage = "en-US";
            })
            .Build();
    }
}

