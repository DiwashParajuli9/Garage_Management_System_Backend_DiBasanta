using GarageManagement.API.Models.DTOs.Reports;

namespace GarageManagement.API.Services.Interfaces;

public interface IFinancialReportService
{
    Task<DailyReportDto> GetDailyReport(DateTime date);
    Task<MonthlyReportDto> GetMonthlyReport(int year, int month);
    Task<YearlyReportDto> GetYearlyReport(int year);
}