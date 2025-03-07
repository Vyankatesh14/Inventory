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
    public class MachineController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MachineController(ApplicationDBContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /Machine
        public async Task<IActionResult> Index()
        {
            
                // Using a join to fetch LineName along with machine details
                var machineList = await (from m in _dbContext.Machines
                                         join l in _dbContext.Lines on m.LineId equals l.LineId
                                         select new MachineEntity
                                         {
                                             MachineId = m.MachineId,
                                             MachineName = m.MachineName,
                                             MachineCode = m.MachineCode,
                                             MachineDetail = m.MachineDetail,
                                             MachineManual = m.MachineManual,
                                             IsActive = m.IsActive,
                                             LineId = m.LineId,
                                             LineName = l.LineName // Fetch LineName from Lines table
                                         }).ToListAsync();

                return View(machineList);
            }
        
         
        // GET: /Machine/Create
        public async Task<IActionResult> Create()
        {
            // Populate LineList for dropdown
            var lines = await _dbContext.Lines.Select(l => new SelectListItem
            {
                Value = l.LineId.ToString(),
                Text = l.LineName
            }).ToListAsync();

            ViewBag.LineList = new SelectList(lines, "Value", "Text");
            return View();
        }

        // POST: /Machine/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MachineView model)
        {
            if (ModelState.IsValid)
            {
                var machine = new MachineEntity
                {
                    MachineName = model.MachineName,
                    MachineCode = model.MachineCode,
                    MachineDetail = model.MachineDetail,
                    IsActive = model.IsActive,
                    LineId = model.LineId,
                    
                };

                // Handle file upload
                if (model.MachineManual != null)
                {
                    string fileName = Path.GetFileName(model.MachineManual.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "MachineManual", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.MachineManual.CopyToAsync(fileStream);
                    }

                    machine.MachineManual = fileName;
                }

                _dbContext.Machines.Add(machine);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Machine created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Populate ViewBag.LineList with Line options if needed
            ViewBag.LineList = new SelectList(await _dbContext.Lines.ToListAsync(), "LineId", "LineName");
            return View(model);
        }

        // GET: /Machine/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var machine = await _dbContext.Machines.FindAsync(id);
            if (machine == null)
            {
                return NotFound();
            }

            var lines = await _dbContext.Lines.Select(l => new SelectListItem
            {
                Value = l.LineId.ToString(),
                Text = l.LineName
            }).ToListAsync();

            ViewBag.LineList = new SelectList(lines, "Value", "Text", machine.LineId);

            var model = new MachineView
            {
                MachineId = machine.MachineId,
                MachineName = machine.MachineName,
                MachineCode = machine.MachineCode,
                MachineDetail = machine.MachineDetail,
                MachineManual = null, // Will be handled in the POST method
                IsActive = machine.IsActive,
                LineId = machine.LineId
            };

            return View(model);
        }


        // POST: /Machine/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, MachineView model)
        {
            if (id != model.MachineId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var machine = await _dbContext.Machines.FindAsync(id);
                if (machine == null)
                {
                    return NotFound();
                }

                machine.MachineName = model.MachineName;
                machine.MachineCode = model.MachineCode;
                machine.MachineDetail = model.MachineDetail;
                machine.IsActive = model.IsActive;
                machine.LineId = model.LineId;
               

                // Handle file upload
                if (model.MachineManual != null)
                {
                    string fileName = Path.GetFileName(model.MachineManual.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "MachineManual", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.MachineManual.CopyToAsync(fileStream);
                    }

                    machine.MachineManual = fileName; // Save the file name in the database
                }

                _dbContext.Machines.Update(machine);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Machine updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Populate ViewBag.LineList with Line options if needed
            ViewBag.LineList = new SelectList(await _dbContext.Lines.ToListAsync(), "LineId", "LineName", model.LineId);
            return View(model);
        }



        // POST: /Machine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var machine = await _dbContext.Machines.FindAsync(id);
            if (machine != null)
            {
                _dbContext.Machines.Remove(machine);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Machine deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ViewManual(string fileName)
{
    if (string.IsNullOrEmpty(fileName))
    {
        return NotFound();
    }

    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "MachineManual", fileName);

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
