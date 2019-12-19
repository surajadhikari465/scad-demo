using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SampleWcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMyService" in both code and config file together.
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        [FaultContract(typeof(MathFault))]
        [FaultContract(typeof(TryAgainFault))]
        int Divide(int a, int b);
    }

    public class MyService : IMyService
    {
        public int Divide(int a, int b)
        {
            if (b == 0)
            {
                throw new FaultException<MathFault>(new MathFault("Division", "Division by zero!"));
            }
            else
            {
                if ((DateTime.Now.Second) % 10 != 0)
                {
                    throw new FaultException<TryAgainFault>(new TryAgainFault("only works 10% of the time"));
                }
                return a / b;
            }
        }
    }


    // Define a math fault data contract
    [DataContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public class MathFault 
    {

        public MathFault(string operation, string problemType)
        {
            _operation = operation;
            _problemType = problemType;
        }

        private string _operation;
        private string _problemType;

        [DataMember]
        public string Operation
        {
            get { return _operation; }
            set { _operation = value; }
        }

        [DataMember]
        public string ProblemType
        {
            get { return _problemType; }
            set { _problemType = value; }
        }

    }

    // Define a math fault data contract
    [DataContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public class TryAgainFault
    {
        public TryAgainFault(string description)
        {
            _Description = description;
        }
        private string _Description;

        [DataMember]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
    }
}
