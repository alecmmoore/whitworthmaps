﻿using System;
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
using System.Globalization;

namespace WhitworthMapWP8
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
            //EN
            //creates object of geolocator type
            Geolocator geolocator = new Geolocator();
            //creates geopostion object, makes requests every 10 seconds for 5 minutes
            Geoposition geopostion = await geolocator.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5), timeout: TimeSpan.FromSeconds(10));
            double LatIt = geopostion.Coordinate.Latitude;
            double LongIt = geopostion.Coordinate.Longitude;
            locationPing.Visibility = Visibility.Visible;
            locationPingShadow.Visibility = Visibility.Visible;
            //calculates the location according to the screen.
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
           //AM
            ViewBuildings.Begin();
        }

        public void WindowResize()
        {
            //EN
            // Gets the scroll containers margin property
            Thickness margin = ScrollContainer.Margin;
            // Gets the current windows resolution percentage
            ResolutionScale resolutionScale = DisplayProperties.ResolutionScale;
            // Converts the resolution percentage to a double
            double resolutionOffset = (Convert.ToDouble(resolutionScale) * 0.01);
            // Gets the current height of the window and multiplies it by the resolution percentage
            double Perfheight = (Window.Current.Bounds.Width * resolutionOffset) * .5625;
            // Elias uses his less magical number
            double answer;
            if (Window.Current.Bounds.Height <= 768)
            {
                double answer1 = (Perfheight * .09);
                double answer2 = Math.Abs(Perfheight - Window.Current.Bounds.Height) + 87;
                answer = answer1 + answer2;
                // Sets the margin properties

            }
            else
            {
                double resCheck = (Window.Current.Bounds.Height - Perfheight) / 2;
                answer = (Perfheight * .09) - resCheck;

            }

            margin.Top = answer;
            margin.Bottom = answer;
            margin.Left = 0;
            margin.Right = 0;
            // Sets the Scroll containers margin
            ScrollContainer.Margin = margin;
        }

        private async void Building_Tapped(object sender, RoutedEventArgs e)
        {
            //AM
            // Casts the object sender as a Framework Element
            FrameworkElement Building = (sender as FrameworkElement);

            // Uses the button name to get the type and key name of the button
            var type = Regex.Match(Building.Name, @"(?<=_)\w*").ToString();
            var key = Regex.Match(Building.Name, @"^.*?(?=_)").ToString();

            // Checks the type of button and gets and sets the title of the building accordingly
            if (type == "ListButton")
            {
                TextBlock BuildingText = (VisualTreeHelper.GetChild(Building, 1) as TextBlock);
                BuildingTitle.Text = BuildingText.Text;
            }
            else if (type == "CanvasButton")
            {
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(this.BuildingsList); i++)
                {
                    FrameworkElement ListItem = (VisualTreeHelper.GetChild(this.BuildingsList, i) as FrameworkElement);
                    string ListItemTitle = (VisualTreeHelper.GetChild(ListItem, 1) as TextBlock).Text;

                    if (ListItem.Name.Contains(key))
                    {
                        BuildingTitle.Text = ListItemTitle;
                    }
                }
            }
            else
            {
                BuildingTitle.Text = "No Building Title";
            }

            // Queries the IMobileServiceTable based on the key name from the button name
            var query = await eventsTable
                .Where(o => o.Locations.Contains(key))
                .Select(o => o)
                .ToCollectionAsync();

            // If there are results from the query then set the event list in the XAML
            // to that query If not then set everything to null and show the no events text
            if (query.Count() > 0)
            {
                EventList.ItemsSource = query;
                EventCount.Text = query.Count().ToString();
                NoEvents.Visibility = Visibility.Collapsed;
            }
            else
            {
                EventList.ItemsSource = null;
                EventCount.Text = "0";
                NoEvents.Visibility = Visibility.Visible;
            }

            // Check whether the BuildingEventsCoontainer is already in view, if not then fire the animation
            var translateX = (BuildingEventsContainer.RenderTransform as CompositeTransform).TranslateX;

            if (translateX == 350)
            {
                ViewEvents.Begin();
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            //AM
            // Casts the sender as a TextBox
            TextBox SearchBox = (sender as TextBox);

            // If the search ox is not empty
            if (SearchBox.Text != "")
            {
                // Loop through the list items and set the to invisible if they do not match the textbox
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(this.BuildingsList); i++)
                {
                    FrameworkElement ListItem = (VisualTreeHelper.GetChild(this.BuildingsList, i) as FrameworkElement);
                    string ListItemTitle = (VisualTreeHelper.GetChild(ListItem, 1) as TextBlock).Text;

                    if (ListItemTitle.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ListItem.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ListItem.Visibility = Visibility.Collapsed;
                    }
                }
            }
            // If it is visible set all list items to visible
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
          //EN
            WindowResize();
        }

        private void appBarOpen_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //EN
            appBar.IsOpen = true;
        }

        private void road_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //EN
            if (RoadLayer.Visibility != Visibility.Collapsed)
                RoadLayer.Visibility = Visibility.Collapsed;
            else
                RoadLayer.Visibility = Visibility.Visible;
        }

        private void text_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //EN
            if (TextLayer.Visibility != Visibility.Collapsed)
                TextLayer.Visibility = Visibility.Collapsed;
            else
                TextLayer.Visibility = Visibility.Visible;
        }





    }
}
