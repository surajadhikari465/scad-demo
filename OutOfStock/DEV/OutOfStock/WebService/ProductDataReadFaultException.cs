using System.Runtime.Serialization;

namespace OutOfStock.WebService
{
    [DataContract]
    public class ProductDataReadFaultException
    {
        [DataMember(Name = "Message")]
        private string message;

        public ProductDataReadFaultException(string message)
        {
            this.message = message;
        }

        public string Message { get { return message; } }
    }
}
