using PM2E10372.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        sitios itemSeleccionado = null;

        public static string descripcionView;
        public static double longitudView, latitudView;
        public int IdView;
        public static byte[] fotoView;

        public PageListSitios()
        {
            InitializeComponent();
        }

        //private async void ToolbarItem_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new MainPage());
        //}

        private async void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.CurrentSelection.Count>0)
            {
                itemSeleccionado = e.CurrentSelection.FirstOrDefault() as sitios;

                string accion = await DisplayActionSheet("Accion: ", "Cancelar", null, "Ir al Mapa", "Eliminar Sitio");

                if (accion.Equals("Eliminar Sitio"))
                {
                    bool respuesta = await DisplayAlert("Accion", "Desea Elminarlo?", "Si", "No");
                    if (respuesta)
                    {
                        await App.Instancia.DeleteSitios(itemSeleccionado);
                        list.ItemsSource = await App.Instancia.listSitios();
                        itemSeleccionado = null;
                    }

                }
                if(accion.Equals("Ir al Mapa"))
                {
                    await Navigation.PushAsync(new Views.PageMaps());

                    IdView = itemSeleccionado.Id;
                    descripcionView = itemSeleccionado.descripcion;
                    latitudView = itemSeleccionado.latitud;
                    longitudView = itemSeleccionado.longitud;
                    fotoView = itemSeleccionado.foto;
                }

                list.SelectedItem = null;
            }

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            list.ItemsSource = await App.Instancia.listSitios();
        }

    }
}