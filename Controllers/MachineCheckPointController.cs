using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Service.Repository;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Unioteq.TrackNTrace.Models;
using Microsoft.EntityFrameworkCore;
using StructureMap;

namespace Unioteq.TrackNTrace.Controllers
{
    public class MachineCheckPointController : Controller
    {
        private readonly MachineCheckPointRepository _repository;
        private readonly ApplicationDBContext _dbContext;

        public MachineCheckPointController(MachineCheckPointRepository repository, ApplicationDBContext dbContext)
        {
            _repository = repository;
            _dbContext = dbContext;
        }



        private async Task PrepareDropdownsForView()
        {
            // Fetch lists of machines and machine parts for dropdowns
            var machines = await _dbContext.Machines
                .Select(m => new SelectListItem
                {
                    Value = m.MachineId.ToString(),
                    Text = m.MachineName
                }).ToListAsync();

            var machineParts = await _dbContext.MachineParts
                .Select(mp => new SelectListItem
                {
                    Value = mp.MachinePartId.ToString(),
                    Text = mp.MachinePartName
                }).ToListAsync();

            // Store the lists in ViewBag
            ViewBag.MachineList = new SelectList(machines, "Value", "Text");
            ViewBag.MachinePartList = new SelectList(machineParts, "Value", "Text");
        }



        public async Task<IActionResult> Index()
        {
            var checkPoints = await _repository.GetAllCheckPoints();
            return View(checkPoints);
        }



        public async Task<IActionResult> Create()
        {
            await PrepareDropdownsForView();
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MachineCheckPointEntity checkPoint)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.AddCheckPoint(checkPoint))
                {
                    TempData["Message"] = "CheckPoint added successfully!";
                }
                    return RedirectToAction(nameof(Index));
                //ModelState.AddModelError("", "Failed to add CheckPoint.");
            }

            // Repopulate dropdowns in case of validation errors
            await PrepareDropdownsForView();
            return View(checkPoint);
        }


        // Edit GET method
        public async Task<IActionResult> Edit(int id)
        {
            var checkPoint = await _repository.GetCheckPointById(id);
            if (checkPoint == null)
            {
                return NotFound();
            }

            await PrepareDropdownsForView();
            return View(checkPoint);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MachineCheckPointEntity checkPoint)
        {
            if (!ModelState.IsValid)
            {
                await PrepareDropdownsForView(); // Repopulate dropdowns if validation fails
                return View(checkPoint); // Return the view with the model to show validation errors
            }

            // Check for required CheckMethod
            if (string.IsNullOrEmpty(checkPoint.CheckMethod))
            {
                ModelState.AddModelError("CheckMethod", "Check Method is required.");
                await PrepareDropdownsForView(); // Repopulate dropdowns if validation fails
                return View(checkPoint);
            }

            // Attempt to update the checkpoint
            bool updateSuccessful = await _repository.UpdateCheckPoint(checkPoint);
            if (updateSuccessful)
            {
                TempData["Message"] = "CheckPoint updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Failed to update CheckPoint.");
            await PrepareDropdownsForView(); // Repopulate dropdowns if the update failed
            return View(checkPoint);
        }




        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var success = await _repository.DeleteCheckPoint(id);
            if (success)
            {
                TempData["SuccessMessage"] = "CheckPoint deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete CheckPoint.";
            }

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> GetMachineParts(int machineId)
        {
            var parts = await _dbContext.MachineParts
                .Where(mp => mp.MachineId == machineId)
                .Select(mp => new
                {
                    id = mp.MachinePartId,
                    name = mp.MachinePartName
                }).ToListAsync();

            return Json(parts);
        }
    }
}
