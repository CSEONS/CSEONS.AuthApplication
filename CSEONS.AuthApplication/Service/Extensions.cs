using CSEONS.AuthApplication.Service;
using Microsoft.AspNetCore.Identity;

namespace CSEONS.AuthApplication.Extensions
{
    public static class Extensions
    {
        public static string CutController(this string str)
        {
            return str.Replace("Controller", "");
        }

        public static string GetUserName(this ApplicationUser user)
        {
            return $"{user.FirstName}{user.LastName}{user.SecondName}";
        }

        public static async Task CreateRoleAsync(this RoleManager<IdentityRole> roleManger, string name)
        {
            IdentityRole newRole = new IdentityRole()
            {
                Name = name,
                NormalizedName = name.ToUpper(),
            };

            await roleManger.CreateAsync(newRole);
        }
    }
}
