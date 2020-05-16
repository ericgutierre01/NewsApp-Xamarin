using NewsApp.Models;
using NewsApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewsApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        List<HomeMenuItem> menuItems;
        private RestService api;
        public MenuPage()
        {
            InitializeComponent();
            api = new RestService();
            version_lb.Text = "Versión: Beta-" + Xamarin.Essentials.AppInfo.VersionString;
            Nusuario_lb.Text = Xamarin.Essentials.AppInfo.Name;
            menuItems = new List<HomeMenuItem>();
            CargarMenu();

            listMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                HomeMenuItem id = e.SelectedItem as HomeMenuItem;
                barr.IsVisible = true;
                var mdPage = Application.Current.MainPage as MasterDetailPage;
                var navPage = mdPage.Detail as NavigationPage;

                switch (id.menId)
                {
                    case 1:
                        await navPage.PopToRootAsync(false);
                        break;

                    default:
                        if(id.menIsHot)
                            await navPage.PushAsync(new NoticiasHotPage(id.menTitulo));
                        else
                            await navPage.PushAsync(new NoticiasPage(id.menTitulo));
                        break;

                }

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);
                barr.IsVisible = false;


                try
                {
                    if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
                        masterDetailPage.IsPresented = false;
                }
                catch
                {
                }
                listMenu.SelectedItem = null;
            };
        }

        private async void CargarMenu()
        {
            try
            {
                //sinNet_g.IsVisible = true;
                barr.IsVisible = true;
                var current = Xamarin.Essentials.Connectivity.NetworkAccess;
                if (current != Xamarin.Essentials.NetworkAccess.Internet)
                {
                    barr.IsVisible = false;
                    return;
                }

                menuItems = await api.GetMenu();

                foreach(HomeMenuItem item in menuItems)
                {
                    item.Imagen = Util.FontConverter.Converter(item.Imagen);
                }
                listMenu.ItemsSource = menuItems;
                if (menuItems.Count > 0)
                    sinNet_g.IsVisible = false;

            }
            catch
            {
            }
            barr.IsVisible = false;
        }

        void Recargar_Clicked(object sender, System.EventArgs e)
        {
            CargarMenu();
        }


    }
}