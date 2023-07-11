using CSEONS.AuthApplication.Domain.Repositories.Abstract;
using CSEONS.AuthApplication.Service;
using Microsoft.EntityFrameworkCore;

namespace CSEONS.AuthApplication.Domain
{
    public class DataManager
    {
        public ApplicationDbContext DefaultContext { get; set; }
        public IMessagesRepository Messages { get; set; }
        public IImageHandlerRepository ImageHandler { get; set; }

        public DataManager(ApplicationDbContext context, IMessagesRepository messagesRepository, IImageHandlerRepository images)
        {
            Messages = messagesRepository;
            DefaultContext = context;
            ImageHandler = images;
        }
    }
}
