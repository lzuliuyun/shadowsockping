using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.com
{
    class ShadowSockIP
    {
        private long ping_time;
        private string remarks;
        private string server;
        private long server_port;
        private string password;
        private string method;

        public long Ping_time
        {
            get
            {
                return ping_time;
            }

            set
            {
                ping_time = value;
            }
        }

        public string Remarks
        {
            get
            {
                return remarks;
            }

            set
            {
                remarks = value;
            }
        }

        public string Server
        {
            get
            {
                return server;
            }

            set
            {
                server = value;
            }
        }

        public long Server_port
        {
            get
            {
                return server_port;
            }

            set
            {
                server_port = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string Method
        {
            get
            {
                return method;
            }

            set
            {
                method = value;
            }
        }

       
    }
}
