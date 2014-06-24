using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text.RegularExpressions;

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
            set
            {
                var pattern = @"('| |:|;|#|£|\$|\.|\?|"")";
                var regEx = new Regex(pattern);
                var result = regEx.Replace(value, "");
                _friendlyId = result;
            }
        }
    }
}
