using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rss;
using System.Net;

namespace CheckForUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            String UpdateURL = "http://dropbox.schrankmonster.de/dropped/FFNSwitcher.xml";
            RssFeed feed = RssFeed.Read(UpdateURL);

            Int32 VersionNumber = 90;

            if (Convert.ToInt32(feed.Channels[0].Items[0].Title) <= VersionNumber)
            {
                // no update available
                Console.WriteLine("No Update Available");
            }
            else
            {
                // if newer versio available
                Console.WriteLine("Update Available");

                Console.WriteLine(feed.Channels[0].Items[0].Description);

                // download ...
                WebClient Client = new WebClient();
                Client.DownloadFile(feed.Channels[0].Items[0].Link, "Update.7z");

                
            }

            Console.ReadLine();
        }
    }
}
