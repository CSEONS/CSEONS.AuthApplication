using CSEONS.AuthApplication.Domain;
using CSEONS.AuthApplication.Domain.Entities;
using CSEONS.AuthApplication.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace CSEONS.AuthApplication.Hubs
{
    public class NotificationHub : Hub
    {
        //static List<Message> messages = new List<Message>();
        UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _context;

        public NotificationHub(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _context = applicationDbContext;
        }

        public async Task Send(string messageObject)
        {
			try
			{
                Message deserealizedMessage = JsonConvert.DeserializeObject<Message>(messageObject);

                Message message = new Message()
                {
                    Text = deserealizedMessage.Text,
                    SenderName = Context?.User?.Identity?.Name,
                    GroupName = deserealizedMessage.GroupName,
                };

                string serializedMessage = JsonConvert.SerializeObject(message);

                _context.Messages.Add(message);
                _context.SaveChanges();

                await Clients.Group(message.GroupName).SendAsync("Send", serializedMessage);
            }
			catch (Exception e)
			{
                Console.WriteLine(e.Message);
                throw;
			}
        }

        public async Task JoinGroup(string name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, name);

            var groupMessages = _context.Messages.Where(m => m.GroupName == name);

            foreach (var message in groupMessages)
            {
                var serializedMessage = JsonConvert.SerializeObject(message);

                await Clients.Caller.SendAsync("Send", serializedMessage);
            }
        }

        public async Task JoinToUser(string id)
        {
            // TODO:  Переписать с вызовом пользователя через контекст как в GroupChat - Action
            string firstUserName = Context?.User?.Identity?.Name;
            string chatHash = string.Empty;
            string callerUserId;

            if (string.IsNullOrEmpty(firstUserName) is false)
            {
                var user = _userManager.FindByNameAsync(firstUserName);
                callerUserId = user.Id.ToString();

                if (string.Compare(id, callerUserId) > 0)
                {
                    chatHash = HashCode.Combine(firstUserName, callerUserId).ToString();
                }
                else
                {
                    chatHash = HashCode.Combine(callerUserId, firstUserName).ToString();
                }

                await JoinGroup(id);
            }
        }

        public async Task LeaveGroup(string name)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, name);
        }
    }
}
