using Microsoft.AspNetCore.Mvc;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Service.Repository;

namespace Unioteq.TrackNTrace.Controllers
{
    public class OperatorSkillController : Controller
    {
        private readonly ApplicationDBContext _dbContext;


        public OperatorSkillController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<IActionResult> Index()
        {

            OperatorSkillRepository OperSkillRepo = new OperatorSkillRepository(_dbContext);
            var OperatorSkillList = await OperSkillRepo.GetOperatorSkillList();

            return View(OperatorSkillList);


        }

        public async Task<IActionResult> Create()
        {
             return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OperatorSkillEntity OperatorSkillEntity)
        {

            if (!ModelState.IsValid)
            {
                OperatorSkillRepository OperatorSkillRepo = new OperatorSkillRepository(_dbContext);

                if (await OperatorSkillRepo.AddOperatorSkill(OperatorSkillEntity))
                {
                    TempData["ActionMessage"] = "Unable to save OperatorSkill";
                    TempData["ActionStatus"] = "Fail";
                }
                else
                {
                    TempData["ActionMessage"] = "Plant saved OperatorSkill !";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");

                }
            }
            return View();
        }

          public async Task<IActionResult> Edit(long id)
        {
            //long currentId = HttpContext.Session.Get<long>("UserId");
            OperatorSkillRepository roleRepo = new OperatorSkillRepository(_dbContext);
            var products = await roleRepo.GetOperatorSkillById(id);
            //List<product> products = GetProducts();
            var product = products.FirstOrDefault(p => p.OperatorSkillId == id);



            return View(product);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OperatorSkillEntity operatorskillEntity)
        {
            // Retrieve current user id from session

            OperatorSkillRepository plantRepo = new OperatorSkillRepository(_dbContext);
            // Check if the current user id is valid  


            // Call AddProductAsync method and await its completion
            if (await plantRepo.UpdateOperatorSkill(operatorskillEntity))            {
                // If AddProductAsync returns true, the user was added successfully
                TempData["InsertMessage"] = "OperatorSkill inserted successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                // Handle the case when AddProductAsync returns false
                // For example, display an error message
                TempData["ErrorMessage"] = "Failed to insert OperatorSkill.";
                return View(); // Or redirect to another action
            }

        }

        public async Task<IActionResult> Delete(long Id, OperatorSkillEntity OperatorSkillEntity)
        {


            OperatorSkillRepository plantRepo = new OperatorSkillRepository(_dbContext);
            var products = await plantRepo.GetOperatorSkillById(Id);
            long r = await plantRepo.DeleteOperatorSkill(Id);
            if (r > 0)
            {
                TempData["DeleteErrorMessage"] = "<script>alert('OperatorSkill not deleted.')</script>";
            }
            else
            {

                TempData["DeleteMessage"] = "<script>alert('OperatorSkill deleted successfully...')</script>";
                return RedirectToAction("Index");
            }
            return View();

            // return View(product);
        }
    }
}
