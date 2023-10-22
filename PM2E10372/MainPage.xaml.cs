using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;
using System.IO;

namespace PM2E10372
{
    public partial class MainPage : ContentPage 
    {

        Plugin.Media.Abstractions.MediaFile photo = null;
        int fotoguardada = 0;
        public MainPage()
        {
            InitializeComponent();
        }

        public string ImageToBase64()
        {
            if (photo != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = photo.GetStream();
                    stream.CopyTo(memory);
                    byte[] data = memory.ToArray();
                    string base64 = Convert.ToBase64String(data);
                    return base64;
                }
            }
            return null;
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
    }
}
