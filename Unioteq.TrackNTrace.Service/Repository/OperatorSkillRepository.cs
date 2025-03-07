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
    public class OperatorSkillRepository
    {
        private readonly ApplicationDBContext _dbContext;


        public OperatorSkillRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<OperatorSkillEntity>> GetOperatorSkillList()
        {
            try
            {

                var OperatorSkill = await _dbContext.OperatorSkills
                    .FromSqlRaw("EXEC GetOperatorSkillList") // Assuming sp_SelectPlants is the name of your stored procedure
                    .ToListAsync();



                return OperatorSkill;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred while fetching OperatorSkills: {ex.Message}");
                throw; // Rethrow the exception to propagate it upwards
            }
        }

        public async Task<bool> AddOperatorSkill(OperatorSkillEntity OperatorSkill)
        {

            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@OperatorSkillName", OperatorSkill.OperatorSkillName));
            parameter.Add(new SqlParameter("@IsActive", OperatorSkill.IsActive));

            int result = await Task.Run(() => _dbContext.Database
               .ExecuteSqlRawAsync(@"InsertOperatorSkill @OperatorSkillName,@IsActive", parameter.ToArray()));

            return result > 0;


        }

        public async Task<long> DeleteOperatorSkill(long OperatorSkillId)
        {
            var sql = $"EXEC DeleteOperatorSkill @OperatorSkillId";
            var parameters = new object[] { OperatorSkillId };

            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"DeleteOperatorSkill {OperatorSkillId}"));
        }

        public async Task<IEnumerable<OperatorSkillEntity>> GetOperatorSkillById(long OperatorSkillId)
        {
            var param = new SqlParameter("@OperatorSkillId", OperatorSkillId);

            var productDetails = await Task.Run(() => _dbContext.OperatorSkills
                            .FromSqlRaw(@"exec GetOperatorSkillByID @OperatorSkillId", param).ToListAsync());

            return productDetails;
        }

        public async Task<bool> UpdateOperatorSkill(OperatorSkillEntity OperatorSkill)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@OperatorSkillId", OperatorSkill.OperatorSkillId));
            parameter.Add(new SqlParameter("@OperatorSkillName", OperatorSkill.OperatorSkillName));
            parameter.Add(new SqlParameter("@IsActive", OperatorSkill.IsActive));
            //parameter.Add(new SqlParameter("@PlantId", plant.PlantId));
            // parameter.Add(new SqlParameter("@UserId", currentId));


            long result = await Task.Run(() => _dbContext.Database
               .ExecuteSqlRawAsync(@"UpdateOperatorSkill @OperatorSkillId, @OperatorSkillName, @IsActive", parameter.ToArray()));

            return result > 0;

        }
    }
}
