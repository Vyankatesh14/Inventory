using Microsoft.AspNetCore.Mvc;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Service.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Unioteq.TrackNTrace.Controllers
{
    public class PlantController : Controller
    {

        private readonly ApplicationDBContext _dbContext;


        public PlantController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            
        }


        public async Task<IActionResult> Index()
        {
          
            PlantRepository userRepo = new PlantRepository(_dbContext);
            var productList = await userRepo.Get();

            return View(productList);


        }


        public async Task<IActionResult> Create()
        {



            return View();
        }








        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlantEntity plantEntity)
        {


            // Retrieve current user id from session
            /*  long currentId = HttpContext.Session.Get<long>("UserId");
              PlantRepository plantRepo = new PlantRepository(_dbContext);
              // Check if the current user id is valid  


              // Call AddProductAsync method and await its completion
              if (await plantRepo.AddPlantAsync(plantEntity, currentId))
              {
                  // If AddProductAsync returns true, the user was added successfully
                  TempData["InsertMessage"] = "User inserted successfully.";
                  return RedirectToAction("Index");
              }
              else
              {
                  // Handle the case when AddProductAsync returns false
                  // For example, display an error message
                  TempData["ErrorMessage"] = "Failed to insert user.";
                  return View(); // Or redirect to another action
              }
            */



            if (ModelState.IsValid)
            {
                PlantRepository plantRepo = new PlantRepository(_dbContext);
              
                if (await plantRepo.AddPlant(plantEntity))
                {
                    TempData["ActionMessage"] = "Unable to save Plant";
                    TempData["ActionStatus"] = "Fail";
                }
                else
                {
                    TempData["ActionMessage"] = "Plant saved successfully !";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");
                   
                }
            }
            return View();
        }




        public async Task<IActionResult> Edit(long id)
        {
            //long currentId = HttpContext.Session.Get<long>("UserId");
            PlantRepository roleRepo = new PlantRepository(_dbContext);
            var products = await roleRepo.GetProductById(id);
            //List<product> products = GetProducts();
            var product = products.FirstOrDefault(p => p.PlantId == id);



            return View(product);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlantEntity plantEntity)
        {
            // Retrieve current user id from session
        
            PlantRepository plantRepo = new PlantRepository(_dbContext);
            // Check if the current user id is valid  


            // Call AddProductAsync method and await its completion
            if (await plantRepo.UpdatePlant(plantEntity))            {
                // If AddProductAsync returns true, the user was added successfully
                TempData["InsertMessage"] = "User inserted successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                // Handle the case when AddProductAsync returns false
                // For example, display an error message
                TempData["ErrorMessage"] = "Failed to insert user.";
                return View(); // Or redirect to another action
            }

        }














        public async Task<IActionResult> Delete(long Id, PlantEntity plantEntity)
        {

          
            PlantRepository plantRepo = new PlantRepository(_dbContext);
            var products = await plantRepo.GetProductById(Id);
            long r = await plantRepo.DeletePlant(Id);
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
