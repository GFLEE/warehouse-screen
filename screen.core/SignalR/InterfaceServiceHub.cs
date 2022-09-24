using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen.Core
{

    /// <summary>
    /// SignalR使用方法： 依赖注入： IHubContext<InterfaceServiceHub> _interfaceHub;
    /// </summary>
    public class InterfaceServiceHub : Hub<IInterfaceServiceClient>
    {
        private readonly ILogger<InterfaceServiceHub> _logger;
        public InterfaceServiceHub(ILogger<InterfaceServiceHub> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 客户端连接服务端
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"SignalR客户端已连接。User：{this.Context.User},UserIdentifier:{this.Context.UserIdentifier},ConnectionId:{this.Context.ConnectionId}");
            return base.OnConnectedAsync();
        }
        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"SignalR客户端连接已断开。ConnectionId:{this.Context.ConnectionId}");

            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// 给所有客户端发送消息
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }
        /// <summary>
        /// 向调用客户端发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageCaller(string message)
        {
            await Clients.Caller.ReceiveCaller(message);
        }


    }
}
