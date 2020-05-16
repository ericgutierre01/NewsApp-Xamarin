using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NewsApp.Views
{
    public partial class WebViewPage : ContentPage
    {
        public WebViewPage(string url)
        {
            InitializeComponent();
            progress_pb.IsVisible = true;
            webView.Source = url;
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
    }
}
