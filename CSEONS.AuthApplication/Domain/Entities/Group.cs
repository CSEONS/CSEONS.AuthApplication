using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CSEONS.AuthApplication.Service;

namespace CSEONS.AuthApplication.Domain.Entities
{
    public class Group
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? TeacherId { get; set; }
        public ICollection<ApplicationUser> Students { get; set; }

        public Group()
        {
            Students = new List<ApplicationUser>();
        }
    }
}
