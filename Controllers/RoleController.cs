using Microsoft.AspNetCore.Mvc;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Service.Repository;

namespace Unioteq.TrackNTrace.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public RoleController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            RoleRepository rolerepo = new RoleRepository(_dbContext);
            var roleList = await rolerepo.Get();
            return View(roleList);
        }



        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleEntity roleEntity)
        {

            if (ModelState.IsValid)
            {
                RoleRepository rolerepo = new RoleRepository(_dbContext);

                if (await rolerepo.AddRole(roleEntity))
                {
                    TempData["ActionMessage"] = "Unable to save Role";
                    TempData["ActionStatus"] = "Fail";
                }
                else
                {
                    TempData["ActionMessage"] = "Role saved successfully !";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");

                }
            }
            return View();
        }


        public async Task<IActionResult> Edit(long id)
        {
            RoleRepository roleRepo = new RoleRepository(_dbContext);
            var products = await roleRepo.GetRoleById(id);
            //List<product> products = GetProducts();
            var product = products.FirstOrDefault(p => p.RoleId == id);



            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(RoleEntity roleEntity)
        {
            RoleRepository roleRepo = new RoleRepository(_dbContext);

            if(await roleRepo.UpdateRole(roleEntity))
            {
                TempData["InsertMessage"] = "User inserted successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to insert user.";
                return View();
            }
        }


        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                RoleRepository stationRepo = new RoleRepository(_dbContext);
                long result = await stationRepo.DeleteRole(id);
                if (result > 0)
                {
                    TempData["ActionMessage"] = "Role Deleted Successfully";
                    TempData["ActionStatus"] = "Success";
                }
                else
                {
                    TempData["ActionMessage"] = "Role Unable to Delete";
                    TempData["ActionStatus"] = "Fail";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the Role: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
