using System.Runtime.Serialization;

namespace OutOfStock.WebService
{
    [DataContract]
    public class MovementDataReadFaultException
    {
        [DataMember(Name = "Message")]
        private string error;

        public MovementDataReadFaultException(string error)
        {
            this.error = error;
        }

        public string Message { get { return error; } }
    }
}
