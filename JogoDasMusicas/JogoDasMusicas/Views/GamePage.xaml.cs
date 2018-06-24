using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JogoDasMusicas.ViewModels;

namespace JogoDasMusicas.Views
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