using CSEONS.AuthApplication.Domain;
using CSEONS.AuthApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace CSEONS.AuthApplication.Service
{
    public class GroupManager
    {
        private readonly ApplicationDbContext _context;
        public GroupManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToGroupAsync(ApplicationUser user, string groupName)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Name == groupName);
            user.GroupId = group.Id;
            user.Group = group;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(string name)
        {
            return await _context.Groups.AnyAsync(g => g.Name == name);
        }

        public async Task CreateGroupAsync(string groupName)
        {
            var group = new Group()
            {
                Name = groupName,
            };

            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        internal async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
