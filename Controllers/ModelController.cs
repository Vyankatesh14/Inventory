using Microsoft.AspNetCore.Mvc;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Service.Repository;

namespace Unioteq.TrackNTrace.Controllers
{
    public class ModelController : Controller
    {
        private readonly ApplicationDBContext _dbContext;


        public ModelController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<IActionResult> Index()
        {

            ModelRepository ModelRepo = new ModelRepository(_dbContext);
            var productList = await ModelRepo.Get();

            return View(productList);
        }


        public async Task<IActionResult> Create()
        {
             return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModelEntity modelEntity)
        {
            if (ModelState.IsValid)
            {
                ModelRepository ModelRepo = new ModelRepository(_dbContext);

                if (await ModelRepo.AddModel(modelEntity))
                {
                    TempData["ActionMessage"] = "Unable to save Model";
                    TempData["ActionStatus"] = "Fail";
                }
                else
                {
                    TempData["ActionMessage"] = "Model saved successfully !";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");

                }
            }
            return View();
        }

        public async Task<IActionResult> Edit(long id)
        {
            
            ModelRepository ModelRepo = new ModelRepository(_dbContext);
            var products = await ModelRepo.GetProductById(id);
         
            var product = products.FirstOrDefault(m => m.ModelId == id);



            return View(product);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ModelEntity modelEntity)
        {
            // Retrieve current user id from session

            ModelRepository ModelRepo = new ModelRepository(_dbContext);
            // Check if the current user id is valid  


            // Call AddProductAsync method and await its completion
            if (await ModelRepo.UpdateModel(modelEntity))
            {
                // If AddProductAsync returns true, the user was added successfully
                TempData["InsertMessage"] = "Model inserted successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                // Handle the case when AddProductAsync returns false
                // For example, display an error message
                TempData["ErrorMessage"] = "Failed to insert Model.";
                return View(); // Or redirect to another action
            }

        }

        public async Task<IActionResult> Delete(long Id, ModelEntity modelEntity)
        {


            ModelRepository ModelRepo = new ModelRepository(_dbContext);
            var products = await ModelRepo.GetProductById(Id);
            long r = await ModelRepo.DeleteModel(Id);
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

            
        }




    }
}
