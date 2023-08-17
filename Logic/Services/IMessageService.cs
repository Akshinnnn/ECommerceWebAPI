using Data.Entities;
using Logic.Models.EmailConfigurationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IMessageService
    {
        public Task<Message> GenerateMessage(User user, string link);
    }
}
