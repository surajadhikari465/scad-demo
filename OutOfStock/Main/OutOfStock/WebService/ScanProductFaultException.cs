using System.Runtime.Serialization;

namespace OutOfStock.WebService
{
    [DataContract]
    public class ScanProductFaultException
    {
        [DataMember(Name = "Message")]
        private string message;

        public ScanProductFaultException(string message)
        {
            this.message = message;
        }

        public string Message { get { return message; } }
    }
}
