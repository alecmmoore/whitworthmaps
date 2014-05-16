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

            Date.Text = NavigationContext.QueryString["Date"];
            Title.Text = NavigationContext.QueryString["Title"];
            Time.Text = NavigationContext.QueryString["Time"];
            Location.Text = NavigationContext.QueryString["LocationsString"];
            Description.Text = NavigationContext.QueryString["Description"];
            Contact.Text = NavigationContext.QueryString["Contact"];
            ContactEmail.Text = NavigationContext.QueryString["ContactEmail"];
            ContactPhone.Text = NavigationContext.QueryString["ContactPhone"];
            Link.Text = NavigationContext.QueryString["Link"];
        }
    }
}