using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E10372.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageListSitios : ContentPage
    {
        public PageListSitios()
        {
            InitializeComponent();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            list.ItemsSource = await App.Instancia.listSitios();
        }

    }
}