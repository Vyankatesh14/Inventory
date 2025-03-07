using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;

namespace Unioteq.TrackNTrace.Service.Repository
{
    public class ModelPartsRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public ModelPartsRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ModelPartsEntity>> GetAllModelParts()
        {
            try
            {
                return await _dbContext.ModelParts
                    .FromSqlRaw("EXEC GetModelPartList") // Stored procedure name
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching ModelParts: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> AddModelParts(ModelPartsEntity ModelParts)
        {
            var parameters = new List<SqlParameter>
            {
    
        new SqlParameter("@StationId", SqlDbType.BigInt) { Value = ModelParts.StationId },
        new SqlParameter("@ModelId", SqlDbType.BigInt) { Value = ModelParts.ModelId },
        new SqlParameter("@ModelPartName",  SqlDbType.NVarChar,100) { Value = ModelParts.ModelPartName},
        new SqlParameter("@ModelPartDescription", SqlDbType.NVarChar, 100) { Value = ModelParts.ModelPartDescription },
        new SqlParameter("@Sequence", SqlDbType.Int) { Value = ModelParts.Sequence },
        new SqlParameter("@ModelPartCode", SqlDbType.NVarChar,100) { Value = ModelParts.ModelPartCode },
        new SqlParameter("@ModelPartImage",  SqlDbType.NVarChar,100) { Value = ModelParts.ModelPartImage },
        new SqlParameter("@LineId", SqlDbType.BigInt) { Value = ModelParts.LineId },
        new SqlParameter("@PlantId", SqlDbType.BigInt) { Value = ModelParts.PlantId },
        new SqlParameter("@ShopFloorId", SqlDbType.BigInt) { Value = ModelParts.ShopFloorId },
        new SqlParameter("@IsActive", SqlDbType.Bit) { Value = ModelParts.IsActive }  
    };
            

            try
            {
                int result = await _dbContext.Database
                    .ExecuteSqlRawAsync("EXEC InsertModelParts  @StationId,@ModelId,@ModelPartName,@ModelPartDescription,@Sequence,@ModelPartCode,@ModelPartImage,@LineId,@PlantId,@ShopFloorId,@IsActive", parameters.ToArray());
                
                 return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding ModelPart: {ex.Message}");
                throw;
            }
        }

        public async Task<ModelPartsEntity> GetModelPartById(long id)
        {
            try
            {
                var parameter = new SqlParameter("@ModelPartId", id);
                var modelparts = await _dbContext.ModelParts
                    .FromSqlRaw("EXEC GetModelPartById @ModelPartId", parameter)
                    .ToListAsync(); // Fetch the results asynchronously as a list

                return modelparts.FirstOrDefault(); // Return the first or default item
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching modelpart by ID: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateCheckPoint(ModelPartsEntity ModelParts)
        {
            var parameters = new List<SqlParameter>
    {

                new SqlParameter("@ModelPartId", ModelParts.ModelPartId),
                new SqlParameter("@StationId", ModelParts.StationId),
                new SqlParameter("@StationCode", ModelParts.StationCode),
                new SqlParameter("@ModelId", ModelParts.ModelId),
                new SqlParameter("@ModelPartName", ModelParts.ModelPartName),
                new SqlParameter("@ModelCode", ModelParts.ModelCode),
                new SqlParameter("@ModelPartDescription", ModelParts.ModelPartDescription),
                new SqlParameter("@Sequence", ModelParts.Sequence),
                new SqlParameter("@ModelPartCode", ModelParts.ModelPartCode),
                new SqlParameter("@ModelPartImage", ModelParts.ModelPartImage),
                new SqlParameter("@LineId", ModelParts.LineId),
                new SqlParameter("@LineCode", ModelParts.LineCode),
                new SqlParameter("@PlantId", ModelParts.PlantId),
               new SqlParameter("@PlantCode", ModelParts.PlantCode),
                new SqlParameter("@ShopFloorId", ModelParts.ShopFloorId),
              new SqlParameter("@ShopFloorCode", ModelParts.ShopFloorCode),
                new SqlParameter("@IsActive", ModelParts.IsActive)
    };

            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateModelPart @ModelPartId, @StationId,@ModelId,@ModelPartName,@ModelPartDescription,@Sequence,@ModelPartCode,@ModelPartImage,@LineId,@PlantId,@ShopFloorId, @IsActive",
                    parameters.ToArray());
                return true; // Or any logic you want to return based on the operation
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating modelpart: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteModelPart(long id)
        {
            try
            {
                var parameter = new SqlParameter("@ModelPartId", id);
                int result = await _dbContext.Database
                    .ExecuteSqlRawAsync("EXEC DeleteModelPart @ModelPartId", parameter);

                return result > 0;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred while deleting ModelParts: {ex.Message}");

                return false;
            }
        }


    }
}
