using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JogoDasMusicas.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
		}

        public void StartGame()
        {
            this.Navigation.PushAsync(new GamePage());
        }
	}
}