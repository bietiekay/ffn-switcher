using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rss;

namespace TestVersionUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Write Update RSS Feed


            #region 101
            RssItem item = new RssItem();
            item.Title = "105";
            item.Comments = "1.0.0.105";
            item.Description = "Freies Funknetz Switcher Version 1.0.0.105\n\n" +
                               "* \"Bake wiederholen\" Text  in \"Bake aktiviert\" geändert\n" +
                               "* Fehler gefixt der dazu geführt hat dass eine Bake ununterbrochen wiederholt wurde\n"+
                               "* Voice Schaltung nocheinmal überarbeitet jetzt erfolgt nurnoch eine Deaktiviert/Aktiviert\n"+
                               "  Meldung und sonst keine Änderung im Verhalten des Switchers\n";

            item.PubDate = DateTime.Now.ToUniversalTime();
            item.Link = new System.Uri("http://dropbox.schrankmonster.de/dropped/FFN-Switcher-105.zip");
            #endregion

            RssChannel channel = new RssChannel();
            channel.Items.Add(item);

            channel.Title = "Freies Funknetz Switcher Tool Update Channel";
            channel.Description = "Updates for FFN Switcher";
            channel.Link = new System.Uri("http://dropbox.schrankmonster.de/FFNSwitcher.xml");
            channel.Generator = "FFN Switcher Update Tool";
            channel.LastBuildDate = DateTime.Now;

            RssFeed feed = new RssFeed();
            feed.Channels.Add(channel);
            feed.Write("FFNSwitcher.xml");
            #endregion
        }
    }
}
