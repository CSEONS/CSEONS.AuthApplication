using CSEONS.AuthApplication.Service;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Permissions;

namespace CSEONS.AuthApplication.Models
{
    public class RegisterViewModel
    {

        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string? SecondName { get; set; }

        [Required]
        [UIHint("password")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [EnumDataType(typeof(ApplicationUser.Roles))]
        [Display(Name = "Группа")]
        public string Role { get; set; }

        [Required]
        [Display(Name = "Группа")]
        public string Group { get; set; }
    }
}
