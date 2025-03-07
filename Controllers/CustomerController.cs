using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Service.Repository;

namespace Unioteq.TrackNTrace.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public CustomerController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            CustomerRepository customerRepo = new CustomerRepository(_dbContext);
            var customerList = await customerRepo.Get();
            return View(customerList);
        }

        public async Task<IActionResult> Create()
        {
            PlantRepository plantRepo = new PlantRepository(_dbContext);
            var plantList = await plantRepo.Get();
            ViewBag.plantList = new SelectList(plantList, "PlantId", "PlantName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CustomerEntity customer)
        {
            try
            {
                CustomerRepository customerRepo = new CustomerRepository(_dbContext);
                if (await customerRepo.AddCustomer(customer))
                {
                    TempData["ActionMessage"] = "Customer Saved Successfully";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ActionMessage"] = "Customer Unable to Save";
                    TempData["ActionStatus"] = "Fail";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the Customer: " + ex.Message;

            }
            return View();
        }

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                CustomerRepository customerRepo = new CustomerRepository(_dbContext);
                long result = await customerRepo.DeleteCustomer(id);
                if (result > 0)
                {
                    TempData["ActionMessage"] = "Customer Deleted Successfully";
                    TempData["ActionStatus"] = "Success";
                }
                else
                {
                    TempData["ActionMessage"] = "Customer Unable to Delete";
                    TempData["ActionStatus"] = "Fail";
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the Customer: " + ex.Message;

            }
            return RedirectToAction("index");

        }


        public async Task<IActionResult> Edit(long id)
        {
            CustomerRepository customerRepo = new CustomerRepository(_dbContext);
            var customer = await customerRepo.GetCustomerById(id);


            PlantRepository plantRepo = new PlantRepository(_dbContext);
            var plantList = await plantRepo.Get();
            ViewBag.PlantList = new SelectList(plantList, "PlantId", "PlantName", customer.FirstOrDefault()?.PlantId);

            return View(customer.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit (CustomerEntity customer)
        {
            try
            {
                CustomerRepository customerRepo = new CustomerRepository(_dbContext);
                if(await customerRepo.UpdateCustomer(customer))
                {
                    TempData["ActionMessage"] = "Customer Updated Successfully";
                    TempData["ActionStatus"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ActionMessage"] = "Customer Unable to Update";
                    TempData["ActionStatus"] = "Fail";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the customer: " + ex.Message;

            }
            return View();
        }

    }
}
