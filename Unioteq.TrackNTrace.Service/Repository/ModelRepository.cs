using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Unioteq.TrackNTrace.Service.Repository
{
   public class ModelRepository
    {
        private readonly ApplicationDBContext _dbContext;


        public ModelRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ModelEntity>> Get()
        {
            try
            {

                var model = await _dbContext.Models
                    .FromSqlRaw("EXEC GetModelList")
                    .ToListAsync();



                return model;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred while fetching plants: {ex.Message}");
                throw; // Rethrow the exception to propagate it upwards
            }
        }

        public async Task<bool> AddModel(ModelEntity model)
        {

            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@ModelName", model.ModelName));
            parameter.Add(new SqlParameter("@ModelCode", model.ModelCode));
            parameter.Add(new SqlParameter("@ModelDescription", model.ModelDescription));
            parameter.Add(new SqlParameter("@IsActive", model.IsActive));

            int result = await Task.Run(() => _dbContext.Database
               .ExecuteSqlRawAsync(@"InsertModel    @ModelName,@ModelCode,@ModelDescription,@IsActive", parameter.ToArray()));

            return result > 0;


        }

        public async Task<long> DeleteModel(long ModelId)
        {
            var sql = $"EXEC DeleteModel @ModelId";
            var parameters = new object[] { ModelId };

            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"DeleteModel {ModelId}"));
        }

        public async Task<IEnumerable<ModelEntity>> GetProductById(long ModelId)
        {
            var param = new SqlParameter("@ModelId", ModelId);

            var productDetails = await Task.Run(() => _dbContext.Models
                            .FromSqlRaw(@"exec GetModelByID @ModelId", param).ToListAsync());

            return productDetails;
        }

        public async Task<bool> UpdateModel(ModelEntity model)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@ModelId", model.ModelId));
            parameter.Add(new SqlParameter("@ModelName", model.ModelName));
            parameter.Add(new SqlParameter("@ModelCode", model.ModelCode));
            parameter.Add(new SqlParameter("@ModelDescription", model.ModelDescription));
            parameter.Add(new SqlParameter("@IsActive", model.IsActive));

            long result = await Task.Run(() => _dbContext.Database
               .ExecuteSqlRawAsync(@"UpdateModel @ModelId, @ModelName,@ModelCode, @ModelDescription", parameter.ToArray()));

            return result > 0;

        }
    }
}
