using Microsoft.AspNetCore.SignalR;

namespace ChatRoom{
    public class ChatHub : Hub{

        public async Task SendMessage(string room, string user, string message){
            await Clients.Group(room).SendAsync("ReceiveMessage",user,message,DateTime.Now.ToString());
        }

        public async Task AddToGroup(string room){
            await Groups.AddToGroupAsync(Context.ConnectionId,room);
        }




    }
}