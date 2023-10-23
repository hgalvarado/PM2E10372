using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;
using System.IO;
using Xamarin.Forms.Maps;
using System.Threading;
using Plugin.Geolocator;
using Xamarin.Essentials;

namespace PM2E10372
{
    public partial class MainPage : ContentPage 
    {

        Plugin.Media.Abstractions.MediaFile photo = null;
        int fotoguardada = 0;
        private bool permissionRequested = false;
        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var connection = Connectivity.NetworkAccess;
            var local = CrossGeolocator.Current;

            if (connection == NetworkAccess.Internet)
            {
                if (local == null || !local.IsGeolocationAvailable || !local.IsGeolocationEnabled)
                {
                    // Si la geolocalización no está disponible o no está habilitada
                    await VerificarPermisosGeolocalizacion();
                }
                else
                {
                    await ObtenerLocalizacion();
                }
            }
            else
            {
                await DisplayAlert("Sin Conexion", "Revise su Conexion a Internet", "OK");
            }
        }


        public byte[] ImageToArrayByte()
        {
            if (photo != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = photo.GetStream();
                    stream.CopyTo(memory);
                    byte[] data = memory.ToArray();
                    return data;
                }
            }
            return null;
        }


        private void btnSalirApp_Clicked(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private async void btnAgregar_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtDescripcion.Text)|| fotoguardada==0)
            {
                await DisplayAlert("Campos Vacios", "Debe de completar todos los campos", "Ok");
            }
            else
            {
                var sites = new Models.sitios
                {
                    latitud = Convert.ToDouble(txtLatitud.Text),
                    longitud = Convert.ToDouble(txtLongitud.Text),
                    descripcion = txtDescripcion.Text,
                    foto = ImageToArrayByte()
                };
                if (await App.Instancia.addSitios(sites) > 0)
                {
                    await DisplayAlert("Aviso", "Sitio Agregado", "Ok");
                }
                else
                {
                    await DisplayAlert("Alerta", "Ocurrio un error, revise si no hay campos vacios", "Ok");
                }
            }
            
        }

        private async void btnTomarFoto_Clicked(object sender, EventArgs e)
        {
            
            photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "MIALBUM",
                Name = "Foto.jpg",
                SaveToAlbum = true

            });

            if (photo != null)
            {
                fotoguardada = 1;
                imgFotoTomada.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            }
            else 
            {
                fotoguardada = 0;
            }

        }

        private async void btnListaSitios_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.PageListSitios());
        }


        private async Task VerificarPermisosGeolocalizacion()
        {
            try
            {
                var estadoPermisos = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (estadoPermisos != PermissionStatus.Granted)
                {
                    if (estadoPermisos == PermissionStatus.Denied && !permissionRequested)
                    {
                        estadoPermisos = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                        permissionRequested = true; // Marcar que hemos mostrado el diálogo.
                    }
                    else if (estadoPermisos == PermissionStatus.Denied)
                    {
                        await DisplayAlert("Aviso", "Los permisos fueron denegados, esta aplicacion necesita los permisos de Ubicacion para poder funcionar correctamente", "OK");
                    }

                }

                if (estadoPermisos == PermissionStatus.Granted)
                {
                    await ObtenerLocalizacion();
                }
            }
            catch (Exception ex)
            {
                // Maneja cualquier error.
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task ObtenerLocalizacion()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var localizacion = await Geolocation.GetLocationAsync(request);

                if (localizacion != null)
                {
                    txtLatitud.Text = "" + localizacion.Latitude;
                    txtLongitud.Text = "" + localizacion.Longitude;
                }

            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Aviso", "Favor habilite los permisos", "OK");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Aviso", fnsEx + "", "OK");
            }
            
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex + "", "OK");
            }
        }

    }
}
