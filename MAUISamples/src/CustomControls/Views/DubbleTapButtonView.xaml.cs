using CommunityToolkit.Maui.Alerts;
using CustomControls.ViewModel;
using System.Collections.ObjectModel;

namespace CustomControls.Views;

public partial class DubbleTapButtonView : ContentPage
{
    public DubbleTapButtonView()
    {
        InitializeComponent();
        BindingContext = new DoubleTapViewModel();
    }

    private void OnDubbleTapButtonClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Success", "You clicked the button", "Ok");
    }
}