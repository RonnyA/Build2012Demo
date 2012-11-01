using Microsoft.AspNet.SignalR.Hubs;

namespace ChatServer
{
    public class Chat : Hub
    {
        public void Send(string send)
        {
            Clients.All.send(send);
        }
    }
}