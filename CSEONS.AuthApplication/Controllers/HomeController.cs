using CSEONS.AuthApplication.Domain;
using CSEONS.AuthApplication.Domain.Entities;
using CSEONS.AuthApplication.Extensions;
using CSEONS.AuthApplication.Models;
using CSEONS.AuthApplication.Service;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace CSEONS.AuthApplication.Controllers
{
    public class HomeController : Controller
    {
        private const int COUNT_USERS_IN_PAGE = 100;

        DataManager _datamanager;
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public HomeController(DataManager dataManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _datamanager = dataManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> GroupChat()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var chatGroups = new List<Group>();

            chatGroups.Add(_datamanager.DefaultContext.Groups.FirstOrDefault(g => g.Id == user.GroupId));
            chatGroups.Add(_datamanager.DefaultContext.Groups.FirstOrDefault(g => g.Name == "GeneralGroup"));

            ViewBag.groupMates = _datamanager.DefaultContext.Users.Where(u => u.Group.Id == user.GroupId && u.Login != user.Login);

            ViewBag.groupChats = chatGroups;

            return View();
        }

        [Authorize(Roles = nameof(ApplicationUser.Roles.Teacher))]
        public async Task<IActionResult> EditPoints()
        {
            var teacher = await _userManager.GetUserAsync(HttpContext.User);

            if (teacher is null)
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());

            var students = await _userManager.GetUsersInRoleAsync(nameof(ApplicationUser.Roles.Student));

            var studentsInTeacherGroup = students.Where(u => u.GroupId == teacher.GroupId).ToList();

            return View(studentsInTeacherGroup);
        }

        [HttpPost]
        [Authorize(Roles = nameof(ApplicationUser.Roles.Teacher))]
        public async Task<IActionResult> EditPoints(List<UserPointChanges> userChanges)
        {
            if (ModelState.IsValid)
            {
                var userChangesId = userChanges.Select(u => u.Id).ToList();

                var editedUsers = _userManager.Users.Where(u => userChangesId.Contains(u.Id)).ToList();

                foreach (var change in userChanges)
                {
                    editedUsers.ForEach((u) =>
                    {
                        if (u.Id == change.Id)
                        {
                            switch (change.Operation)
                            {
                                case UserPointChanges.PointOperations.Add:
                                    u.Points += change.Value ?? 0;
                                    break;

                                case UserPointChanges.PointOperations.Remove:
                                    u.Points -= change.Value ?? 0;
                                    break;

                                case UserPointChanges.PointOperations.Assign:
                                    u.Points = change.Value ?? u.Points;
                                    break;
                                default:
                                    break;
                            }
                        }
                    });
                }

                foreach (var user in editedUsers)
                {
                    await _userManager.UpdateAsync(user);
                }
            }

            var teacher = await _userManager.GetUserAsync(HttpContext.User);

            if (teacher is null)
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());

            var students = await _userManager.GetUsersInRoleAsync(nameof(ApplicationUser.Roles.Student));

            var studentsInTeacherGroup = students.Where(u => u.GroupId == teacher.GroupId).ToList();


            return View(studentsInTeacherGroup);
        }

        public async Task<IActionResult> PointsBoard(int page = 0)
        {
            var users = await _userManager.GetUsersInRoleAsync(nameof(ApplicationUser.Roles.Student));

            var topUsersByPoints = users
                .OrderBy(u => u.Points)
                .Skip(page * COUNT_USERS_IN_PAGE)
                .Take(COUNT_USERS_IN_PAGE);

            return View(topUsersByPoints);
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfilePhoto(IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                var callerUser = await _userManager.GetUserAsync(HttpContext.User);

                if (callerUser is null)
                {
                    ModelState.AddModelError(nameof(callerUser), "Пользователь не найден");
                    return RedirectToAction(nameof(Profile), nameof(HomeController).CutController());
                }

                await _datamanager.ImageHandler.UploadImage(uploadedFile, callerUser.Login);
                callerUser.ImagePhotoURL = _datamanager.ImageHandler.GetImageUrl(callerUser.Login);

                await _userManager.UpdateAsync(callerUser);
            }

            var a = ModelState;

            return RedirectToAction(nameof(Profile), nameof(HomeController).CutController());
        }
    }
}