using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhitworthMap
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Get_Coord();
            Window_adjustment();
            Window.Current.SizeChanged += WindowSizeChanged;
        }

        public async void Get_Coord()
        {
            //creates object of geolocator type
            Geolocator geolocator = new Geolocator();
            //creates geopostion object, makes requests every 10 seconds for 5 minutes
            Geoposition geopostion = await geolocator.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5), timeout: TimeSpan.FromSeconds(10));
            double LatIt = geopostion.Coordinate.Latitude;
            double LongIt = geopostion.Coordinate.Longitude;
            locationPing.Visibility = Visibility.Visible;
            locationPingShadow.Visibility = Visibility.Visible;
            double calc1 = (47.757025 - LatIt) * (111545.9883);
            double calc2 = (117.426186 + LongIt) * (83612.52731);
            int Calc1 = Convert.ToInt32(Math.Round(calc1));
            int Calc2 = Convert.ToInt32(Math.Round(calc2));
            Canvas.SetLeft(locationPing, Calc2 - 15);
            Canvas.SetTop(locationPing, Calc1 - 30);
            Canvas.SetLeft(locationPingShadow, Calc2 - 24);
            Canvas.SetTop(locationPingShadow, Calc1 - 14);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void BackButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            ViewBuildings.Begin();
        }

        private void Title_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            ViewEvents.Begin();
        }
        public void Window_adjustment() 
        {
            Thickness margin = ScrollContainer.Margin;
            double answer = (Window.Current.Bounds.Height) * .0731482;
            margin.Top = answer;
            margin.Bottom = answer;
            margin.Left = 0;
            margin.Right = 0;
            ScrollContainer.Margin = margin;
        }
        public void WindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e) 
        {
            Window_adjustment();
        }
    }
}
