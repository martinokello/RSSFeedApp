using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using XMLManipulatorEngine.RssDataModel;

namespace RSSFeedApp.VotingService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IVotingService" in both code and config file together.
    [ServiceContract]
    public interface IVotingService
    {
        [OperationContract]
        void AddVotes(RSSFeedItem[] feeds);

        [OperationContract]
        string GetCurrentXml();
    }
}
