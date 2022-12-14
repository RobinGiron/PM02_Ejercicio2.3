using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarea2_3.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tarea2_3
{
    public partial class MainPage : ContentPage
    {
        AudioRecorderService recorder = new AudioRecorderService () ;
        private readonly AudioRecorderService audioRecorderService = new AudioRecorderService()
        {
            StopRecordingOnSilence = false,
            StopRecordingAfterTimeout = false
        };

        public MainPage()
        {
            InitializeComponent();
            DisplayAlert("Aviso", "Es necesario dar permiso de  audio y el alacenamiento", "OK");
        }

        private async void btnGrabar_Clicked(object sender, EventArgs e)
        {
            var status = await Permissions.RequestAsync<Permissions.Microphone>();

            if (status != PermissionStatus.Granted)
                return;

            if (!audioRecorderService.IsRecording)
            {
                //recorder = new AudioRecorderService();
                lblTitulo.Text = "Grabando";
                await audioRecorderService.StartRecording();
            }
        }

        private async void btnDetener_Clicked(object sender, EventArgs e)
        {
            if (audioRecorderService.IsRecording)
            {
                lblTitulo.Text = "Audio Detenido";
                await audioRecorderService.StopRecording();

                if (String.IsNullOrWhiteSpace(txtDescripcion.Text)) { txtDescripcion.Text = "--"; }
                AudioPlayer player = new AudioPlayer();
                var filePath = audioRecorderService.GetAudioFilePath();

                var emple = new constructor
                {
                    descripcion = txtDescripcion.Text,
                    ruta = filePath,
                    fecha = DateTime.Now
                };

                var resultado = await App.BaseDatos.EmpleadoGuardar(emple);
                if (resultado != 0)
                {
                    await DisplayAlert("Aviso", "Datos Guardados!!", "OK");
                    await DisplayAlert("Aviso", filePath, "OK");
                    txtDescripcion.Text = "";
                    filePath = "";
                }

            }
        }

        private async void btnPlay_Clicked(object sender, EventArgs e)
        {

            AudioPlayer player = new AudioPlayer();
            var filePath = audioRecorderService.GetAudioFilePath();
            lblTitulo.Text = "Reproduciendo";
            player.Play(filePath);

            lblTitulo.Text = "Sin Grabar";

        }

        private async void btnLista_Clicked(object sender, EventArgs e)
        {
            var newpage = new lista();
            await Navigation.PushAsync(newpage);
        }
    }
}
