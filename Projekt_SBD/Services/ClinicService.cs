using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Projekt_SBD.Data;

namespace Projekt_SBD.Services
{
    public class DoctorAvailabilityDto
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class MonthlyCostDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalSalariesCost { get; set; }
    }

    public class SupplyUsageDto
    {
        public string Name { get; set; }
        public int TotalUsed { get; set; }
    }

    public class DoctorAvailabilityViewDto
    {
        public int DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class PatientMedicalHistoryViewDto
    {
        public int PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateTime VisitDate { get; set; }
        public string Symptoms { get; set; }
        public string Illness { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public int RoomId { get; set; }
    }

    public class LowStockSupplyViewDto
    {
        public int SupplyId { get; set; }
        public string SupplyName { get; set; }
        public int Quantity { get; set; }
        public int RoomNumber { get; set; }
        public string RoomName { get; set; }
    }

    public class ClinicService
    {
        private readonly PrzychodniaContext _context;

        public ClinicService(PrzychodniaContext context)
        {
            _context = context;
        }

        // PKG_VISITS

        public async Task ScheduleVisitAsync(int patientId, int workerId, int roomId, DateTime startTime, DateTime endTime, string purpose, int cost)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "BEGIN ADMINISTRATOR.PKG_VISITS.ScheduleVisit(:p0, :p1, :p2, :p3, :p4, :p5, :p6); END;",
                patientId, workerId, roomId, startTime, endTime, purpose, cost
            );
        }

        public async Task CancelVisitAsync(int visitId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "BEGIN ADMINISTRATOR.PKG_VISITS.CancelVisit(:p0); END;",
                visitId
            );
        }

        public async Task<List<DoctorAvailabilityDto>> CheckDoctorAvailabilityAsync(int workerId, DateTime date)
        {
            var result = new List<DoctorAvailabilityDto>();

            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "BEGIN :ret := ADMINISTRATOR.PKG_VISITS.CheckDoctorAvailability(:p0, :p1); END;";
            
            var retParam = new OracleParameter("ret", OracleDbType.RefCursor, ParameterDirection.Output);
            command.Parameters.Add(retParam);
            command.Parameters.Add(new OracleParameter("p0", workerId));
            command.Parameters.Add(new OracleParameter("p1", date));

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new DoctorAvailabilityDto
                {
                    Start = reader.GetDateTime(reader.GetOrdinal("Start")),
                    End = reader.GetDateTime(reader.GetOrdinal("End"))
                });
            }

            return result;
        }

        // PKG_INVENTORY


        public async Task ConsumeSupplyAsync(int supplyId, int amount)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "BEGIN ADMINISTRATOR.PKG_INVENTORY.ConsumeSupply(:p0, :p1); END;",
                supplyId, amount
            );
        }

        public async Task AddSupplyDeliveryAsync(int supplyId, int amount)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "BEGIN ADMINISTRATOR.PKG_INVENTORY.AddSupplyDelivery(:p0, :p1); END;",
                supplyId, amount
            );
        }

        public async Task<decimal> CalculateAssetsValueAsync()
        {
            var retParam = new OracleParameter("ret", OracleDbType.Decimal, ParameterDirection.Output);

            await _context.Database.ExecuteSqlRawAsync(
                "BEGIN :ret := ADMINISTRATOR.PKG_INVENTORY.CalculateAssetsValue(); END;",
                retParam
            );

            if (retParam.Value == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToDecimal(retParam.Value.ToString());
        }

        // PKG_REPORTS
        public async Task<List<MonthlyCostDto>> GetMonthlyCostsAsync(int year, int month)
        {
            var result = new List<MonthlyCostDto>();

            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "BEGIN :ret := ADMINISTRATOR.PKG_REPORTS.MonthlyCosts(:p0, :p1); END;";

            var retParam = new OracleParameter("ret", OracleDbType.RefCursor, ParameterDirection.Output);
            command.Parameters.Add(retParam);
            command.Parameters.Add(new OracleParameter("p0", year));
            command.Parameters.Add(new OracleParameter("p1", month));

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new MonthlyCostDto
                {
                    Year = Convert.ToInt32(reader["Year"]),
                    Month = Convert.ToInt32(reader["Month"]),
                    TotalSalariesCost = Convert.ToInt32(reader["TotalSalariesCost"])
                });
            }

            return result;
        }

        public async Task<List<SupplyUsageDto>> GetSuppliesUsageReportAsync(DateTime startDate, DateTime endDate)
        {
            var result = new List<SupplyUsageDto>();

            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "BEGIN :ret := ADMINISTRATOR.PKG_REPORTS.SuppliesUsageReport(:p0, :p1); END;";

            var retParam = new OracleParameter("ret", OracleDbType.RefCursor, ParameterDirection.Output);
            command.Parameters.Add(retParam);
            command.Parameters.Add(new OracleParameter("p0", startDate));
            command.Parameters.Add(new OracleParameter("p1", endDate));

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new SupplyUsageDto
                {
                    Name = reader["Name"].ToString(),
                    TotalUsed = Convert.ToInt32(reader["TotalUsed"])
                });
            }

            return result;
        }

        // WIDOKI (VIEWS)

        public async Task<List<DoctorAvailabilityViewDto>> GetDoctorAvailabilityViewAsync()
        {
            var result = new List<DoctorAvailabilityViewDto>();
            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM ADMINISTRATOR.\"v_DoctorAvailability\"";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new DoctorAvailabilityViewDto
                {
                    DoctorId = Convert.ToInt32(reader["DoctorId"]),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Specialization = reader["Specialization"].ToString(),
                    Start = reader.GetDateTime(reader.GetOrdinal("Start")),
                    End = reader.GetDateTime(reader.GetOrdinal("End"))
                });
            }
            return result;
        }

        public async Task<List<PatientMedicalHistoryViewDto>> GetPatientMedicalHistoryViewAsync(int patientId)
        {
            var result = new List<PatientMedicalHistoryViewDto>();
            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM ADMINISTRATOR.\"v_PatientMedicalHistory\" WHERE \"PatientId\" = :p0";
            command.Parameters.Add(new OracleParameter("p0", patientId));

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new PatientMedicalHistoryViewDto
                {
                    PatientId = Convert.ToInt32(reader["PatientId"]),
                    PatientFirstName = reader["PatientFirstName"].ToString(),
                    PatientLastName = reader["PatientLastName"].ToString(),
                    VisitDate = reader.GetDateTime(reader.GetOrdinal("VisitDate")),
                    Symptoms = reader["Symptoms"].ToString(),
                    Illness = reader["Illness"].ToString(),
                    DoctorFirstName = reader["DoctorFirstName"].ToString(),
                    DoctorLastName = reader["DoctorLastName"].ToString(),
                    RoomId = Convert.ToInt32(reader["RoomId"])
                });
            }
            return result;
        }

        public async Task<List<LowStockSupplyViewDto>> GetLowStockSuppliesViewAsync()
        {
            var result = new List<LowStockSupplyViewDto>();
            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM ADMINISTRATOR.\"v_LowStockSupplies\"";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new LowStockSupplyViewDto
                {
                    SupplyId = Convert.ToInt32(reader["SupplyId"]),
                    SupplyName = reader["SupplyName"].ToString(),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    RoomNumber = Convert.ToInt32(reader["RoomNumber"]),
                    RoomName = reader["RoomName"].ToString()
                });
            }
            return result;
        }
    }
}
