using System;
using Foundation;

namespace RssReader.Models
{
    public class NewsItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }

        public NewsItem(){ }

        public NewsItem(string title, string description, string date, string link)
        {
            this.Title = title;
            this.Description = description;
            this.Date = date;
            this.Link = link;
        }
    }
}

