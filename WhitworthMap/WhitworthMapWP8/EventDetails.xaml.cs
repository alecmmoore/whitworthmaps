using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WhitworthMapWP8
{
    public partial class EventDetails : PhoneApplicationPage
    {
        public EventDetails()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string EmptyString = String.Empty;
            if (NavigationContext.QueryString.TryGetValue("Date", out EmptyString))
            {
                Date.Text = NavigationContext.QueryString["Date"];
            }
            if (NavigationContext.QueryString.TryGetValue("Title", out EmptyString))
            {
                Title.Text = NavigationContext.QueryString["Title"];
            }
            if (NavigationContext.QueryString.TryGetValue("Time", out EmptyString))
            {
                Time.Text = NavigationContext.QueryString["Time"];
            }
            if (NavigationContext.QueryString.TryGetValue("LocationsString", out EmptyString))
            {
                Location.Text = NavigationContext.QueryString["LocationsString"];
            }
            if (NavigationContext.QueryString.TryGetValue("Description", out EmptyString))
            {
                Description.Text = NavigationContext.QueryString["Description"];
            }
            if (NavigationContext.QueryString.TryGetValue("Contact", out EmptyString))
            {
                Contact.Text = NavigationContext.QueryString["Contact"];
            }
            if (NavigationContext.QueryString.TryGetValue("ContactEmail", out EmptyString))
            {
                ContactEmail.Text = NavigationContext.QueryString["ContactEmail"];
            }
            if (NavigationContext.QueryString.TryGetValue("ContactPhone", out EmptyString))
            {
                ContactPhone.Text = NavigationContext.QueryString["ContactPhone"];
            }
            if (NavigationContext.QueryString.TryGetValue("Link", out EmptyString))
            {
                Link.Text = NavigationContext.QueryString["Link"];
            }
        }
    }
}