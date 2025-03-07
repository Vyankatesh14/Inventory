using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Service.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Unioteq.TrackNTrace.Models;

namespace Unioteq.TrackNTrace.Controllers
{
    public class LineController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly LineRepository _lineRepository;
        private readonly PlantRepository _plantRepository;
        private readonly ShopFloorRepository _shopFloorRepository;

        public LineController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            _lineRepository = new LineRepository(_dbContext);
            _plantRepository = new PlantRepository(_dbContext);
            _shopFloorRepository = new ShopFloorRepository(_dbContext);
        }

        // GET: /Line/
        public async Task<IActionResult> Index()
        {
            var lines = await _lineRepository.GetLinesAsync();
            return View(lines);
        }

        // GET: /Line/Create
        public async Task<IActionResult> Create()
        {
            var plantList = await _plantRepository.Get();
            ViewBag.PlantList = new SelectList(plantList, "PlantId", "PlantName");

            var shopFloorList = await _shopFloorRepository.Get();
            ViewBag.ShopFloorList = new SelectList(shopFloorList, "ShopFloorId", "ShopFloorName");

            return View();
        }

        // POST: /Line/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LineEntity line)
        {
            if (ModelState.IsValid)
            {
                await _lineRepository.AddLineAsync(line);
                return RedirectToAction(nameof(Index));
            }

            // Reload the dropdown lists in case of validation failure
            var plantList = await _plantRepository.Get();
            ViewBag.PlantList = new SelectList(plantList, "PlantId", "PlantName");

            var shopFloorList = await _shopFloorRepository.Get();
            ViewBag.ShopFloorList = new SelectList(shopFloorList, "ShopFloorId", "ShopFloorName");

            return View(line);
        }

        // GET: /Line/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var line = await _lineRepository.GetLineByIdAsync(id);
            if (line == null)
            {
                return NotFound();
            }

            var plantList = await _plantRepository.Get();
            ViewBag.PlantList = new SelectList(plantList, "PlantId", "PlantName", line.PlantId);

            var shopFloorList = await _shopFloorRepository.Get();
            ViewBag.ShopFloorList = new SelectList(shopFloorList, "ShopFloorId", "ShopFloorName", line.ShopFloorId);

            return View(line);
        }

        // POST: /Line/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, LineEntity line)
        {
            if (id != line.LineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _lineRepository.UpdateLineAsync(line);
                return RedirectToAction(nameof(Index));
            }

            // Reload the dropdown lists in case of validation failure
            var plantList = await _plantRepository.Get();
            ViewBag.PlantList = new SelectList(plantList, "PlantId", "PlantName", line.PlantId);

            var shopFloorList = await _shopFloorRepository.Get();
            ViewBag.ShopFloorList = new SelectList(shopFloorList, "ShopFloorId", "ShopFloorName", line.ShopFloorId);

            return View(line);
        }

        // GET: /Line/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var line = await _lineRepository.GetLineByIdAsync(id);
            if (line == null)
            {
                return NotFound();
            }

            return View(line);
        }

        // POST: /Line/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _lineRepository.DeleteLineAsync(id);
            TempData["DeleteMessage"] = "<script>alert('Data deleted successfully...')</script>";
            return RedirectToAction(nameof(Index));
        }
    }
}
