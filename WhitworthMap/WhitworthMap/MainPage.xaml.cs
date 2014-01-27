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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.MobileServices;

namespace WhitworthMap
{
    
    public sealed partial class MainPage : Page
    {

        //private MobileServiceCollection<Event, Event> events;
        private IMobileServiceTable<Event> eventsTable = App.MobileService.GetTable<Event>();

        public MainPage()
        {
            this.InitializeComponent();
            GetCoord();
        }

        public async void GetCoord() 
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

        private void BackButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewBuildings.Begin();
        }

        private void Title_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewEvents.Begin();
        }

        private async void Building_Tapped(object sender, RoutedEventArgs e)
        {
            FrameworkElement Building = (sender as FrameworkElement);
            TextBlock BuildingText = (VisualTreeHelper.GetChild(Building, 1) as TextBlock);

            var key = Regex.Match(Building.Name, @"^.*?(?=_)").ToString();

            var query = await eventsTable
                .Where(o => o.Locations.Contains(key))
                .Select(o => o)
                .ToCollectionAsync();

            BuildingTitle.Text = BuildingText.Text;

            if (query.Count() > 0)
            {
                EventList.ItemsSource = query;
                NoEvents.Visibility = Visibility.Collapsed;
            }
            else
            {
                EventList.ItemsSource = null;
                NoEvents.Visibility = Visibility.Visible;
            }

            ViewEvents.Begin();
        }

    }
}
