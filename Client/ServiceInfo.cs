using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ServiceInfo<TService>
    {
        private readonly ChannelFactory _channelFactory;
        public ServiceInfo(ChannelFactory channelFactory)
        {
            _channelFactory = channelFactory;
        }
        public TService Service { get; set; }

        public void Close()
        {
            if (_channelFactory != null)
                _channelFactory.Close();
        }
    }
}
