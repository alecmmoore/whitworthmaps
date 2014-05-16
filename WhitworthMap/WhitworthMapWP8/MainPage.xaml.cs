using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhitworthMapWP8.Resources;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;
using System.Windows.Media;
using Microsoft.WindowsAzure.MobileServices;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace WhitworthMapWP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // List of events that is queried from the Azure Mobile Service
        private IMobileServiceTable<Event> EventsTable = App.MobileService.GetTable<Event>();

        // Collections for UI Lists
        ObservableCollection<Building> Buildings = new ObservableCollection<Building>();
        ObservableCollection<Event> Events = new ObservableCollection<Event>();

        bool IsShowEvents = false;
        bool IsExpandMap = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Buildings.Add(new Building() { Title = "McEachran Hall", Key = "McEachran", Hor = 712, Ver = 626});
            Buildings.Add(new Building() { Title = "MacKay Hall", Key = "MacKay", Hor = 725, Ver = 719});
            Buildings.Add(new Building() { Title = "Cowles Auditorium", Key = "Cowles", Hor = 623, Ver = 611 });
            Buildings.Add(new Building() { Title = "Music Building", Key = "Music", Hor = 535, Ver = 642 });
            Buildings.Add(new Building() { Title = "Auld House", Key = "Auld", Hor = 516, Ver = 714 });
            Buildings.Add(new Building() { Title = "Dixon Hall", Key = "Dixon", Hor = 617, Ver = 547 });
            Buildings.Add(new Building() { Title = "Warren Hall", Key = "Warren", Hor = 542, Ver = 497 });
            Buildings.Add(new Building() { Title = "Seeley G. Mudd Chapel", Key = "Seeley", Hor = 509, Ver = 424 });
            Buildings.Add(new Building() { Title = "Ballard Hall", Key = "Ballard", Hor = 470, Ver = 406 });
            Buildings.Add(new Building() { Title = "McMillan Hall", Key = "McMillan", Hor = 481, Ver = 333 });
            Buildings.Add(new Building() { Title = "Graves Gym", Key = "Graves", Hor = 388, Ver = 365 });
            Buildings.Add(new Building() { Title = "The Fieldhouse", Key = "Fieldhouse", Hor = 240, Ver = 165 });
            Buildings.Add(new Building() { Title = "Aquatics Center", Key = "Aquatics", Hor = 363, Ver = 166 });
            Buildings.Add(new Building() { Title = "Scotford Conditioning Center", Key = "Conditioning", Hor = 334, Ver = 181 });
            // Buildings.Add(new Building() { Title = "University Recreation Center", Key = "Recreation", Hor = 725, Ver = 719 });
            // Buildings.Add(new Building() { Title = "", Key = "Westside" });
            Buildings.Add(new Building() { Title = "Westminister Hall", Key = "Westminster", Hor = 494, Ver = 204 });
            Buildings.Add(new Building() { Title = "Weyerhaeuser Hall", Key = "Weyerhaeuser", Hor = 573, Ver = 309 });
            Buildings.Add(new Building() { Title = "Lied Center For Visual Arts", Key = "Lied", Hor = 584, Ver = 193});
            Buildings.Add(new Building() { Title = "Schumacher Hall", Key = "Schumacher", Hor = 648, Ver = 276 });
            Buildings.Add(new Building() { Title = "Robinson Science Hall", Key = "Robinson", Hor = 714, Ver = 298 });
            Buildings.Add(new Building() { Title = "Lindaman Center", Key = "Lindaman", Hor = 644, Ver = 396 });
            Buildings.Add(new Building() { Title = "Harriet Cheney Cowles Memorial Library", Key = "Library", Hor = 719, Ver = 397 });
            Buildings.Add(new Building() { Title = "Eric Johnston Science Center", Key = "Johnston", Hor = 814, Ver = 322 });
            Buildings.Add(new Building() { Title = "Baldwin Jenkins Hall", Key = "BJ", Hor = 900, Ver = 246 });
            Buildings.Add(new Building() { Title = "Eileen Hendrick Hall", Key = "Hendrick", Hor = 901, Ver = 363 });
            Buildings.Add(new Building() { Title = "Stewart Hall", Key = "Stewart", Hor = 943, Ver = 326 });
            Buildings.Add(new Building() { Title = "The Village", Key = "Village", Hor = 997, Ver = 284 });
            Buildings.Add(new Building() { Title = "Arend Hall", Key = "Arend", Hor = 854, Ver = 415 });
            Buildings.Add(new Building() { Title = "Boppell", Key = "Boppell", Hor = 1072, Ver = 443 });
            Buildings.Add(new Building() { Title = "Cornerstone", Key = "Cornerstone", Hor = 197, Ver = 710 });
            Buildings.Add(new Building() { Title = "Hixon Union Building", Key = "HUB", Hor = 837, Ver = 470 });
            // Buildings.Add(new Building() { Title = "Hardwick Hall", Key = "Hardwick", Hor = 725, Ver = 719 });
            Buildings.Add(new Building() { Title = "Hawthorne Hall", Key = "Hawthorne", Hor = 1190, Ver = 580 });
            Buildings.Add(new Building() { Title = "Facilities Services Building", Key = "Facilities", Hor = 696, Ver = 197 });
            Buildings.Add(new Building() { Title = "Pirate's Cove", Key = "Cove", Hor = 1082, Ver = 168 });
            Buildings.Add(new Building() { Title = "Duvall Hall", Key = "Duvall", Hor = 1141, Ver = 351 });
            Buildings.Add(new Building() { Title = "Scotford Tennis Center/Cutter Tennis Courts", Key = "Center", Hor = 369, Ver = 114 });
            Buildings.Add(new Building() { Title = "The Pine Bowl", Key = "Pine", Hor = 342, Ver = 465 });
            Buildings.Add(new Building() { Title = "Omache Field", Key = "Omache", Hor = 154, Ver = 589 });
            Buildings.Add(new Building() { Title = "Merkel Field", Key = "Merkel", Hor = 65, Ver = 47 });
            Buildings.Add(new Building() { Title = "Soccer Field", Key = "Soccer", Hor = 136, Ver = 431 });
            Buildings.Add(new Building() { Title = "Marks Field", Key = "Marks", Hor = 242, Ver = 497 });
            Buildings.Add(new Building() { Title = "East Hall", Key = "East", Hor = 1081, Ver = 257 });
            // Buildings.Add(new Building() { Title = "The Loop", Key = "Loop", Hor = 725, Ver = 719 });
            Buildings.Add(new Building() { Title = "Hill Hall", Key = "Hill", Hor = 555, Ver = 750 });
            Buildings.Add(new Building() { Title = "Whit-Pres Quall Hall", Key = "Quall", Hor = 849, Ver = 594 });
            Buildings.Add(new Building() { Title = "The President's House", Key = "President's", Hor = 926, Ver = 741 });
            
            BuildingList.DataContext = Buildings;
        }

        private void Building_Tap(object sender, RoutedEventArgs e)
        {
            Button Building = sender as Button;

            if (Building.Name != "")
            {
                var key = Regex.Match(Building.Name, @"^.*?(?=_)").ToString();
                BuildingSelected(key);
            }
        }

        private void BuildingListBoxItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (BuildingList.SelectedIndex == -1)
                return;

            Building item = (BuildingList.SelectedItem as Building);
            BuildingSelected(item.Key);
            MapPanel.ScrollToHorizontalOffset(item.Hor - 230);
            MapPanel.ScrollToVerticalOffset(item.Ver - 125);
        }

        private void BuildingTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private async void BuildingSelected(string Key)
        {

            // If show events animation has not been played, fire it
            if (!IsShowEvents)
            {
                ShowEvents.Begin();
                IsShowEvents = true;
            }

            if (IsExpandMap)
            {
                ExpandMap();
            }

            // Reset the Events Collection
            Events.Clear();
            // Display loading bar
            EventsLoading.Visibility = Visibility.Visible;

            try
            {
                // Always set newtork issue text to collapsed
                NoNetwork.Visibility = Visibility.Collapsed;
                // Query for those events based on the key
                Events = new ObservableCollection<Event>(await App.MobileService.GetTable<Event>().Where(o => o.Locations.Contains(Key)).Select(o => o).ToListAsync());
            }
            catch (Exception e)
            {
                if (IsShowEvents)
                {
                    NoNetwork.Visibility = Visibility.Visible;
                }
                return;
            }
            // Remove events that have already happened
            DateTime ParsedDate = new DateTime();
            foreach (Event e in Events)
            {
                if (DateTime.TryParse(e.Date, out ParsedDate))
                {
                    var date = ParsedDate.ToString("d").Split(new string[] { "/" }, StringSplitOptions.None);
                    if (date.Length >= 2)
                    {
                        e.Date = String.Format("{0}/{1}", date[0], date[1]);
                    }
                }
            }

            // Set the results of the query to the UI Collection
            EventList.DataContext = Events;
            // Hide loading bar
            EventsLoading.Visibility = Visibility.Collapsed;

            if (Events.Count <= 0)
            {
                NoEvents.Visibility = Visibility.Visible;
            }
            else
            {
                NoEvents.Visibility = Visibility.Collapsed;
            }

        }

        private void ExpandMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ExpandMap();
        }

        private void ExpandMap()
        {
            if (!IsExpandMap)
            {
                ExpandMap_Icon.Source = new BitmapImage(new Uri("/Assets/Smallscreen.png", UriKind.Relative));
                LayoutRoot.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                LayoutRoot.RowDefinitions[2].Height = new GridLength(0);
                LayoutRoot.RowDefinitions[3].Height = new GridLength(0);

                SearchGrid.Visibility = Visibility.Collapsed;
                BuildingListGrid.Visibility = Visibility.Collapsed;
                EventListGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                ExpandMap_Icon.Source = new BitmapImage(new Uri("/Assets/Fullscreen.png", UriKind.Relative));
                LayoutRoot.RowDefinitions[1].Height = new GridLength(250);
                LayoutRoot.RowDefinitions[2].Height = new GridLength(75);
                LayoutRoot.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);

                SearchGrid.Visibility = Visibility.Visible;
                BuildingListGrid.Visibility = Visibility.Visible;
                EventListGrid.Visibility = Visibility.Visible;
            }
            IsExpandMap = !IsExpandMap;
        }
        public async void GetCoord()
        {
            try
            {
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
                MapPanel.ScrollToHorizontalOffset(Calc2 - 230);
                MapPanel.ScrollToVerticalOffset(Calc1 - 125);
            }
            catch (Exception e)
            {

            }
        }

        private void BackButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // Animate back to building list
            ShowBuildings.Begin();
            // Clear events list
            Events.Clear();
            // Set IsShowEvents flag to false
            IsShowEvents = false;
            // Collapse the Events loading bar and the No Events text block
            NoEvents.Visibility = Visibility.Collapsed;
            EventsLoading.Visibility = Visibility.Collapsed;
            NoNetwork.Visibility = Visibility.Collapsed;

        }

        private void EventListBoxItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (EventList.SelectedIndex == -1)
                return;

            Event item = (EventList.SelectedItem as Event);
            string URL = String.Format("/EventDetails.xaml?Title={0}&Date={1}&Time={2}&Description={3}&LocationsString={4}&Contact={5}&ContactPhone={6}&ContactEmail={7}&Link={8}",
                item.Title, item.Date, item.Time, item.Description, item.LocationsString, item.Contact, item.ContactPhone, item.ContactEmail, item.Link);
            NavigationService.Navigate(new Uri(URL, UriKind.Relative));
        }


        private void Location_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GetCoord();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Casts the sender as a TextBox
            TextBox SearchBox = (sender as TextBox);

            if (IsShowEvents)
            {
                // If the search ox is not empty
                if (SearchBox.Text != "")
                {
                    EventList.DataContext = new ObservableCollection<Event>(Events.Where(o => o.Title.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) != -1));
                }
                // If it is visible set all list items to visible
                else
                {
                    EventList.DataContext = Events;
                }  
            }
            else
            {
                // If the search ox is not empty
                if (SearchBox.Text != "")
                {
                    BuildingList.DataContext = new ObservableCollection<Building>(Buildings.Where(o => o.Title.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) != -1));
                }
                // If it is visible set all list items to visible
                else
                {
                    BuildingList.DataContext = Buildings;
                }  
            }

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            LayoutRoot.RowDefinitions[1].Height = new GridLength(0);
            LayoutRoot.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);

            MapPanel.Visibility = Visibility.Collapsed;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            LayoutRoot.RowDefinitions[1].Height = new GridLength(250);
            LayoutRoot.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
            
            MapPanel.Visibility = Visibility.Visible;
        }

    }
}