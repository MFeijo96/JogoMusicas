using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JogoDasMusicas.ViewModel;

namespace JogoDasMusicas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GamePage : ContentPage
	{
		public GamePage ()
		{
			InitializeComponent ();

            BindingContext = new GameViewModel(); 
        }
    }
}