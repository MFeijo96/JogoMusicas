using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JogoDasMusicas.ViewModels;

namespace JogoDasMusicas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GamePage : ContentPage
	{
        private Entry AnswerEntry2;
		public GamePage ()
		{
			InitializeComponent ();

            BindingContext = new GameViewModel();
            AnswerEntry2 = this.FindByName<Entry>("AnswerEntry");
            this.FindByName<Button>("myButton").IsEnabled = false;

            Appearing += GetEntryFocus;
            AnswerEntry2.Unfocused += GetEntryFocus;
        }

        private void GetEntryFocus(object sender, EventArgs e)
        {
            if (AnswerEntry2.IsVisible) AnswerEntry2.Focus();
        }
    }
}