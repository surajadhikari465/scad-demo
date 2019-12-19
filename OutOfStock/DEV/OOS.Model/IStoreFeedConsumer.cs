using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace OOS.Model
{
    [ServiceContract]
    public interface IStoreFeedConsumer
    {
        [OperationContract]
        [WebGet(UriTemplate = "/stores/", RequestFormat = WebMessageFormat.Json)]
        dynamic Consume();
    }
}
