using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StructureMap.Diagnostics.TreeView;
using System.Reflection.PortableExecutable;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Models.Machine;
using Unioteq.TrackNTrace.Service.Repository;

namespace Unioteq.TrackNTrace.Controllers
{
    public class ModelPartsController : Controller
    {
        private readonly ModelPartsRepository _repository;
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

       
        public ModelPartsController(ModelPartsRepository repository, ApplicationDBContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
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
            var ModelPart = await _repository.GetAllModelParts();
            return View(ModelPart);
        }

        public async Task<IActionResult> Create()
        {
            await PrepareDropdownsForView();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModelPartView modelPart)
        {
            if (!ModelState.IsValid)
            {
                await PrepareDropdownsForView();    
               


                var modelParts = new ModelPartsEntity
                {
                    StationId = long.TryParse(modelPart.StationId.ToString(), out var stationId) ? stationId : 0,
                    ModelId = long.TryParse(modelPart.ModelId.ToString(), out var modelId) ? modelId : 0,
                    ModelPartName = modelPart.ModelPartName,
                    ModelPartDescription = modelPart.ModelPartDescription,
                    Sequence = modelPart.Sequence,
                    ModelPartCode = modelPart.ModelCode,
                    LineId = long.TryParse(modelPart.LineId.ToString(), out var lineId) ? lineId : 0,
                    PlantId = long.TryParse(modelPart.PlantId.ToString(), out var plantId) ? plantId : 0,
                    ShopFloorId = long.TryParse(modelPart.ShopFloorId.ToString(), out var shopFloorId) ? shopFloorId : 0,
                    IsActive = bool.TryParse(modelPart.IsActive.ToString(), out var isActive) ? isActive : false
                };

                if (modelPart.ModelPartImage != null)
                {
                    try
                    {
                        string fileName = Path.GetFileName(modelPart.ModelPartImage.FileName);
                        string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ModelPartImages");
                        Directory.CreateDirectory(uploadFolder);
                        string filePath = Path.Combine(uploadFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await modelPart.ModelPartImage.CopyToAsync(fileStream);
                        }

                        modelParts.ModelPartImage = uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Image upload failed. Please try again.");
                        await PrepareDropdownsForView();
                        return View(modelPart);
                    }
                }


                bool result = await _repository.AddModelParts(modelParts);
                if (result)
                {
                    TempData["SuccessMessage"] = "Model Part created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }

                ModelState.AddModelError("", "Failed to create Model Part.");
                await PrepareDropdownsForView();
                return View(modelPart);
            
        }
        public async Task<IActionResult> Edit(long id)
        {
            var modelPart = await _dbContext.ModelParts.FindAsync(id);
            if (modelPart == null)
            {
                return NotFound();
            }



            await PrepareDropdownsForView();

            var model = new ModelPartView
            {
                ModelPartId = long.TryParse(modelPart.ModelPartId.ToString(), out var ModelPartId) ? ModelPartId : 0,
                StationId = long.TryParse(modelPart.StationId.ToString(), out var stationId) ? stationId : 0,
                ModelId = long.TryParse(modelPart.ModelId.ToString(), out var modelId) ? modelId : 0,
                ModelPartName = modelPart.ModelPartName,
                ModelPartDescription = modelPart.ModelPartDescription,
                Sequence = modelPart.Sequence,
                ModelPartCode = modelPart.ModelCode,
                LineId = long.TryParse(modelPart.LineId.ToString(), out var lineId) ? lineId : 0,
                PlantId = long.TryParse(modelPart.PlantId.ToString(), out var plantId) ? plantId : 0,
                ShopFloorId = long.TryParse(modelPart.ShopFloorId.ToString(), out var shopFloorId) ? shopFloorId : 0,
                IsActive = bool.TryParse(modelPart.IsActive.ToString(), out var isActive) ? isActive : false
                //IsActive = modelPart.IsActive ? "Yes" : "No",


            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ModelPartView model)
        {
            if (id != model.ModelPartId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var ModelPart = await _dbContext.ModelParts.FindAsync(id);
                if (ModelPart == null)
                {
                    return NotFound();
                }

                ModelPart.ModelPartId = model.ModelPartId;
                ModelPart.StationId = model.StationId;
                ModelPart.StationCode = model.StationCode;
                ModelPart.ModelId = model.ModelId;
                ModelPart.ModelPartName = model.ModelPartName;
                ModelPart.ModelCode = model.ModelCode;
                ModelPart.ModelPartDescription = model.ModelPartDescription;
                ModelPart.Sequence = model.Sequence;
                ModelPart.ModelPartCode = model.ModelPartCode;
                ModelPart.LineId = model.LineId;
                 ModelPart.LineCode = model.LineCode;
                ModelPart.PlantId = model.PlantId;
                ModelPart.PlantCode = model.PlantCode;
                ModelPart.ShopFloorId = model.ShopFloorId;
                ModelPart.ShopFloorCode = model.ShopFloorCode;
              //  ModelPart.IsActive = bool.Parse(model.IsActive);



                // Handle file upload
                if (model.ModelPartImage != null)
                {
                    string fileName = Path.GetFileName(model.ModelPartImage.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "ModelPartImage", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ModelPartImage.CopyToAsync(fileStream);
                    }

                    ModelPart.ModelPartImage = fileName; // Save the file name in the database
                }

                _dbContext.ModelParts.Update(ModelPart);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "ModelPart updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Populate ViewBag.LineList with Line options if needed
            await PrepareDropdownsForView();
            return View(model);
        }



        // POST: /Machine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var ModelPart = await _dbContext.ModelParts.FindAsync(id);
            if (ModelPart != null)
            {
                _dbContext.ModelParts.Remove(ModelPart);
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

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "ModelPartImage", fileName);

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
    
