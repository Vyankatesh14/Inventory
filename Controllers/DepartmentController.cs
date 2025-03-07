using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Service.Repository;


namespace Unioteq.TrackNTrace.Controllers
{

    public class DepartmentController : Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public DepartmentController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            DepartmentRepository DepRepo = new DepartmentRepository(_dbContext);
            var productlist = await DepRepo.Get();
            return View(productlist);
        }

        public async Task<IActionResult> Create()
        {



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentEntity departmentEntity)
        {
            if (ModelState.IsValid)
            {
                DepartmentRepository depoRepo = new DepartmentRepository(_dbContext);

                if (await depoRepo.AddDepartment(departmentEntity))
                {
                    TempData["ActionMessage"] = "Unable to save Department";
                    TempData["ActionStatus"] = "Fail";
                }
                else
                {
                    TempData["ActionMessage"] = "Department saved successfully !";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");

                }
            }
            return View();
        }

        public async Task<IActionResult> Edit(long id)
        {
            //long currentId = HttpContext.Session.Get<long>("UserId");
            DepartmentRepository roleRepo = new DepartmentRepository(_dbContext);
            var products = await roleRepo.GetProductById(id);
            //List<product> products = GetProducts();
            var product = products.FirstOrDefault(p => p.DepartmentId == id);



            return View(product);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentEntity departmentEntity)
        {
            // Retrieve current user id from session

            DepartmentRepository DepoRepo = new DepartmentRepository(_dbContext);
            // Check if the current user id is valid  


            // Call AddProductAsync method and await its completion
            if (await DepoRepo.UpdateDepartment(departmentEntity))
            {
                // If AddProductAsync returns true, the user was added successfully
                TempData["InsertMessage"] = "Department updated successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                // Handle the case when AddProductAsync returns false
                // For example, display an error message
                TempData["ErrorMessage"] = "Failed to update Department.";
                return View(); // Or redirect to another action
            }

        }

        public async Task<IActionResult> Delete(long Id, DepartmentEntity departmentEntity)
        {


            DepartmentRepository DepartmentRepo = new DepartmentRepository(_dbContext);
            var products = await DepartmentRepo.GetProductById(Id);
            long r = await DepartmentRepo.DeleteDepartment(Id);
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

