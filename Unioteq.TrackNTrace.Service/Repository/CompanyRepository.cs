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
    public class CompanyRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CompanyRepository (ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CompanyEntity>> Get()
        {
            var company = new List<CompanyEntity>();

            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetCompanyList";
                    command.CommandType = CommandType.StoredProcedure;

                    await _dbContext.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            company.Add(new CompanyEntity
                            {
                                CompId = reader.GetInt64(reader.GetOrdinal("CompId")),
                                CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                CompLogo = reader.IsDBNull(reader.GetOrdinal("CompLogo")) ? null : reader.GetString(reader.GetOrdinal("CompLogo")),
                                CompCorpAdd = reader.GetString(reader.GetOrdinal("CompCorpAdd")),
                                CompClientAdd = reader.GetString(reader.GetOrdinal("CompClientAdd")),
                                CompCode = reader.GetString(reader.GetOrdinal("CompCode")),

                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
                Console.WriteLine($"An error occurred while fetching Company: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
            return company;
        }



        public async  Task<CompanyEntity> GetCompanyById(long id)
        {
            CompanyEntity company = null;
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetCompanyById";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CompId", id));

                    await _dbContext.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            company = new CompanyEntity
                            {
                                CompId = reader.GetInt64(reader.GetOrdinal("CompId")),
                                CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                CompLogo = reader.IsDBNull(reader.GetOrdinal("CompLogo")) ? null : reader.GetString(reader.GetOrdinal("CompLogo")),
                                CompCorpAdd = reader.GetString(reader.GetOrdinal("CompCorpAdd")),
                                CompClientAdd = reader.GetString(reader.GetOrdinal("CompClientAdd")),
                                CompCode = reader.GetString(reader.GetOrdinal("CompCode")),




                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                               
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
                Console.WriteLine($"An error occurred while fetching the company: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
            return company;
        }





        public async Task InsertCompany(CompanyEntity company)
        {
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "InsertCompany";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@CompanyName", company.CompanyName));
                    command.Parameters.Add(new SqlParameter("@CompLogo", company.CompLogo ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@CompCorpAdd", company.CompCorpAdd));
                    command.Parameters.Add(new SqlParameter("@CompClientAdd", company.CompClientAdd));
                    command.Parameters.Add(new SqlParameter("@CompCode", company.CompCode));

                    command.Parameters.Add(new SqlParameter("@IsActive", company.IsActive));

                    await _dbContext.Database.OpenConnectionAsync();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
                Console.WriteLine($"An error occurred while adding the company: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }




        public async Task UpdateCompany(CompanyEntity company)
        {
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "UpdateCompany";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@CompId", company.CompId));
                    command.Parameters.Add(new SqlParameter("@CompanyName", company.CompanyName));
                    command.Parameters.Add(new SqlParameter("@CompLogo", company.CompLogo ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@CompCorpAdd", company.CompCorpAdd));
                    command.Parameters.Add(new SqlParameter("@CompClientAdd", company.CompClientAdd));
                    command.Parameters.Add(new SqlParameter("@CompCode", company.CompCode));

                    command.Parameters.Add(new SqlParameter("@IsActive", company.IsActive));

                    await _dbContext.Database.OpenConnectionAsync();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
                Console.WriteLine($"An error occurred while updating the Company: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }











        public async Task DeleteCompany(long id)
        {
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = " DeleteCompany";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CompId", id));

                    await _dbContext.Database.OpenConnectionAsync();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the machinePart: {ex.Message}");
               throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }

        }





    }
}
