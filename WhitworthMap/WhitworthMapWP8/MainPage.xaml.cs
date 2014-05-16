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
            
            Buildings.Add(new Building() { Title = "McEachran Hall", Key = "McEachran" });
            Buildings.Add(new Building() { Title = "MacKay Hall", Key = "MacKay" });
            Buildings.Add(new Building() { Title = "Cowles Auditorium", Key = "Cowles" });
            Buildings.Add(new Building() { Title = "Music Building", Key = "Music" });
            Buildings.Add(new Building() { Title = "Auld House", Key = "Auld" });
            Buildings.Add(new Building() { Title = "Dixon Hall", Key = "Dixon" });
            Buildings.Add(new Building() { Title = "Warren Hall", Key = "Warren" });
            Buildings.Add(new Building() { Title = "Seeley G. Mudd Chapel", Key = "Seeley" });
            Buildings.Add(new Building() { Title = "Ballard Hall", Key = "Ballard" });
            Buildings.Add(new Building() { Title = "McMillan Hall", Key = "McMillan" });
            Buildings.Add(new Building() { Title = "Graves Gym", Key = "Graves" });
            Buildings.Add(new Building() { Title = "The Fieldhouse", Key = "Fieldhouse" });
            Buildings.Add(new Building() { Title = "Aquatics Center", Key = "Aquatics" });
            Buildings.Add(new Building() { Title = "Scotford Conditioning Center", Key = "Conditioning" });
            Buildings.Add(new Building() { Title = "University Recreation Center", Key = "Recreation" });
            //Buildings.Add(new Building() { Title = "", Key = "Westside" });
            Buildings.Add(new Building() { Title = "Westminister Hall", Key = "Westminster" });
            Buildings.Add(new Building() { Title = "Weyerhaeuser Hall", Key = "Weyerhaeuser" });
            Buildings.Add(new Building() { Title = "Lied Center For Visual Arts", Key = "Lied" });
            Buildings.Add(new Building() { Title = "Schumacher Hall", Key = "Schumacher" });
            Buildings.Add(new Building() { Title = "Robinson Science Hall", Key = "Robinson" });
            Buildings.Add(new Building() { Title = "Lindaman Center", Key = "Lindaman" });
            Buildings.Add(new Building() { Title = "Harriet Cheney Cowles Memorial Library", Key = "Library" });
            Buildings.Add(new Building() { Title = "Eric Johnston Science Center", Key = "Johnston" });
            Buildings.Add(new Building() { Title = "Baldwin Jenkins Hall", Key = "Baldwin-Jenkins" });
            Buildings.Add(new Building() { Title = "Eileen Hendrick Hall", Key = "Hendrick" });
            Buildings.Add(new Building() { Title = "Stewart Hall", Key = "Stewart" });
            Buildings.Add(new Building() { Title = "The Village", Key = "Village" });
            Buildings.Add(new Building() { Title = "Arend Hall", Key = "Arend" });
            Buildings.Add(new Building() { Title = "Boppell", Key = "Boppell" });
            Buildings.Add(new Building() { Title = "Cornerstone", Key = "Cornerstone" });
            Buildings.Add(new Building() { Title = "Hixon Union Building", Key = "HUB" });
            Buildings.Add(new Building() { Title = "Hardwick Hall", Key = "Hardwick" });
            Buildings.Add(new Building() { Title = "Hawthorne Hall", Key = "Hawthorne" });
            Buildings.Add(new Building() { Title = "Facilities Services Building", Key = "Facilities" });
            Buildings.Add(new Building() { Title = "Pirate's Cove", Key = "Cove" });
            Buildings.Add(new Building() { Title = "Duvall Hall", Key = "Duvall" });
            Buildings.Add(new Building() { Title = "Scotford Tennis Center/Cutter Tennis Courts", Key = "Tennis" });
            Buildings.Add(new Building() { Title = "The Pine Bowl", Key = "Pine" });
            Buildings.Add(new Building() { Title = "Omache Field", Key = "Omache" });
            Buildings.Add(new Building() { Title = "Merkel Field", Key = "Merkel" });
            Buildings.Add(new Building() { Title = "Soccer Field", Key = "Soccer" });
            Buildings.Add(new Building() { Title = "Marks Field", Key = "Marks" });
            Buildings.Add(new Building() { Title = "East Hall", Key = "East" });
            Buildings.Add(new Building() { Title = "The Loop", Key = "Loop" });
            Buildings.Add(new Building() { Title = "Hill Hall", Key = "Hill" });
            Buildings.Add(new Building() { Title = "Whit-Pres Quall Hall", Key = "Quall" });
            Buildings.Add(new Building() { Title = "The President's House", Key = "President's" });

     
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
            }
            catch (Exception e)
            {
                
            }
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
                NoNetwork.Visibility = Visibility.Visible;
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
                ListGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                ExpandMap_Icon.Source = new BitmapImage(new Uri("/Assets/Fullscreen.png", UriKind.Relative));
                LayoutRoot.RowDefinitions[1].Height = new GridLength(250);
                LayoutRoot.RowDefinitions[2].Height = new GridLength(75);
                LayoutRoot.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);

                SearchGrid.Visibility = Visibility.Visible;
                ListGrid.Visibility = Visibility.Visible;
            }
            IsExpandMap = !IsExpandMap;
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

    }
}