using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Service.Repository;

namespace Unioteq.TrackNTrace.Controllers
{
    public class ShopFloorController : Controller
    {
        private readonly ApplicationDBContext _dbContext;


        public ShopFloorController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IActionResult> Index()
        {

            ShopFloorRepository shopFlooRepo= new ShopFloorRepository(_dbContext);
            var ShopFloorList = await shopFlooRepo.Get();

            return View(ShopFloorList);


        }

        public  async Task<IActionResult> Create()
        {
            PlantRepository PlantRepo = new PlantRepository(_dbContext);
            //List<PlantEntity> plants = await PlantRepo.Get();
            // SelectList plantList = new SelectList(plants, "PlantId", "PlantName");
            // ViewBag.plantList = new SelectList(PlantList, "PlantId", "PlantName");

            var PlantList = await PlantRepo.Get();
            ViewBag.PlantList = new SelectList(PlantList, "PlantId", "PlantName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShopFloorEntity shop)
        {
           // long currentId = Convert.ToInt64(Session["UserId"]);
            try
            {

                ShopFloorRepository shopFlooRepo = new ShopFloorRepository(_dbContext);
                if ( await shopFlooRepo.AddShopFloor(shop))
                {
                    TempData["ActionMessage"] = "ShopFloor Saved Successfully";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ActionMessage"] = "ShopFloor Unable Saved ";
                    TempData["ActionStatus"] = "Fail";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the file: " + ex.Message;
            }
            return View();
        }





        public async Task<IActionResult> Edit(long id)
        {
            // Properly await GetShopFloorById
            ShopFloorRepository shopFlooRepo = new ShopFloorRepository(_dbContext);
            var shop = await shopFlooRepo.GetShopFloorById(id); // Add await here

            PlantRepository PlantRepo = new PlantRepository(_dbContext);
            List<PlantEntity> plants = await PlantRepo.Get(); // This is correct, already awaited

            SelectList plantList = new SelectList(plants, "PlantId", "PlantName", shop.FirstOrDefault()?.PlantId);
            ViewBag.PlantList = plantList;

            return View(shop.FirstOrDefault()); // Ensure you pass the first entity or null if none found
        }


           [HttpPost]
         public async Task<IActionResult> Edit(ShopFloorEntity shop)
         {
           ShopFloorRepository shopFlooRepo = new ShopFloorRepository(_dbContext);
           PlantRepository plantRepo = new PlantRepository(_dbContext);

                // Ensure UpdateShopFloor is properly awaited
             if (await shopFlooRepo.UpdateShopFloor(shop))
             {
                TempData["InsertMessage"] = "User inserted successfully.";
                return RedirectToAction("Index");
             }
             else
             {
                 TempData["ErrorMessage"] = "Failed to insert user.";
                  return View(shop); // Pass shop entity to view in case of failure
             }
            }





        public async Task<IActionResult> Delete(long Id, ShopFloorEntity shopFloor)
        {


            ShopFloorRepository shopRepo = new ShopFloorRepository(_dbContext);
            var products = await shopRepo.GetShopFloorById(Id);
            long r = await shopRepo.DeleteShopFloor(Id);
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
