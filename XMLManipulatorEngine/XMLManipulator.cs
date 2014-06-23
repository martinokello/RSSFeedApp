using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using XMLManipulatorEngine.RssDataModel;

namespace XMLManipulatorEngine
{
    public class XMLManipulator
    {
        private static readonly object lockObject = new object();

        public RSSFeedItem[] GetRssFeedsObjects(string rssFeedUrl)
        {
            try
            {
                var xml = GetXmlFromFeed(rssFeedUrl);
                return GetFeedDataItems(xml);
            }
            catch (Exception e)
            {
                return null;
            }

        }

        private string GetXmlFromFeed(string rssFeedUrl)
        {
            var xmlfeed = string.Empty;
            var webRequest = HttpWebRequest.Create(rssFeedUrl) as HttpWebRequest;

            using (var stream = webRequest.GetResponse().GetResponseStream())
            {
                var length = 4096;

                var buffer = new byte[length];

                var streamReader = new StreamReader(stream);
                xmlfeed = streamReader.ReadToEnd();
                streamReader.Close();
                stream.Close();
                return xmlfeed;
            }
        }

        private RSSFeedItem[] GetFeedDataItems(string xml)
        {
            var dateFormat = new DateTimeFormatInfo();

            dateFormat.LongDatePattern = "ddd, dd MMM yyyy HH:mm:ss";
            var doc = XDocument.Parse(xml);

            var channel = doc.Descendants("channel").SingleOrDefault();
            var channelTitle = channel.Elements("title").SingleOrDefault().Value;

            var items = channel.Elements("item");

            var results = from item in items
                select new RSSFeedItem
                {
                    ChannelTitle = channelTitle,
                    Description = item.Element("description") != null? item.Element("description").Value:string.Empty,
                    Link = item.Element("link") != null ? item.Element("link").Value: string.Empty,
                    Title = item.Element("title") != null? item.Element("title").Value:string.Empty,
                    FriendlyID = item.Element("title") != null ? item.Element("title").Value : string.Empty,
                    PublishDate = item.Element("pubDate") != null ? DateTime.Parse(item.Element("pubDate").Value, dateFormat) : DateTime.Now
                };

            return results.ToArray();
        }

        public void AddVotes(RSSFeedItem[] feeds,string urlDataStoreXml)
        {
            lock (lockObject)
            {
                XDocument doc = null;

                var fileInfo = new FileInfo(urlDataStoreXml);
                if (fileInfo.Exists)
                {
                    doc = XDocument.Load(urlDataStoreXml);
                }
                else
                {
                    doc = new XDocument();
                    doc.Add(new XElement("stories"));
                }

                var stories = doc.Root.Descendants("story");

                foreach (var feed in feeds)
                {
                    if (stories.Any())
                    {
                        var story = stories.SingleOrDefault(p => p.Attribute("id").Value == feed.FriendlyID);

                        if (story != null)
                        {
                            var count = int.Parse(story.Attribute("voteCount").Value);
                            count++;
                            story.SetAttributeValue("voteCount", count);
                        }
                        else
                        {
                            //add a new story element:       
                            AddNewStoryToDocument(doc, feed, 1);
                        }
                    }
                    else
                    {
                        //add new Story to blank document                        
                        AddNewStoryToDocument(doc, feed, 1);
                    }
                }

                SaveDocument(doc, urlDataStoreXml);
            }
        }

        private void AddNewStoryToDocument(XDocument doc, RSSFeedItem feed, int voteCount)
        {
            //add a new story element:
            var xStory = new XElement("story");
            xStory.SetAttributeValue("id", feed.FriendlyID);
            xStory.SetAttributeValue("voteCount", voteCount);
            xStory.SetAttributeValue("datePublished", feed.PublishDate);

            var link = new XElement("link", feed.Link);
            var title = new XElement("title", feed.Title);
            var desc = new XElement("description",feed.Description);
            xStory.Add(link);
            xStory.Add(title);
            xStory.Add(desc);
            doc.Root.Add(xStory);
        }

        private void SaveDocument(XDocument doc, string path)
        {
            var fileInfo = new FileInfo(path);

            var stream = fileInfo.CreateText();
            var writer = new XmlTextWriter(stream);
            doc.WriteTo(writer);

            writer.Flush();
            writer.Close();
            stream.Close();
        }

        public string ReadXmlDocument(string path)
        {
            lock (lockObject)
            {
                var fileInfo = new FileInfo(path);

                var stream = fileInfo.OpenRead();

                var streamReader = new StreamReader(stream);

                var xmlResult = streamReader.ReadToEnd();
                stream.Close();
                return xmlResult;
            }
        }
    }
}
