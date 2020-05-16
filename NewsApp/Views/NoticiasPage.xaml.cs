using System;
using System.Collections.Generic;
using NewsApp.Models;
using NewsApp.Services;
using Xamarin.Forms;

namespace NewsApp.Views
{
    public partial class NoticiasPage : ContentPage
    {
        private RestService api;
        private int startIndex;
        private string topic;

        private List<Noticia> noticias;
        public bool isLoading { get; set; }

        public NoticiasPage(string topi)
        {
            topic = topi;
            Iniciador();
            Title = topi;

        }

        public NoticiasPage()
        {
            
            topic = "a";
            Iniciador();
            Title = "Hoy";
        }

        private void Iniciador()
        {
            InitializeComponent();
            api = new RestService();
            noticias = new List<Noticia>();
            startIndex = 1;
            CargarNoticias(topic);
        }

        async private void CargarNoticias(string topic)
        {
            try
            {

                sinNet_g.IsVisible = true;
                isLoading = true;
                barr.IsVisible = true;
                var current = Xamarin.Essentials.Connectivity.NetworkAccess;
                if (current != Xamarin.Essentials.NetworkAccess.Internet)
                {
                    await DisplayAlert("Aviso", "No tienes acceso a internet", "Entiendo");
                    barr.IsVisible = false;
                    return;
                }
                foreach(Noticia noti in await api.GetNoticias(topic, startIndex.ToString()))
                {
                    noticias.Add(noti);
                }

                listaNoticias.ItemsSource = null;
                listaNoticias.ItemsSource = noticias;

                if(noticias.Count>0)
                    sinNet_g.IsVisible = false;

                startIndex = startIndex + 10;

            }
            catch (Exception e)
            {
                await DisplayAlert("Aviso", e.Message, "Aceptar");
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
            CargarNoticias(topic);
        }
    }
}
