using JuanMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace JuanMVC
{
    public class JuanHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;

        public JuanHub(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated )
            {
                var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
                user.IsOnline = true;
                user.ConnectionId = Context.ConnectionId;

                await _userManager.UpdateAsync(user);

                await Clients.All.SendAsync("SetOnline", user.Id);

            }

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User.Identity.IsAuthenticated )
            {
                var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
                user.IsOnline = false;
                user.ConnectionId = null;
                user.LastOnlineAt = DateTime.UtcNow.AddHours(4);


                await _userManager.UpdateAsync(user);

                await Clients.All.SendAsync("SetOffline", user.Id);

            }
        }
    }
}
