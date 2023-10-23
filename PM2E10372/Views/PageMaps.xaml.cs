using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using static Xamarin.Essentials.Permissions;

namespace PM2E10372.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMaps : ContentPage
    {
        public PageMaps()
        {
            InitializeComponent();
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var conexion = Connectivity.NetworkAccess;
            var locl = CrossGeolocator.Current;

            if (conexion == NetworkAccess.Internet)
            {
                if (locl != null || !locl.IsGeolocationAvailable && !locl.IsGeolocationEnabled) 
                {
                    var pinEstatico = new Pin
                    {
                        Type = PinType.Place,
                        Position = new Xamarin.Forms.Maps.Position(PageListSitios.latitudView, PageListSitios.longitudView),
                        Label = PageListSitios.descripcionView,
                    };

                    variableMostrarMapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(
                        PageListSitios.latitudView, PageListSitios.longitudView), Distance.FromKilometers(1)));
                    variableMostrarMapa.Pins.Add(pinEstatico);
                    variableMostrarMapa.IsShowingUser = true;

                }
                else
                {
                    if (!locl.IsGeolocationEnabled)
                    {
                        await DisplayAlert("Aviso", "activa el GPS para continuar.", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Aviso", "Por favor, revisa tu conexion a internet para continuar.", "OK");
            }
        }

        private void Locl_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            var mapcenter = new Xamarin.Forms.Maps.Position(e.Position.Latitude, e.Position.Longitude);
            variableMostrarMapa.MoveToRegion(new Xamarin.Forms.Maps.MapSpan(mapcenter, 1, 1));

        }

        private async void compartirImagen(object sender, EventArgs e)
        {
            await ShareImage(PageListSitios.fotoView, "foto.jpg");
        }
        async Task ShareImage(byte[] imageData, string filename)
        {
            var file = Path.Combine(FileSystem.CacheDirectory, filename);
            File.WriteAllBytes(file, imageData);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Compartir imagen",
                File = new ShareFile(file)
            });
        }
    }
}