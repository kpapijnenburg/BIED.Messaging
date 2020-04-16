using System;
using System.Collections.Generic;
using System.Text;

namespace BIED.Messaging.Config
{
    public class RabbitMqConfig
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int ConnectionRetries { get; set; }
    }
}
