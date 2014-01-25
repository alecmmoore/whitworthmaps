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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhitworthMap
{
    
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Get_Coord();
        }
        
        public async void Get_Coord() 
        { 
            //creates object of geolocator type
            Geolocator geolocator = new Geolocator();
           //creates geopostion object, makes requests every 10 seconds for f minutes
            Geoposition geopostion = await geolocator.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5), timeout: TimeSpan.FromSeconds(10));
            double LatIt = geopostion.Coordinate.Latitude;
            double LongIt = geopostion.Coordinate.Longitude;
            locationPing.Visibility = Visibility.Visible;
            double calc1 = (47.757025 - LatIt) * (111545.9883);
            double calc2 = (117.426186 + LongIt) * (83612.52731);
            int Calc1 = Convert.ToInt32(Math.Round(calc1));
            int Calc2 = Convert.ToInt32(Math.Round(calc2));
            Canvas.SetLeft(locationPing, Calc2 - 15);
            Canvas.SetTop(locationPing, Calc1 - 15);
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void BackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }
    }
}
