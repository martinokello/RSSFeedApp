using XMLManipulatorEngine.RssDataModel;

namespace XMLManipulatorEngine.Interfaces
{
    public interface IXmlServices
    {
        RSSFeedItem[] GetRssFeedsObjects(string rssFeedUrl);
        void AddVotes(RSSFeedItem[] feeds, string urlDataStoreXml);
        string ReadXmlDocument(string path);
    }
}
