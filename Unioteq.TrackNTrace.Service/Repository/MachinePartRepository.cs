using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Models.Entity;

namespace Unioteq.TrackNTrace.Service.Repository
{
    public class MachinePartRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public MachinePartRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MachinePartEntity>> GetMachinePartsAsync()
        {
            var machineParts = new List<MachinePartEntity>();

            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetAllMachineParts";
                    command.CommandType = CommandType.StoredProcedure;

                    await _dbContext.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            machineParts.Add(new MachinePartEntity
                            {
                                MachinePartId = reader.GetInt64(reader.GetOrdinal("MachinePartId")),
                                MachinePartName = reader.GetString(reader.GetOrdinal("MachinePartName")),
                                MachinePartImage = reader.IsDBNull(reader.GetOrdinal("MachinePartImage")) ? null : reader.GetString(reader.GetOrdinal("MachinePartImage")),
                                MachineName = reader.GetString(reader.GetOrdinal("MachineName")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                MachineId = reader.GetInt64(reader.GetOrdinal("MachineId"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
                Console.WriteLine($"An error occurred while fetching machineParts: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
            return machineParts;
        }

        public async Task<MachinePartEntity> GetMachinePartByIdAsync(long id)
        {
            MachinePartEntity machinePart = null;

            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetMachinePartById";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@MachinePartId", id));

                    await _dbContext.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            machinePart = new MachinePartEntity
                            {
                                MachinePartId = reader.GetInt64(reader.GetOrdinal("MachinePartId")),
                                MachinePartName = reader.GetString(reader.GetOrdinal("MachinePartName")),
                                MachinePartImage = reader.IsDBNull(reader.GetOrdinal("MachinePartImage")) ? null : reader.GetString(reader.GetOrdinal("MachinePartImage")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                MachineId = reader.GetInt64(reader.GetOrdinal("MachineId"))
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
                Console.WriteLine($"An error occurred while fetching the machinePart: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
            return machinePart;
        }

        public async Task AddMachinePartAsync(MachinePartEntity machinePart)
        {
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "InsertMachinePart";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@MachinePartName", machinePart.MachinePartName));
                    command.Parameters.Add(new SqlParameter("@MachinePartImage", machinePart.MachinePartImage ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@IsActive", machinePart.IsActive));
                    command.Parameters.Add(new SqlParameter("@MachineId", machinePart.MachineId));

                    await _dbContext.Database.OpenConnectionAsync();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
                Console.WriteLine($"An error occurred while adding the machinePart: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }

        public async Task UpdateMachinePartAsync(MachinePartEntity machinePart)
        {
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "UpdateMachinePart";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@MachinePartId", machinePart.MachinePartId));
                    command.Parameters.Add(new SqlParameter("@MachinePartName", machinePart.MachinePartName));
                    command.Parameters.Add(new SqlParameter("@MachinePartImage", machinePart.MachinePartImage ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@IsActive", machinePart.IsActive));
                    command.Parameters.Add(new SqlParameter("@MachineId", machinePart.MachineId));

                    await _dbContext.Database.OpenConnectionAsync();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
                Console.WriteLine($"An error occurred while updating the machinePart: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }


        public async Task DeleteMachinePartAsync(long id)
        {
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "DeleteMachinePart";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@MachinePartId", id));

                    await _dbContext.Database.OpenConnectionAsync();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
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
