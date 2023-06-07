using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace DoctorWebForum
{
    public class OnlineUsersHub : Hub
    {
        private static readonly ConcurrentDictionary<string, bool> _onlineUsers = new ConcurrentDictionary<string, bool>();

        public override async Task OnConnectedAsync()
        {
            var userGuid = Context.GetHttpContext().Request.Cookies["UserGuid"];

            _onlineUsers.TryAdd(userGuid, true);

            await UpdateOnlineUsers();

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userGuid = Context.GetHttpContext().Request.Cookies["UserGuid"];

            //try to remove key from dictionary
            if (!_onlineUsers.TryRemove(userGuid, out _))
                //if not possible to remove key from dictionary, then try to mark key as not existing in cache
                _onlineUsers.TryUpdate(userGuid, false, true);

            await UpdateOnlineUsers();

            await base.OnDisconnectedAsync(exception);
        }

        private Task UpdateOnlineUsers()
        {
            var count = GetOnlineUsersCount();
            return Clients.All.SendAsync("UpdateOnlineUsers", count);
        }

        public static int GetOnlineUsersCount()
        {
            return _onlineUsers.Count(p => p.Value);
        }
    }
}
