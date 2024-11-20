using ContractMonthlyClaimSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContractMonthlyClaimSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Index: List all users
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // Create (GET): Show the form to create a new user
        public IActionResult Create()
        {
            return View(new User()); // Pass an empty User object to the view
        }


        // Create (POST): Handle user creation

        [HttpPost]
        public async Task<IActionResult> Create(User model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(model, model.FullName);
                if (result.Succeeded)
                {
                    // Assign the role after user is created
                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        await _userManager.AddToRoleAsync(model, model.Role);
                    }

                    return RedirectToAction(nameof(Index)); // Redirect to the index view
                }

                // Collect errors if creation fails
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // Edit (GET): Show the form to edit a user's details
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // Edit (POST): Handle updating user details
        [HttpPost]
        public async Task<IActionResult> Edit(User model, string password)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.FullName = model.FullName;
                    user.Email = model.Email;
                    user.Role = model.Role;

                    // Handle password update if provided
                    if (!string.IsNullOrEmpty(password))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var result = await _userManager.ResetPasswordAsync(user, token, password);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }

                    var updateResult = await _userManager.UpdateAsync(user);
                    if (updateResult.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        // Delete (GET): Show the delete confirmation page
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // Delete (POST): Handle deleting the user
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                // Handle errors when deleting the user
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return NotFound();
        }
    }
}
