using NeoBox.ViewModels;
namespace NeoBox.Views;

public sealed partial class PythonPage : Page
{
    public PythonViewModel ViewModel { get; }
    public PythonPage()
    {
        ViewModel = App.GetService<PythonViewModel>();
        this.InitializeComponent();
    }
}
