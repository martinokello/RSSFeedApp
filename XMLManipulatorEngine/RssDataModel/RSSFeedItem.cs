using System;
using System.Runtime.Serialization;
using System.ServiceModel;
namespace XMLManipulatorEngine.RssDataModel
{
    [DataContract]
    public class RSSFeedItem
    {
        private string _friendlyId;
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Link { get; set; }


        [DataMember]
        public DateTime PublishDate { get; set; }


        [DataMember]
        public string ChannelTitle { get; set; }

        [DataMember]
        public string FriendlyID
        {
            get { return _friendlyId; }
            set { _friendlyId = value.Replace(" ", String.Empty).Replace("'", String.Empty).Replace(".",string.Empty).Replace("#",string.Empty).Replace(":",string.Empty).Replace("?",string.Empty).Replace(";",string.Empty); }
        }
    }
}
