using GarageManagement.API.Models.DTOs.Reports;

namespace GarageManagement.API.Services.Interfaces;

/// <summary>
/// Service contract for F9 – Customer Reports.
/// </summary>
public interface ICustomerReportService
{
    /// <summary>Returns total number of registered customers.</summary>
    Task<TotalCustomersDto> GetTotalCustomersAsync();

    /// <summary>Returns customers grouped by vehicle make.</summary>
    Task<List<CustomersByVehicleDto>> GetCustomersByVehicleAsync();

    /// <summary>Returns the <paramref name="count"/> most recently registered customers.</summary>
    Task<List<RecentCustomerDto>> GetRecentCustomersAsync(int count = 10);

    /// <summary>Returns service history for all customers or a specific customer.</summary>
    Task<List<dynamic>> GetCustomerServiceHistoryAsync(Guid? customerId = null);

    /// <summary>Returns sales invoices for all customers or a specific customer.</summary>
    Task<List<dynamic>> GetCustomerSalesInvoicesAsync(Guid? customerId = null);
}
