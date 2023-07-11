using CSEONS.AuthApplication.Domain.Entities;
using CSEONS.AuthApplication.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CSEONS.AuthApplication.Domain.Repositories.EntityFramework
{
    public class EFMessagesRepository : IMessagesRepository
    {
        ApplicationDbContext _context;

        public EFMessagesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Message item)
        {
            if (item.Id == default)
                _context.Entry(item).State = EntityState.Added;
            else
                _context.Entry(item).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            _context.Messages.Remove(new Message() { Id = id.ToString()});
            _context.SaveChanges();
        }

        public IQueryable<Message> Get()
        {
            return _context.Messages;
        }

        public Message GetById(Guid id)
        {
            return _context.Messages.FirstOrDefault(m => m.Id == id.ToString());
        }
    }
}
