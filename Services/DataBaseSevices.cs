using Dapper;
using DynamicGridGeneration.Data;
using DynamicGridGeneration.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DynamicGridGeneration.Services
{
    public class DataBaseServices
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        //= "Server=SSSLBG4NB1027\\SQLEXPRESS;Database=DynamicGridDb;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=true"

        public DataBaseServices(IConfiguration configuration)
        {
           _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<string>> GetAllVisibleColumns()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT ColumnName FROM ColumnVisibility WHERE IsVisible = 1";
                //var data = (await connection.QueryAsync<dynamic>(dataQuery)).ToList();
                var data = await connection.QueryAsync<string>(sqlQuery);
                return data.ToList();
            }
        }

        // Executes the ALTER TABLE SQL query to add a new column
        public async Task ExecuteAlterTableQueryAsync(string tableName, string columnName, string dataType)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Execute the ALTER TABLE query
                var sqlQuery = $"ALTER TABLE [{tableName}] ADD [{columnName}] {dataType}";
                await connection.ExecuteAsync(sqlQuery);

                // Insert the column visibility record
                var insertQuery = @"
            INSERT INTO ColumnVisibility (TableName, ColumnName, IsVisible)
            VALUES (@TableName, @ColumnName, @IsVisible)";

                var columnVisibility = new
                {
                    TableName = tableName,
                    ColumnName = columnName,
                    IsVisible = true
                };

                await connection.ExecuteAsync(insertQuery, columnVisibility);
            }
        }


        public async Task<bool> ColumnExistsAsync(string tableName, string columnName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Use UPPER for case-insensitive comparison
                var query = @"
            SELECT COUNT(*) 
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER(@tableName) 
              AND UPPER(COLUMN_NAME) = UPPER(@columnName)";

                var exists = await connection.ExecuteScalarAsync<int>(query, new { tableName, columnName });
                return exists > 0;
            }
        }



        // Fetches employees data and column names dynamically
        public async Task<(List<string>, List<dynamic>)> GetEmployeesAsync()
        {
            //Console.WriteLine(myTableName);
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                //Fetching column names
                var columnsQuery = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Employees'";
                Console.WriteLine(columnsQuery);
                var columns = (await connection.QueryAsync<string>(columnsQuery)).ToList();


                // Fetching employee data (rows)
                var dataQuery = $"SELECT * FROM Employees";
                var data = (await connection.QueryAsync<dynamic>(dataQuery)).ToList();

                return (columns, data);
            }
        }

        public async Task<List<ColumnVisibility>> GetColumnVisibilityAsync(string tableName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM ColumnVisibility WHERE TableName = @tableName";
                var columns = (await connection.QueryAsync<ColumnVisibility>(query, new { tableName })).ToList();
                return columns;
            }
        }

        public async Task UpdateColumnVisibilityAsync(ColumnVisibility column)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "UPDATE ColumnVisibility SET IsVisible = @IsVisible WHERE TableName = @TableName AND ColumnName = @ColumnName";
                await connection.ExecuteAsync(query, new { column.IsVisible, column.TableName, column.ColumnName });
            }
        }

        public async Task InsertEmployeeAsync(Dictionary<string, object> employeeData)
        {
            var columnNames = string.Join(", ", employeeData.Keys);
            var paramNames = string.Join(", ", employeeData.Keys.Select(key => $"@{key}"));
            var sql = $"INSERT INTO Employees ({columnNames}) VALUES ({paramNames})";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, employeeData);
            }
        }


        public async Task UpdateEmployeeAsync(Dictionary<string, object> updatedData)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE Employees SET ";

            // Dynamically build the SET clause based on dictionary keys
            var setClause = string.Join(", ", updatedData.Keys.Where(key => key != "Id").Select(key => $"{key} = @{key}"));
            sql += setClause + " WHERE Id = @Id";

            // Execute the query with Dapper
            await connection.ExecuteAsync(sql, updatedData);
        }

        public async Task DeleteEmployeeAsync(string employeeId)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "DELETE FROM Employees WHERE Id = @Id";
            await connection.ExecuteAsync(query, new { Id = employeeId });
        }

    }
}