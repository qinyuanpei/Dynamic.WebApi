
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Server.Service
{
    [ServiceContract]
    public interface IMessage
    {
        [OperationContract]
        string Echo(string receiver);
    }

    public class MessageService : IMessage
    {
        public string Echo(string receiver)
        {
            return $"Hello, {receiver}";
        }
    }
}
