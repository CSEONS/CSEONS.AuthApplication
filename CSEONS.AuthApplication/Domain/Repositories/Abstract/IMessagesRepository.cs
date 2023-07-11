using CSEONS.AuthApplication.Domain.Entities;

namespace CSEONS.AuthApplication.Domain.Repositories.Abstract
{
    public interface IMessagesRepository
    {
        IQueryable<Message> Get();
        Message GetById(Guid id);
        void Add(Message item);
        void Delete(Guid id);
    }
}