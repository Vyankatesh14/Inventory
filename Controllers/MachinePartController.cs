using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Models.Machine;

namespace Unioteq.TrackNTrace.Controllers
{
    public class MachinePartController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MachinePartController(ApplicationDBContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /MachinePart/Index
        public async Task<IActionResult> Index()
        {
            var machinePartList = await (from MP in _dbContext.MachineParts
                                     join M in _dbContext.Machines on MP.MachineId equals M.MachineId
                                     select new MachinePartEntity
                                     {
                                         MachinePartId = MP.MachinePartId,
                                         MachinePartName = MP.MachinePartName,
                                         MachinePartImage = MP.MachinePartImage,
                                         IsActive = MP.IsActive,
                                         MachineId = MP.MachineId,
                                         MachineName = M.MachineName // Fetch LineName from Lines table
                                     }).ToListAsync();

            return View(machinePartList);
        }

       
        // GET: /MachinePart/Create
        public async Task<IActionResult> Create()
        {
            var machines = await _dbContext.Machines.Select(m => new SelectListItem
            {
                Value = m.MachineId.ToString(),
                Text = m.MachineName
            }).ToListAsync();

            ViewBag.MachineList = new SelectList(machines, "Value", "Text");

            return View(new MachinePartView());
        }

        // POST: /MachinePart/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MachinePartView model)
        {
            if (ModelState.IsValid)
            {
                var machinePart = new MachinePartEntity
                {
                    MachinePartName = model.MachinePartName,
                    IsActive = model.IsActive,
                    MachineId = model.MachineId,
                    MachineName = model.MachineName
                };

                if (model.MachinePartImage != null)
                {
                    string fileName = Path.GetFileName(model.MachinePartImage.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "MachinePartImage", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.MachinePartImage.CopyToAsync(fileStream);
                    }

                    machinePart.MachinePartImage = fileName; // Save the file name in the database
                }

                _dbContext.MachineParts.Add(machinePart);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Machine part created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Populate ViewBag.MachineList with Machine options if needed
            ViewBag.MachineList = new SelectList(await _dbContext.Machines.ToListAsync(), "MachineId", "MachineName", model.MachineId);
            return View(model);
        }

        // GET: /MachinePart/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var machinePart = await _dbContext.MachineParts.FindAsync(id);
            if (machinePart == null)
            {
                return NotFound();
            }

            var machines = await _dbContext.Machines.Select(m => new SelectListItem
            {
                Value = m.MachineId.ToString(),
                Text = m.MachineName
            }).ToListAsync();

            ViewBag.MachineList = new SelectList(machines, "Value", "Text", machinePart.MachineId);

            var model = new MachinePartView
            {
                MachinePartId = machinePart.MachinePartId,
                MachinePartName = machinePart.MachinePartName,
             // This will be handled by file upload, so set to null
                IsActive = machinePart.IsActive,
                MachineId = machinePart.MachineId,
                MachineName = machinePart.MachineName
            };

            return View(model);
        }

        // POST: /MachinePart/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, MachinePartView model)
        {
            if (id != model.MachinePartId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var machinePart = await _dbContext.MachineParts.FindAsync(id);
                if (machinePart == null)
                {
                    return NotFound();
                }

                machinePart.MachinePartName = model.MachinePartName;
                machinePart.IsActive = model.IsActive;
                machinePart.MachineId = model.MachineId;
                machinePart.MachineName = model.MachineName;

                // Handle file upload
                if (model.MachinePartImage != null)
                {
                    string fileName = Path.GetFileName(model.MachinePartImage.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "MachinePartImage", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.MachinePartImage.CopyToAsync(fileStream);
                    }

                    machinePart.MachinePartImage = fileName; // Save the file name in the database
                }
                else
                {
                    // Retain existing image if no new file is uploaded
                    machinePart.MachinePartImage = machinePart.MachinePartImage; // Keep the current image
                }

                _dbContext.MachineParts.Update(machinePart);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Machine part updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Populate ViewBag.MachineList with Machine options if needed
            ViewBag.MachineList = new SelectList(await _dbContext.Machines.ToListAsync(), "MachineId", "MachineName", model.MachineId);
            return View(model);
        }




        // POST: /Machine/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var machinePart = await _dbContext.MachineParts.FindAsync(id);
            if (machinePart != null)
            {
                _dbContext.MachineParts.Remove(machinePart);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "MachinePart deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }



        public IActionResult ViewImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "MachinePartImage", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var mimeType = "application/octet-stream";

            // Determine the mime type based on the file extension (for better handling of different file types)
            var fileExtension = Path.GetExtension(fileName).ToLower();
            if (fileExtension == ".jpg" || fileExtension == ".jpeg")
            {
                mimeType = "image/jpeg";
            }
            else if (fileExtension == ".png")
            {
                mimeType = "image/png";
            }
            else if (fileExtension == ".pdf")
            {
                mimeType = "application/pdf";
            }

            return File(fileBytes, mimeType, fileName);
        }
    }
}
