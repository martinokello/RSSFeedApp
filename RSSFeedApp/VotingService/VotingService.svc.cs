using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Remoting.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Web.UI.WebControls;
using XMLManipulatorEngine;
using XMLManipulatorEngine.RssDataModel;

namespace RSSFeedApp.VotingService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "VotingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select VotingService.svc or VotingService.svc.cs at the Solution Explorer and start debugging.
    public class VotingService : IVotingService
    {
        [WebInvoke(Method = "POST",UriTemplate = "/AddVotes", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        public void AddVotes(RSSFeedItem[] feeds)
        {
            var xmlManipulator = new XMLManipulator();
            var dataStorePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["XMLDataStoreUrl"]);

            xmlManipulator.AddVotes(feeds,dataStorePath);
        }
        [WebInvoke(Method = "GET",UriTemplate = "/GetCurrentXml",ResponseFormat = WebMessageFormat.Xml)]
        public string GetCurrentXml()
        {
            var xmlManipulator = new XMLManipulator();
            var dataStorePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["XMLDataStoreUrl"]);
            return xmlManipulator.ReadXmlDocument(dataStorePath);
        }
    }
}
