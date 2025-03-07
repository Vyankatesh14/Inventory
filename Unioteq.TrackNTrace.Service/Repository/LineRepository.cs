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
    public class LineRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public LineRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get all lines
        public async Task<List<LineEntity>> GetLinesAsync()
        {
            try
            {
                var lines = new List<LineEntity>();

                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetAllLines";
                    command.CommandType = CommandType.StoredProcedure;

                    _dbContext.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lines.Add(new LineEntity
                            {
                                LineId = reader.GetInt64(reader.GetOrdinal("LineId")),
                                LineCode = reader.GetString(reader.GetOrdinal("LineCode")),
                                LineName = reader.GetString(reader.GetOrdinal("LineName")),
                                LineDescription = reader.IsDBNull(reader.GetOrdinal("LineDescription")) ? null : reader.GetString(reader.GetOrdinal("LineDescription")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                PlantId = reader.GetInt64(reader.GetOrdinal("PlantId")),
                                ShopFloorId = reader.GetInt64(reader.GetOrdinal("ShopFloorId")),
                                ShopFloorName = reader.GetString(reader.GetOrdinal("ShopFloorName")),
                                PlantName = reader.GetString(reader.GetOrdinal("PlantName"))
                            });
                        }
                    }
                }

                return lines;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching lines: {ex.Message}");
                throw;
            }
        }

        // Get a line by ID
        public async Task<LineEntity> GetLineByIdAsync(long id)
        {
            try
            {
                LineEntity line = null;

                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetLineById";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@LineId", id));

                    _dbContext.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            line = new LineEntity
                            {
                                LineId = reader.GetInt64(reader.GetOrdinal("LineId")),
                                LineCode = reader.GetString(reader.GetOrdinal("LineCode")),
                                LineName = reader.GetString(reader.GetOrdinal("LineName")),
                                LineDescription = reader.IsDBNull(reader.GetOrdinal("LineDescription")) ? null : reader.GetString(reader.GetOrdinal("LineDescription")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                PlantId = reader.GetInt64(reader.GetOrdinal("PlantId")),
                                ShopFloorId = reader.GetInt64(reader.GetOrdinal("ShopFloorId"))
                            };
                        }
                    }
                }

                return line;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching the line: {ex.Message}");
                throw;
            }
        }

        // Add a new line
        public async Task AddLineAsync(LineEntity line)
        {
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "InsertLine";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@LineName", line.LineName));
                    command.Parameters.Add(new SqlParameter("@LineCode", line.LineCode));
                    command.Parameters.Add(new SqlParameter("@ShopFloorId", line.ShopFloorId));
                    command.Parameters.Add(new SqlParameter("@PlantId", line.PlantId));
                    command.Parameters.Add(new SqlParameter("@LineDescription", line.LineDescription ?? (object)DBNull.Value));
                   
                    _dbContext.Database.OpenConnection();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the line: {ex.Message}");
                throw;
            }
        }

        // Update an existing line
        public async Task UpdateLineAsync(LineEntity line)
        {
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "UpdateLine";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@LineId", line.LineId));
                    command.Parameters.Add(new SqlParameter("@LineName", line.LineName));
                    command.Parameters.Add(new SqlParameter("@LineCode", line.LineCode));
                    command.Parameters.Add(new SqlParameter("@ShopFloorId", line.ShopFloorId));
                    command.Parameters.Add(new SqlParameter("@PlantId", line.PlantId));
                    command.Parameters.Add(new SqlParameter("@LineDescription", line.LineDescription ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@IsActive", line.IsActive));
                    
                    _dbContext.Database.OpenConnection();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the line: {ex.Message}");
                throw;
            }
        }

        // Delete a line
        public async Task DeleteLineAsync(long id)
        {
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "DeleteLine";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@LineId", id));

                    _dbContext.Database.OpenConnection();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the line: {ex.Message}");
                throw;
            }
        }
    }
}
