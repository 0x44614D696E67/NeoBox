using NeoBox.ViewModels;
namespace NeoBox.Views;

public sealed partial class SystemInfoPage : Page
{
    public SystemInfoViewModel ViewModel { get; }
    public SystemInfoPage()
    {
        ViewModel = App.GetService<SystemInfoViewModel>();
        this.InitializeComponent();
    }
}
