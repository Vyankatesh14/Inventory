using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unioteq.TrackNTrace.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Unioteq.TrackNTrace.Models;

namespace Unioteq.TrackNTrace.Service.Repository
{
    public class MachineCheckPointRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public MachineCheckPointRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MachineCheckPointEntity>> GetAllCheckPoints()
        {
            try
            {
                return await _dbContext.MachineCheckPoints
                    .FromSqlRaw("EXEC GetMachineCheckPointList") // Stored procedure name
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching checkpoints: {ex.Message}");
                throw;
            }
        }

            
        public async Task<bool> AddCheckPoint(MachineCheckPointEntity checkPoint)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@CheckPointName", checkPoint.CheckPointName ?? (object)DBNull.Value),
                new SqlParameter("@Effect", checkPoint.Effect ?? (object)DBNull.Value),
                new SqlParameter("@ValidationType", checkPoint.ValidationType ?? (object)DBNull.Value),
                new SqlParameter("@StandardText", checkPoint.StandardText ?? (object)DBNull.Value),
                new SqlParameter("@OKText", checkPoint.OKText ?? (object)DBNull.Value),
                new SqlParameter("@NOKText", checkPoint.NOKText ?? (object)DBNull.Value),
                new SqlParameter("@CheckMethod", checkPoint.CheckMethod ?? (object)DBNull.Value),
                new SqlParameter("@Frequecy", checkPoint.Frequecy ?? (object)DBNull.Value),
                new SqlParameter("@MachineId", checkPoint.MachineId),
                new SqlParameter("@MachinePartId", checkPoint.MachinePartId),
                new SqlParameter("@IsActive", checkPoint.IsActive)
            };

            try
            {
                int result = await _dbContext.Database
                    .ExecuteSqlRawAsync("EXEC InsertMachineCheckPoint @CheckPointName, @Effect, @ValidationType, @StandardText, @OKText, @NOKText, @CheckMethod, @Frequecy, @MachineId, @MachinePartId, @IsActive", parameters.ToArray());

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding checkpoint: {ex.Message}");
                throw;
            }
        }


        public async Task<MachineCheckPointEntity?> GetCheckPointById(long id)
        {
            try
            {
                var parameter = new SqlParameter("@MachineCheckPointId", id);
                var checkpoints = await _dbContext.MachineCheckPoints
                    .FromSqlRaw("EXEC GetMachineCheckPointById @MachineCheckPointId", parameter)
                    .ToListAsync(); // Fetch the results asynchronously as a list

                return checkpoints.FirstOrDefault(); // Return the first or default item
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching checkpoint by ID: {ex.Message}");
                throw;
            }
        }





        public async Task<bool> UpdateCheckPoint(MachineCheckPointEntity checkPoint)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@MachineCheckPointId", checkPoint.MachineCheckPointId),
        new SqlParameter("@Effect", (object)checkPoint.Effect ?? DBNull.Value),
        new SqlParameter("@CheckPointName", (object)checkPoint.CheckPointName ?? DBNull.Value),
        new SqlParameter("@ValidationType", (object)checkPoint.ValidationType ?? DBNull.Value),
        new SqlParameter("@StandardText", (object)checkPoint.StandardText ?? DBNull.Value),
        new SqlParameter("@OKText", (object)checkPoint.OKText ?? DBNull.Value),
        new SqlParameter("@NOKText", (object)checkPoint.NOKText ?? DBNull.Value),
        new SqlParameter("@CheckMethod", (object)checkPoint.CheckMethod ?? DBNull.Value),
        new SqlParameter("@Frequecy", (object)checkPoint.Frequecy ?? DBNull.Value),
        new SqlParameter("@MachineId", checkPoint.MachineId),
        new SqlParameter("@MachinePartId", checkPoint.MachinePartId),
        new SqlParameter("@IsActive", checkPoint.IsActive),
    };

            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateMachineCheckPoint @MachineCheckPointId, @Effect, @CheckPointName, @ValidationType, @StandardText, @OKText, @NOKText, @CheckMethod, @Frequecy, @MachineId, @MachinePartId, @IsActive",
                    parameters.ToArray());
                return true; // Or any logic you want to return based on the operation
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating checkpoint: {ex.Message}");
                return false;
            }
        }





        public async Task<bool> DeleteCheckPoint(long id)
        {
            try
            {
                var parameter = new SqlParameter("@MachineCheckPointId", id);
                int result = await _dbContext.Database
                    .ExecuteSqlRawAsync("EXEC DeleteMachineCheckPoint @MachineCheckPointId", parameter);

                return result > 0;
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"An error occurred while deleting checkpoint: {ex.Message}");
              
                return false;
            }
        }

    }
}
