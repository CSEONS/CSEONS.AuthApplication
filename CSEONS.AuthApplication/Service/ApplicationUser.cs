using CSEONS.AuthApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSEONS.AuthApplication.Service
{
    public class ApplicationUser : IdentityUser
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? SecondName { get; set; }
        public int Points { get; set; }
        public string? GroupId { get; set; }
        public string? ImagePhotoURL { get; set; }
        public Group Group { get; set; }

        public enum Roles
        {
            Admin,
            Teacher,
            Student
        }
    }
}
