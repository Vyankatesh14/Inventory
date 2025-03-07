
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Service.Repository;

namespace Unioteq.TrackNTrace.Controllers
{
    public class StationController : Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public StationController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            StationRepository stationRepo = new StationRepository(_dbContext);
            var stationList = await stationRepo.Get();
            return View(stationList);
        }

        public async Task<IActionResult> Create()
        {
            PlantRepository PlantRepo = new PlantRepository(_dbContext);
            var PlantList = await PlantRepo.Get();
            ViewBag.PlantList = new SelectList(PlantList, "PlantId", "PlantName");

            ShopFloorRepository shopFloorRepo = new ShopFloorRepository(_dbContext);
            var shopFloorList = await shopFloorRepo.Get();
            ViewBag.ShopFloorList = new SelectList(shopFloorList, "ShopFloorId", "ShopFloorName");

            LineRepository lineRepo = new LineRepository(_dbContext);
            var lineList = await lineRepo.GetLinesAsync();
            ViewBag.LineList = new SelectList(lineList, "LineId", "LineName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StationEntity station)
        {
            try
            {
                StationRepository stationRepo = new StationRepository(_dbContext);
                if (await stationRepo.AddStation(station))
                {
                    TempData["ActionMessage"] = "Station Saved Successfully";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ActionMessage"] = "Station Unable to Save";
                    TempData["ActionStatus"] = "Fail";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the station: " + ex.Message;
            }
            return View();
        }

        public async Task<IActionResult> Edit(long id)
        {
            StationRepository stationRepo = new StationRepository(_dbContext);
            var station = await stationRepo.GetStationById(id);

            PlantRepository plantRepo = new PlantRepository(_dbContext);
            var plantList = await plantRepo.Get();
            ViewBag.PlantList = new SelectList(plantList, "PlantId", "PlantName", station.FirstOrDefault()?.PlantId);

            ShopFloorRepository shopFloorRepo = new ShopFloorRepository(_dbContext);
            var shopFloorList = await shopFloorRepo.Get();
            ViewBag.ShopFloorList = new SelectList(shopFloorList, "ShopFloorId", "ShopFloorName", station.FirstOrDefault()?.ShopFloorId);

            LineRepository lineRepo = new LineRepository(_dbContext);
            var lineList = await lineRepo.GetLinesAsync();
            ViewBag.LineList = new SelectList(lineList, "LineId", "LineName", station.FirstOrDefault()?.LineId);

            return View(station.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StationEntity station)
        {
            try
            {
                StationRepository stationRepo = new StationRepository(_dbContext);
                if (await stationRepo.UpdateStation(station))
                {
                    TempData["ActionMessage"] = "Station Updated Successfully";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ActionMessage"] = "Station Unable to Update";
                    TempData["ActionStatus"] = "Fail";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the station: " + ex.Message;
            }
            return View();
        }

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                StationRepository stationRepo = new StationRepository(_dbContext);
                long result = await stationRepo.DeleteStation(id);
                if (result > 0)
                {
                    TempData["ActionMessage"] = "Station Deleted Successfully";
                    TempData["ActionStatus"] = "Success";
                }
                else
                {
                    TempData["ActionMessage"] = "Station Unable to Delete";
                    TempData["ActionStatus"] = "Fail";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the station: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
