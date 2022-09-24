using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen.Core
{
    public interface IInterfaceServiceClient
    {
        Task ReceiveMessage(string user, string message);
        Task ReceiveMessage(object message);
        Task ReceiveCaller(object message);
    }
}
