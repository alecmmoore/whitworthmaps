using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhitworthMapWP8
{
    public class Event
    {
        public string id { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }
        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }
        [JsonProperty(PropertyName = "descrition")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "locations")]
        public string Locations { get; set; }
        [JsonProperty(PropertyName = "locationstring")]
        public string LocationsString { get; set; }
        [JsonProperty(PropertyName = "contact")]
        public string Contact { get; set; }
        [JsonProperty(PropertyName = "contactphone")]
        public string ContactPhone { get; set; }
        [JsonProperty(PropertyName = "contactemail")]
        public string ContactEmail { get; set; }
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        public Event(string title, string date, string time, string description,
            string locations, string contact, string contactPhone,
            string contactEmail, string link)
        {
            this.Title = title;
            this.Date = date;
            this.Time = time;
            this.Description = description;
            this.Locations = "";
            this.LocationsString = locations;
            this.Contact = contact;
            this.ContactPhone = contactPhone;
            this.ContactEmail = contactEmail;
            this.Link = link;
        }

        private void setBuildings()
        {
            List<string> buildings = new List<string> {
                "McEachran",
                "MacKay",
                "Cowles",
                "Music",
                "Auld",
                "Dixon",
                "Warren",
                "Seeley",
                "Ballard",
                "McMillan",
                "Graves",
                "Fieldhouse",
                "Aquatics",
                "Conditioning",
                "Recreation",
                "Westside",
                "Westminster",
                "Weyerhaeuser",
                "Lied",
                "Schumacher",
                "Robinson",
                "Lindaman",
                "Library",
                "Johnston",
                "Baldwin-Jenkins",
                "Hendrick",
                "Stewart",
                "Village",
                "Arend",
                "Boppell",
                "Cornerstone",
                "HUB",
                "Hardwick",
                "Hawthorne",
                "Facilities",
                "Cove",
                "Duvall",
                "Tennis",
                "Pine",
                "Omache",
                "Merkel",
                "Soccer",
                "Marks",
                "Cove",
                "East",
                "Loop",
                "Hill",
                "Quall",
                "President's"
            };
        }
    }
}
