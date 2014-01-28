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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Windows.Graphics.Display;

namespace WhitworthMap
{

    public sealed partial class MainPage : Page
    {

        // List of events that is queried from the Azure Mobile Service
        private IMobileServiceTable<Event> eventsTable = App.MobileService.GetTable<Event>();

        public MainPage()
        {
            this.InitializeComponent();
            GetCoord();
            WindowResize();
        }

        public async void GetCoord() 
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
        
        private void BackButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewBuildings.Begin();
        }

        private void Title_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewEvents.Begin();
        }

        public void WindowResize() 
        {
            // Gets the scroll containers margin property
            Thickness margin = ScrollContainer.Margin;
            // Gets the current windows resolution percentage
            ResolutionScale resolutionScale = DisplayProperties.ResolutionScale;
            // Converts the resolution percentage to a double
            double resolutionOffset = (Convert.ToDouble(resolutionScale) * 0.01);
            // Gets the current height of the window and multiplies it by the resolution percentage
            double height = Window.Current.Bounds.Height * resolutionOffset;
            // Elias uses his less magical number
            double answer = (height - 768) / 4;
            // Sets the margin properties
            margin.Top = answer;
            margin.Bottom = answer;
            margin.Left = 0;
            margin.Right = 0;
            // Sets the Scroll containers margin
            ScrollContainer.Margin = margin;
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

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox SearchBox = (sender as TextBox);

            if (SearchBox.Text != "")
            {
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(this.BuildingsList); i++)
                {
                    FrameworkElement ListItem = (VisualTreeHelper.GetChild(this.BuildingsList, i) as FrameworkElement);
                    string ListItemTitle = (VisualTreeHelper.GetChild(ListItem, 1) as TextBlock).Text;

                    if (ListItemTitle.Contains(SearchBox.Text))
                    {
                        ListItem.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ListItem.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(this.BuildingsList); i++)
                {
                    FrameworkElement ListItem = (VisualTreeHelper.GetChild(this.BuildingsList, i) as FrameworkElement);
                    ListItem.Visibility = Visibility.Visible;
                }
            }
        }

        private void MapContainer_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            WindowResize();
        }

    }
}
