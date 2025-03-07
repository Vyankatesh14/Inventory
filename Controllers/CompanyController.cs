using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Models.Machine;
using Unioteq.TrackNTrace.Service.Repository;

namespace Unioteq.TrackNTrace.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CompanyController(ApplicationDBContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Index()
        {
            var companyList = await (from MP in _dbContext.Companies
                                         join M in _dbContext.Companies on MP.CompId equals M.CompId
                                         select new CompanyEntity
                                         {
                                             CompId = MP.CompId,
                                             CompanyName = MP.CompanyName,
                                             CompLogo = MP.CompLogo,
                                             CompCorpAdd = MP.CompCorpAdd,
                                             CompClientAdd = MP.CompClientAdd,
                                             CompCode = MP.CompCode,
                                             IsActive = MP.IsActive,
                                              
                                         }).ToListAsync();

            return View(companyList);
        }




        // GET: /Company/Create
        public async Task<IActionResult> Create()
        {
            var company = await _dbContext.Companies.Select(m => new SelectListItem
            {
                Value = m.CompId.ToString(),
                Text = m.CompanyName
            }).ToListAsync();


            return View(new CompanyView());
        }

        // POST: /Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyView model)
        {
            if (ModelState.IsValid)
            {
                var company = new CompanyEntity
                {
                    CompanyName = model.CompanyName,
                    CompCorpAdd = model.CompCorpAdd,
                    CompClientAdd = model.CompClientAdd,
                    CompCode = model.CompCode,
                    IsActive = model.IsActive

                };

                if (model.CompLogo != null)
                {
                    string fileName = Path.GetFileName(model.CompLogo.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Company", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.CompLogo.CopyToAsync(fileStream);
                    }

                    company.CompLogo = fileName; // Save the file name in the database
                }

                _dbContext.Companies.Add(company);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Company created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);

        }






        public async Task<IActionResult> Edit(long id)
        {
            var company = await _dbContext.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

           

            var model = new CompanyView
            {
                CompId = company.CompId,
                CompanyName = company.CompanyName,
             
                CompCorpAdd = company.CompCorpAdd,
                CompClientAdd = company.CompClientAdd,
                CompCode = company.CompCode,
                // This will be handled by file upload, so set to null
                IsActive = company.IsActive,
               
            };

            return View(model);
        }

        // POST: /Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, CompanyView model)
        {
            if (id != model.CompId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var company = await _dbContext.Companies.FindAsync(id);
                if (company == null)
                {
                    return NotFound();
                }

                company.CompanyName = model.CompanyName;
                company.CompCorpAdd = model.CompCorpAdd;
                company.CompClientAdd = model.CompClientAdd;
                company.CompCode = model.CompCode;
                company.IsActive = model.IsActive;
               

                // Handle file upload
                if (model.CompLogo != null)
                {
                    string fileName = Path.GetFileName(model.CompLogo.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Company", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.CompLogo.CopyToAsync(fileStream);
                    }

                    company.CompLogo = fileName; // Save the file name in the database
                }
                else
                {
                    // Retain existing image if no new file is uploaded
                    company.CompLogo = company.CompLogo; // Keep the current image
                }

                _dbContext.Companies.Update(company);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Company part updated successfully!";
                return RedirectToAction(nameof(Index));
            }

           
            return View(model);
        }



        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var company = await _dbContext.Companies.FindAsync(id);
            if(company != null)
            {
                _dbContext.Companies.Remove(company);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Company deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }





        public IActionResult ViewImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Company", fileName);

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
