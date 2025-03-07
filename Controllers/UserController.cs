using Microsoft.AspNetCore.Mvc;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Unioteq.TrackNTrace.Service.Repository;
using System.Numerics;
using System.Data;

namespace Unioteq.TrackNTrace.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ShopFloorRepository _shopFloorRepository;


        public UserController(ApplicationDBContext dbContext, ShopFloorRepository shopFloorRepository)
        {
            _dbContext = dbContext;
            _shopFloorRepository = shopFloorRepository;
        }


        [HttpGet]
        public IActionResult Register()
        {
            // Fetch the dropdown data from the respective tables
            var departments = _dbContext.Departments.ToList();
            var plants = _dbContext.plants.ToList();
            var roles = _dbContext.Role.ToList();
         
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
            ViewBag.Plants = new SelectList(plants, "PlantId", "PlantName");
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
           
            return View();
        }

        [HttpGet]
        public JsonResult GetShopFloorByPlantId(long plantId)
        {
            var shopFloors = _shopFloorRepository.GetActiveListByPlantId(plantId);
            return Json(shopFloors);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserEntity userModel)
        {
            if (ModelState.IsValid == true)
            {
                // Save the new user to the database
                _dbContext.Users.Add(userModel);
                _dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login");
            }

            // If ModelState is not valid, repopulate the ViewBag with the dropdown data
            var departments = _dbContext.Departments.ToList();
            var plants = _dbContext.plants.ToList();
            var roles = _dbContext.Role.ToList();
           
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
            ViewBag.Plants = new SelectList(plants, "PlantId", "PlantName");
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");

            // Return the view with the model and the dropdowns repopulated
            TempData["ErrorMessage"] = "Please fix the validation errors.";
            return View(userModel);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserEntity Users)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == Users.Email && u.Password == Users.Password);

            if (user != null)
            {
                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Index", "Home");
            }
                
            TempData["ErrorMessage"] = "Invalid login credentials.";
            return View();
        }

        public async Task<IActionResult> Index()
        {
            UserRepository UserRepo = new UserRepository(_dbContext);
            var product = await UserRepo.GetUser();
            return View(product);
        }

        // GET: Create
        // GET: Create
        public async Task<IActionResult> Create()
        {
            var departments = _dbContext.Departments.ToList();
            var plants = _dbContext.plants.ToList();
            var roles = _dbContext.Role.ToList();

            // Use SelectList to bind the dropdown options properly
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
            ViewBag.Plants = new SelectList(plants, "PlantId", "PlantName");
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");

            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserEntity user)
        {
            if (ModelState.IsValid)
            {
                
                // Save the user to the database asynchronously
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            // Fetch data again if validation fails
            var departments = _dbContext.Departments.ToList();
            var plants = _dbContext.plants.ToList();
            var roles = _dbContext.Role.ToList();

            // Use SelectList to bind the dropdown options properly
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
            ViewBag.Plants = new SelectList(plants, "PlantId", "PlantName");
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
            return View(user);
        }

        public async Task<IActionResult> Edit(long id)
        {
 
            UserRepository userRepo = new UserRepository(_dbContext);

            var user = await userRepo.GetUserById(id);

            PlantRepository plantRepo = new PlantRepository(_dbContext);
            var plantList = await plantRepo.Get();
            ViewBag.PlantList = new SelectList(plantList, "PlantId", "PlantName", user.FirstOrDefault()?.PlantId);

           
            DepartmentRepository departmentRepo = new DepartmentRepository(_dbContext);
            var departmentList = await departmentRepo.Get();

            
            ViewBag.DepartmentList = new SelectList(departmentList, "DepartmentId", "DepartmentName", user.FirstOrDefault()?.DepartmentId);

         
            RoleRepository roleRepo = new RoleRepository(_dbContext);
            var roleList = await roleRepo.Get();

          
            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName", user.FirstOrDefault()?.RoleId);

           
            return View(user.FirstOrDefault());
        }


        // POST: Edit action to update user details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEntity user)
        {
            try
            {
       
                UserRepository userRepo = new UserRepository(_dbContext);

                if (await userRepo.UpdateUser(user))
                {
                    TempData["ActionMessage"] = "User Updated Successfully";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ActionMessage"] = "User Unable to Update";
                    TempData["ActionStatus"] = "Fail";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the user: " + ex.Message;
            }

           
            return View(user);
        }

        public async Task<IActionResult> Delete(long Id, UserEntity User)
        {


            UserRepository UserRepo = new UserRepository(_dbContext);
            var products = await UserRepo.GetUserById(Id);
            long r = await UserRepo.DeleteUser(Id);
            if (r > 0)
            {
                TempData["DeleteErrorMessage"] = "<script>alert('data not deleted.')</script>";
            }
            else
            {

                TempData["DeleteMessage"] = "<script>alert('data deleted successfully...')</script>";
                return RedirectToAction("Index");
            }
            return View();

            // return View(product);
        }


    }
}
