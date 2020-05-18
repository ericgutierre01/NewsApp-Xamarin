using System;
using NewsApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewsApp.Views
{
    public partial class WebViewPage : ContentPage
    {
        private Noticia noticia;

        public WebViewPage(Noticia noti)
        {
            InitializeComponent();
            progress_pb.IsVisible = true;
            noticia = noti;
            webView.Source = noti.Link;
            titulo_lb.Text = noti.LinkMostrar;
        }

        async void webviewNavigating(object sender, WebNavigatingEventArgs e)
        {
            await progress_pb.ProgressTo(0.9, 1200, Easing.SpringIn);
        }

        void webviewNavigated(object sender, WebNavigatedEventArgs e)
        {
            progress_pb.Progress = 10;
            progress_pb.IsVisible = false;
            progress_pb.Progress = 0;

        }

        private async void OnCloseButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync(false);
        }

        private async void Abriropciones_Clicked(object sender, EventArgs args)
        {
            if (menu_g.IsVisible)
                menu_g.IsVisible = false;
            else
                menu_g.IsVisible = true;
        }

        public async void CompartirEnlace(object sender, EventArgs args)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = noticia.Link,
                Title = noticia.Titulo
            });
        }

        public async void AbrirNavegador(object sender, EventArgs args)
        {
            await Browser.OpenAsync(noticia.Link, BrowserLaunchMode.External);
        }

        public async void CopiarEncale(object sender, EventArgs args)
        {
            await Clipboard.SetTextAsync(noticia.Link);
            await DisplayAlert("Aviso", "Enlace copiado correctamente", "Entendido");
        }

        protected override bool OnBackButtonPressed()
        {
            if (menu_g.IsVisible)
            {
                menu_g.IsVisible = false;
            }
            else
            {
                Navigation.PopModalAsync();
            }
            return true;
        }
    }
}
