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
    public class OperationRepository
    {
        private readonly ApplicationDBContext _dbContext;


        public OperationRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<OperationsEntity>> GetOperationList()
        {
            try
            {

                var Operation = await _dbContext.Operations
                    .FromSqlRaw("EXEC GetOperationList") // Assuming sp_SelectPlants is the name of your stored procedure
                    .ToListAsync();



                return Operation;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred while fetching Operations: {ex.Message}");
                throw; // Rethrow the exception to propagate it upwards
            }
        }

        public async Task<bool> AddOperation(OperationsEntity Operation)
        {

            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@Operation", Operation.Operation ?? (object)DBNull.Value));
            parameter.Add(new SqlParameter("@PlantId", Operation.PlantId));
            parameter.Add(new SqlParameter("@ShopFloorId", Operation.ShopFloorId));
            parameter.Add(new SqlParameter("@LineId", Operation.LineId));
            parameter.Add(new SqlParameter("@StationId", Operation.StationId));
            parameter.Add(new SqlParameter("@ModelId", Operation.ModelId));
            parameter.Add(new SqlParameter("@Sequence", Operation.Sequence));
            parameter.Add(new SqlParameter("@IsActive", Operation.IsActive));

            int result = await Task.Run(() => _dbContext.Database
               .ExecuteSqlRawAsync(@"InsertOperation @Operation,@PlantId,@ShopFloorId,@LineId,@StationId,@ModelId,@Sequence,@IsActive", parameter.ToArray()));

            return result > 0;


        }
        public async Task<bool> DeleteOperation(long OperationId)
        {
            var parameter = new SqlParameter("@OperationId", OperationId);

            try
            {
                // Execute the stored procedure to soft delete the user
                var result = await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC DeleteOperation @OperationId", parameter);

                return result > 0; // Return true if the update was successful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return false;
            }
        }
        /* public async Task<long> DeleteOperation(long OperationId)
         {
             var sql = $"EXEC DeleteOperation @OperationId";
             var parameters = new object[] { OperationId };

             return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"DeleteOperation {OperationId}"));
         }*/

        public async Task<IEnumerable<OperationsEntity>> GetOperationById(long OperationId)
        {
            var param = new SqlParameter("@OperationId", OperationId);

            var productDetails = await Task.Run(() => _dbContext.Operations
                            .FromSqlRaw(@"exec GetOperationById @OperationId", param).ToListAsync());

            return productDetails;
        }

        public async Task<bool> UpdateOperation(OperationsEntity operation)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@OperationId", SqlDbType.BigInt) { Value = operation.OperationId },
        new SqlParameter("@Operation", SqlDbType.NVarChar, 100) { Value = operation.Operation ?? (object)DBNull.Value },
        new SqlParameter("@Sequence", SqlDbType.Int) { Value = operation.Sequence },
        new SqlParameter("@IsActive", SqlDbType.Bit) { Value = operation.IsActive },
        new SqlParameter("@ModelId", SqlDbType.BigInt) { Value = operation.ModelId },
        new SqlParameter("@PlantId", SqlDbType.BigInt) { Value = operation.PlantId },
        new SqlParameter("@ShopFloorId", SqlDbType.BigInt) { Value = operation.ShopFloorId },
        new SqlParameter("@StationId", SqlDbType.BigInt) { Value = operation.StationId },
        new SqlParameter("@LineId", SqlDbType.BigInt) { Value = operation.LineId }
       
    };

            int result = await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC UpdateOperation @OperationId, @Operation, @Sequence, @IsActive, @ModelId, @PlantId, @ShopFloorId, @StationId, @LineId",
                parameters.ToArray());

            return result > 0;
        }

    }
}
