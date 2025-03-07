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
   public class DeviceMasterRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public DeviceMasterRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DeviceMasterEntity>> Get()
        {
            try
            {
                var devicemaster = await _dbContext.Devicemaster.FromSqlRaw("EXEC GetDeviceMasterList").ToListAsync();
                return devicemaster;

            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching DeviceMaster: {ex.Message}");
                throw;
            }
        }








        public async Task<IEnumerable<DeviceMasterEntity>> GetDeviceMasterById(long Id)
        {
            try
            {
                var param = new SqlParameter("@Id", Id); // Use correct parameter name

                var DeviceMasterById = await _dbContext.Devicemaster
                    .FromSqlRaw("EXEC GetDeviceMasterById @Id", param) // Use EXEC with correct parameter
                    .ToListAsync();

                return DeviceMasterById;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching the DeviceMaster by ID: {ex.Message}");
                throw;
            }
        }









        public async Task<bool> AddDeviceMaster(DeviceMasterEntity devicemaster)
        {
            var parameter = new List<SqlParameter>
    {
        new SqlParameter("@CompId", devicemaster.CompId),
        new SqlParameter("@PlantId", devicemaster.PlantId),
        new SqlParameter("@LineId", devicemaster.LineId),
        new SqlParameter("@MacCode", devicemaster.MacCode),
        new SqlParameter("@ActivationDate", devicemaster.ActivationDate),
        new SqlParameter("@ExpiryDate", devicemaster.ExpiryDate),
        new SqlParameter("@DeviceCode", devicemaster.DeviceCode),
        // Handle null for IsActive
        new SqlParameter("@IsActive", devicemaster.IsActive ?? (object)DBNull.Value)
    };

            try
            {
                int result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"InsertDeviceMaster @CompId, @PlantId, @LineId, @MacCode, @ActivationDate, @ExpiryDate, @DeviceCode, @IsActive", parameter.ToArray()));

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the DeviceMaster: {ex.Message}");
                throw;
            }
        }





        public async Task<long> DeleteDeviceMaster(long deviceId)
        {
            try
            {
                return await Task.Run(() => _dbContext.Database
                    .ExecuteSqlInterpolatedAsync($"DeleteDeviceMaster {deviceId}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the DeviceMaster: {ex.Message}");
                throw;
            }
        }






        public async Task<bool> UpdateDeviceMaster(DeviceMasterEntity devicemaster)
        {
            var parameter = new List<SqlParameter>
    {
        new SqlParameter("@Id", devicemaster.Id),
        new SqlParameter("@CompId", devicemaster.CompId),
        new SqlParameter("@PlantId", devicemaster.PlantId),
        new SqlParameter("@LineId", devicemaster.LineId),
        new SqlParameter("@MacCode", devicemaster.MacCode),
        new SqlParameter("@ActivationDate", devicemaster.ActivationDate),
        new SqlParameter("@ExpiryDate", devicemaster.ExpiryDate),
        new SqlParameter("@DeviceCode", devicemaster.DeviceCode),
        new SqlParameter("@IsActive", devicemaster.IsActive ?? (object)DBNull.Value),
    };

            try
            {
                long result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"UpdateDeviceMaster @Id, @CompId, @PlantId, @LineId, @MacCode, @ActivationDate, @ExpiryDate, @DeviceCode, @IsActive", parameter.ToArray()));

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the DeviceMaster: {ex.Message}");
                throw;
            }
        }






    }
}
