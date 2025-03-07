using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;

namespace Unioteq.TrackNTrace.Service.Repository
{
    public class DepartmentRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public DepartmentRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DepartmentEntity>> Get()
        {
            try
            {
                var Department = await _dbContext.Departments
                .FromSqlRaw("EXEC GetDepartmentList")
                .ToListAsync();


                return Department;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occur:{ex.Message}");
                throw;
            }
        }
        public async Task<bool> AddDepartment(DepartmentEntity departments)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@DepartmentName",departments.DepartmentName));
            parameter.Add(new SqlParameter("@DepartmentCode",departments.DepartmentCode));
            parameter.Add(new SqlParameter("@DepartmentDescription",departments.DepartmentDescription));
            parameter.Add(new SqlParameter("@IsActive",departments.IsActive));

            int result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"InsertDepartment @DepartmentName, @DepartmentCode, @DepartmentDescription,@IsActive", parameter.ToArray()));
            return result > 0;
        }

        public async Task<long> DeleteDepartment(long departmentId)
        {
            var sql = $"EXEC DeleteDepartment @DepartmentId";
            var parameters = new object[] { departmentId };

            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"DeleteDepartment {departmentId}"));
        }

        public async Task<IEnumerable<DepartmentEntity>> GetProductById(long departmentId)
        {
            var param = new SqlParameter("@DepartmentId", departmentId);

            var productDetails = await Task.Run(() => _dbContext.Departments
                            .FromSqlRaw(@"exec GetDepartmentByID @DepartmentId", param).ToListAsync());

            return (IEnumerable<DepartmentEntity>)productDetails;
        }

        public async Task<bool> UpdateDepartment(DepartmentEntity department)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@DepartmentId", department.DepartmentId));
            parameter.Add(new SqlParameter("@DepartmentName", department.DepartmentName));
            parameter.Add(new SqlParameter("@DepartmentCode", department.DepartmentCode));
            parameter.Add(new SqlParameter("@DepartmentDescription", department.DepartmentDescription));
            parameter.Add(new SqlParameter("@IsActive", department.IsActive));



            long result = await Task.Run(() => _dbContext.Database
               .ExecuteSqlRawAsync(@"UpdateDepartment @DepartmentId, @DepartmentName,@DepartmentCode, @DepartmentDescription,@IsActive", parameter.ToArray()));

            return result > 0;

        }
    }
}
