namespace NeoBox.Views;

public sealed partial class SearchPage : Page
{
    public SearchViewModel ViewModel { get; }
    public SearchPage()
    {
        ViewModel = App.GetService<SearchViewModel>();
        this.InitializeComponent();
    }
}
