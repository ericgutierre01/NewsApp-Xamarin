using System;
using System.Collections.Generic;
using NewsApp.Models;
using NewsApp.Services;
using Xamarin.Forms;

namespace NewsApp.Views
{
    public partial class NoticiasHotPage : ContentPage
    {
        private RestService api;
        private int startIndex;
        private string topic;

        private List<Noticia> noticias;
        public bool isLoading { get; set; }

        public NoticiasHotPage(string topic)
        {
            InitializeComponent();
            api = new RestService();
            noticias = new List<Noticia>();
            startIndex = 1;
            this.topic = topic;
            Title = topic;
            CargarImagenes(topic);
            CargarNoticias(topic);
        }

        async private void CargarImagenes(string topic)
        {
            try
            {
                sindatos_g.IsVisible = true;
                barr.IsVisible = true;
                var current = Xamarin.Essentials.Connectivity.NetworkAccess;
                if (current != Xamarin.Essentials.NetworkAccess.Internet)
                {
                    barr.IsVisible = false;
                    return;
                }
                var imagenes = new List<ImagenHot>();
                imagenes = await api.GetHotImagenes(topic);

                HotImagenes.ItemsSource = imagenes;

                if (imagenes.Count > 0)
                    sindatos_g.IsVisible = false;

            }
            catch
            {
                await DisplayAlert("Aviso", "Ups algo salio mal", "Aceptar");
            }
            barr.IsVisible = false;
        }

        async private void CargarNoticias(string topic)
        {
            try
            {
                isLoading = true;
                barr.IsVisible = true;
                var current = Xamarin.Essentials.Connectivity.NetworkAccess;
                if (current != Xamarin.Essentials.NetworkAccess.Internet)
                {
                    await DisplayAlert("Aviso", "No tienes acceso a internet", "Entiendo");
                    barr.IsVisible = false;
                    return;
                }
                foreach (Noticia noti in await api.GetNoticias(topic, startIndex.ToString()))
                {
                    noticias.Add(noti);
                }

                listaNoticias.ItemsSource = null;
                listaNoticias.ItemsSource = noticias;

                startIndex = startIndex + 10;
            }
            catch (Exception e)
            {
                await DisplayAlert("Aviso", "Ups, en estos momentos no podemos cargar las noticias.", "Aceptar");
            }
            barr.IsVisible = false;
            isLoading = false;
        }

        async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            barr.IsVisible = true;
            var current = Xamarin.Essentials.Connectivity.NetworkAccess;
            if (current != Xamarin.Essentials.NetworkAccess.Internet)
            {
                await DisplayAlert("Aviso", "No tienes acceso a internet", "Entiendo");
                barr.IsVisible = false;
                return;
            }
            Noticia noti = e.Item as Noticia;
            await Navigation.PushModalAsync(new WebViewPage(noti.Link));
            listaNoticias.SelectedItem = null;
            barr.IsVisible = false;

        }

        public void ItemAppering(object sender, ItemVisibilityEventArgs e)
        {
            if (isLoading || noticias.Count == 0)
            {
                //DisplayAlert("aviso", "no cargando", "Entiendo");
                Console.WriteLine("no cargando");
                return;
            }


            //hit bottom!
            Noticia item = noticias[noticias.Count - 1];
            if ((e.Item as Noticia).Descripcion == item.Descripcion)
            {
                Console.WriteLine("cargando");
                CargarNoticias(topic);
            }
        }

        void Recargar_Clicked(object sender, System.EventArgs e)
        {
            CargarImagenes(topic);
        }

        void Ver_Clicked(object sender, System.EventArgs e)
        {
            if (HotImagenes.IsVisible)
            {
                HotImagenes.IsVisible = false;
                ver_btn.Text = "Mostrar";
            }
            else
            {
                HotImagenes.IsVisible = true;
                ver_btn.Text = "Ocultar";
            }
                

        }

    }
}
