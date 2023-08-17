using Data.Entities;
using Logic.Models.EmailConfigurationModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logic.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly UserManager<User> _userManager;

        public MessageService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Message> GenerateMessage(User user, string link)
        {       
            var message = new Message(new List<string>() { user.Email! }, "EMaghazin Email confirmation!", link!);

            return message;
        }
    }
}
