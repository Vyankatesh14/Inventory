using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Service.Repository;

namespace Unioteq.TrackNTrace.Controllers
{
    public class DeviceMasterController : Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public DeviceMasterController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
           DeviceMasterRepository devicemasterRepo = new DeviceMasterRepository(_dbContext);
            var devicemasterList = await devicemasterRepo.Get();
            return View(devicemasterList);
        }



        public async Task<IActionResult> Create()
        {
            // Fetching company data from database
            // Fetch companies from the database
            var companies = _dbContext.Companies.ToList();

            // Check if companies list is not null or empty
            if (companies == null || !companies.Any())
            {
                // Handle the case where no companies are found
                ModelState.AddModelError("", "No companies available. Please add a company first.");
            }

            // Set ViewBag.CompanyList to a SelectList
            ViewBag.CompanyList = new SelectList(companies, "CompId", "CompanyName");

      
            PlantRepository PlantRepo = new PlantRepository(_dbContext);
            var PlantList = await PlantRepo.Get();
            ViewBag.PlantList = new SelectList(PlantList, "PlantId", "PlantName");

            LineRepository lineRepo = new LineRepository(_dbContext);
            var lineList = await lineRepo.GetLinesAsync();
            ViewBag.LineList = new SelectList(lineList, "LineId", "LineName");


            return View();

        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeviceMasterEntity devicemaster)
        {
            try
            {
                DeviceMasterRepository devicemasterRepo = new DeviceMasterRepository(_dbContext);
                if (await devicemasterRepo.AddDeviceMaster(devicemaster))
                {
                    TempData["ActionMessage"] = "DeviceMaster Saved Successfully";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ActionMessage"] = "DeviceMaster Unable to Save";
                    TempData["ActionStatus"] = "Fail";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the DeviceMaster: " + ex.Message;
            }
            return View();
        }




        public async Task<IActionResult> Edit(long id)
        {
            DeviceMasterRepository deviceRepo = new DeviceMasterRepository(_dbContext);
            var device = await deviceRepo.GetDeviceMasterById(id);

            var companies = _dbContext.Companies.ToList();

            // Check if companies list is not null or empty
            if (companies == null || !companies.Any())
            {
                // Handle the case where no companies are found
                ModelState.AddModelError("", "No companies available. Please add a company first.");
            }

            // Set ViewBag.CompanyList to a SelectList
            ViewBag.CompanyList = new SelectList(companies, "CompId", "CompanyName");

            PlantRepository plantRepo = new PlantRepository(_dbContext);
            var plantList = await plantRepo.Get();
            ViewBag.PlantList = new SelectList(plantList, "PlantId", "PlantName", device.FirstOrDefault()?.PlantId);

            LineRepository lineRepo = new LineRepository(_dbContext);
            var lineList = await lineRepo.GetLinesAsync();
            ViewBag.LineList = new SelectList(lineList, "LineId", "LineName", device.FirstOrDefault()?.LineId);

            return View(device.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DeviceMasterEntity device)
        {
            try
            {
                DeviceMasterRepository deviceRepo = new DeviceMasterRepository(_dbContext);
                if (await deviceRepo.UpdateDeviceMaster(device))
                {
                    TempData["ActionMessage"] = "DeviceMaster Updated Successfully";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ActionMessage"] = "DeviceMaster Unable to Update";
                    TempData["ActionStatus"] = "Fail";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the DeviceMaster: " + ex.Message;
            }
            return View();
        }




        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                DeviceMasterRepository deviceRepo = new DeviceMasterRepository(_dbContext);
                long result = await deviceRepo.DeleteDeviceMaster(id);
                if (result > 0)
                {
                    TempData["ActionMessage"] = "DeviceMaster Deleted Successfully";
                    TempData["ActionStatus"] = "Success";
                }
                else
                {
                    TempData["ActionMessage"] = "DeviceMasster Unable to Delete";
                    TempData["ActionStatus"] = "Fail";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the DeviceMaster: " + ex.Message;
            }
            return RedirectToAction("Index");
        }




    }
}
