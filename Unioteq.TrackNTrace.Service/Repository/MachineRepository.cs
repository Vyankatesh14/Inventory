using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Unioteq.TrackNTrace.Models.Entity;
using Unioteq.TrackNTrace.Models;

namespace Unioteq.TrackNTrace.Service.Repository
{
    public class MachineRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public MachineRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MachineEntity>> GetMachinesAsync()
        {
            var machines = new List<MachineEntity>();

            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetAllMachines"; // This stored procedure should return LineName
                    command.CommandType = CommandType.StoredProcedure;

                    await _dbContext.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            machines.Add(new MachineEntity
                            {
                                MachineId = reader.GetInt64(reader.GetOrdinal("MachineId")),
                                MachineName = reader.GetString(reader.GetOrdinal("MachineName")),
                                MachineCode = reader.GetString(reader.GetOrdinal("MachineCode")),
                                MachineDetail = reader.GetString(reader.GetOrdinal("MachineDetail")),
                                MachineManual = reader.IsDBNull(reader.GetOrdinal("MachineManual")) ? null : reader.GetString(reader.GetOrdinal("MachineManual")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                LineId = reader.GetInt64(reader.GetOrdinal("LineId")),
                                LineName = reader.GetString(reader.GetOrdinal("LineName")) // Ensure LineName is fetched
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching machines: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }

            return machines;
        }

        public async Task<MachineEntity> GetMachineByIdAsync(long id)
        {
            MachineEntity machine = null;

            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetMachinesById";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@MachineId", id));

                    await _dbContext.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            machine = new MachineEntity
                            {
                                MachineId = reader.GetInt64(reader.GetOrdinal("MachineId")),
                                MachineName = reader.GetString(reader.GetOrdinal("MachineName")),
                                MachineCode = reader.GetString(reader.GetOrdinal("MachineCode")),
                                MachineDetail = reader.GetString(reader.GetOrdinal("MachineDetail")),
                                MachineManual = reader.IsDBNull(reader.GetOrdinal("MachineManual")) ? null : reader.GetString(reader.GetOrdinal("MachineManual")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                LineId = reader.GetInt64(reader.GetOrdinal("LineId")),
                                LineName = reader.GetString(reader.GetOrdinal("LineName"))

                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
                Console.WriteLine($"An error occurred while fetching the machine: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }

            return machine;
        }

        public async Task AddMachineAsync(MachineEntity machine)
        {
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "InsertMachine";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@MachineName", machine.MachineName));
                    command.Parameters.Add(new SqlParameter("@MachineCode", machine.MachineCode));
                    command.Parameters.Add(new SqlParameter("@MachineDetail", machine.MachineDetail));
                    command.Parameters.Add(new SqlParameter("@MachineManual", machine.MachineManual ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@IsActive", machine.IsActive));
                    command.Parameters.Add(new SqlParameter("@LineId", machine.LineId));

                    await _dbContext.Database.OpenConnectionAsync();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
                Console.WriteLine($"An error occurred while adding the machine: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }

        public async Task UpdateMachineAsync(MachineEntity machine)
        {
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "UpdateMachine";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@MachineId", machine.MachineId));
                    command.Parameters.Add(new SqlParameter("@MachineName", machine.MachineName));
                    command.Parameters.Add(new SqlParameter("@MachineCode", machine.MachineCode));
                    command.Parameters.Add(new SqlParameter("@MachineDetail", machine.MachineDetail));
                    command.Parameters.Add(new SqlParameter("@MachineManual", machine.MachineManual ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@IsActive", machine.IsActive));
                    command.Parameters.Add(new SqlParameter("@LineId", machine.LineId));

                    await _dbContext.Database.OpenConnectionAsync();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
                Console.WriteLine($"An error occurred while updating the machine: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }

        public async Task DeleteMachineAsync(long id)
        {
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "DeleteMachine";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@MachineId", id));

                    await _dbContext.Database.OpenConnectionAsync();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Consider using a logging framework
                Console.WriteLine($"An error occurred while deleting the machine: {ex.Message}");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }
    }
}
