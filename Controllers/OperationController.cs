using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Service.Repository;

namespace Unioteq.TrackNTrace.Controllers
{
    public class OperationController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly OperationRepository _operationRepository;


        public OperationController(ApplicationDBContext dbContext, OperationRepository operationRepository)
        {
            _dbContext = dbContext;
            _operationRepository = operationRepository;
        }

        private async Task PrepareDropdownsForView()
        {
            // Fetch lists of dropdowns
            var Plants = await _dbContext.plants
                .Select(p => new SelectListItem
                {
                    Value = p.PlantId.ToString(),
                    Text = p.PlantCode
                }).ToListAsync();

            var ShopFloors = await _dbContext.shopFloors
                .Select(sf => new SelectListItem
                {
                    Value = sf.ShopFloorId.ToString(),
                    Text = sf.ShopFloorCode,
                }).ToListAsync();

            var Lines = await _dbContext.Lines
               .Select(l => new SelectListItem
               {
                   Value = l.LineId.ToString(),
                   Text = l.LineCode,
               }).ToListAsync();

            var Models = await _dbContext.Models
              .Select(M => new SelectListItem
              {
                  Value = M.ModelId.ToString(),
                  Text = M.ModelCode,
              }).ToListAsync();

            var Station = await _dbContext.Stations
            .Select(s => new SelectListItem
            {
                Value = s.StationId.ToString(),
                Text = s.StationName,
            }).ToListAsync();

            // Store the lists in ViewBag
            ViewBag.PlantList = new SelectList(Plants, "Value", "Text");
            ViewBag.ShopFloorList = new SelectList(ShopFloors, "Value", "Text");
            ViewBag.lineList = new SelectList(Lines, "Value", "Text");
            ViewBag.ModelList = new SelectList(Models, "Value", "Text");
            ViewBag.StationList = new SelectList(Station, "Value", "Text");
        }
        public async Task<IActionResult> Index()
        {

            OperationRepository OperRepo = new OperationRepository(_dbContext);
            var productList = await OperRepo.GetOperationList();

            return View(productList);


        }

        public async Task<IActionResult> Create()
        {
            await PrepareDropdownsForView();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OperationsEntity operationEntity)
        {
            if (!ModelState.IsValid)
            {
                OperationRepository OperRepo = new OperationRepository(_dbContext);
                await PrepareDropdownsForView();

                if (await OperRepo.AddOperation(operationEntity))
                {
                    TempData["ActionMessage"] = "Operation saved successfully !";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");

                }
                else
                {
                    await PrepareDropdownsForView();
                    TempData["ActionMessage"] = "Unable to save Operation";
                    TempData["ActionStatus"] = "Fail";
                  

                }
             }
            return View();
        }
        public async Task<IActionResult> Edit(long id)
        {
            //long currentId = HttpContext.Session.Get<long>("UserId");
            OperationRepository OperRepo = new OperationRepository(_dbContext);
            var products = await OperRepo.GetOperationById(id);
            var product = products.FirstOrDefault(p => p.OperationId == id);


            await PrepareDropdownsForView();
            return View(product);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OperationsEntity operationEntity)
        {
           

            OperationRepository OperRepo = new OperationRepository(_dbContext);
            await PrepareDropdownsForView();

            if (await OperRepo.UpdateOperation(operationEntity))
            {
                TempData["InsertMessage"] = "operation inserted successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                
                TempData["ErrorMessage"] = "Failed to insert operation.";
                await PrepareDropdownsForView();
                return View();
            }

        }

        /* public async Task<IActionResult> Delete(long Id, OperationsEntity operationEntity)
         {


             OperationRepository OperRepo = new OperationRepository(_dbContext);
             var products = await OperRepo.GetOperationById(Id);
             long r = await OperRepo.DeleteOperation(Id);
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
         }*/

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                // Call the repository method to soft delete the user
                await _operationRepository.DeleteOperation(id);
                TempData["SuccessMessage"] = "User has been soft deleted.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the user.";
            }

            return RedirectToAction("Index");
        }
    }
}
