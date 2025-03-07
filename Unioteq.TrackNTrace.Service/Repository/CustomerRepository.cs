using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;

namespace Unioteq.TrackNTrace.Service.Repository
{
    
   public class CustomerRepository
   {
        private readonly ApplicationDBContext   _dbContext;

        public CustomerRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CustomerEntity>> Get()
        {
            try
            {
                var customer = await _dbContext.Customer.FromSqlRaw("EXEC GetCustomerList").ToListAsync();

                return customer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching Customer: {ex.Message}");
                throw;
            }

        }

        public async Task<bool> AddCustomer (CustomerEntity customer)
        {
            var parameter = new List<SqlParameter>
            {
                new SqlParameter("@CustomerName", customer.CustomerName),
                new SqlParameter("@PlantId", customer.PlantId),
                new SqlParameter("@Contact", customer.Contact),
                new SqlParameter("@Address", customer.Address),
                new SqlParameter("@IsActive", customer.IsActive)
            };

            try
            {
                int result = await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"insertCustomer @CustomerName,@PlantId,@Contact,@Address,@IsActive", parameter.ToArray()));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the Customer: {ex.Message}");
                throw;
            }
        }



        public async Task<IEnumerable<CustomerEntity>> GetCustomerById(long customerId)
        {
            try
            {
                var param = new SqlParameter("@CustomerId", customerId); // Use correct parameter name

                var customerById = await _dbContext.Customer
                    .FromSqlRaw("EXEC GetCustomerById @CustomerId", param) // Use EXEC with correct parameter
                    .ToListAsync();

                return customerById;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching the customer by ID: {ex.Message}");
                throw;
            }
        }







        public async Task<long> DeleteCustomer(long customerId)
        {
            try
            {
                return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"DeleteCustomer {customerId}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the Customer: {ex.Message}");
                throw;
            }
        }



        public async Task<bool> UpdateCustomer(CustomerEntity customer)
        {
            var parameter = new List<SqlParameter> 
            {
                new SqlParameter("@CustomerId", customer.CustomerId),
                new SqlParameter("@CustomerName", customer.CustomerName),
                new SqlParameter("@PlantId", customer.PlantId),
                new SqlParameter("@Contact", customer.Contact),
                new SqlParameter("@Address", customer.Address),
                new SqlParameter("@IsActive", customer.IsActive)

            };

            try
            {
                long result = await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"UpdateCustomer @CustomerId, @CustomerName, @PlantId,@Contact,@Address,@IsActive" , parameter.ToArray()));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the Customer: {ex.Message}");
                throw;
            }

        }

   }
}
