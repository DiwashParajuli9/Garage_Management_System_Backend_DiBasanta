namespace GarageManagement.API.Models.DTOs.Reports;

// ── F9 Report Response DTOs ─────────────────────────────────────────────────

/// <summary>Total customer count.</summary>
public class TotalCustomersDto
{
    public int TotalCustomers { get; set; }
}

/// <summary>Customers grouped by vehicle make.</summary>
public class CustomersByVehicleDto
{
    public string VehicleMake { get; set; } = string.Empty;
    public int CustomerCount { get; set; }
}

/// <summary>Summary of a recently registered customer.</summary>
public class RecentCustomerDto
{
    public Guid CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public int VehicleCount { get; set; }
}

// ── Sales & Financial Report DTOs ───────────────────────────────────────────

public class PartSalesDto
{
    public string PartName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}

public class DailySummaryDto
{
    public DateTime Date { get; set; }
    public decimal Sales { get; set; }
    public decimal Purchases { get; set; }
    public decimal Profit { get; set; }
}

public class MonthlySummaryDto
{
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public decimal Sales { get; set; }
    public decimal Purchases { get; set; }
    public decimal Profit { get; set; }
}

public class DailyReportDto
{
    public DateTime Date { get; set; }
    public decimal TotalSales { get; set; }
    public decimal TotalPurchases { get; set; }
    public decimal Profit { get; set; }
    public int TotalTransactions { get; set; }
    public List<PartSalesDto> TopSellingParts { get; set; } = new();
}

public class MonthlyReportDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public decimal TotalSales { get; set; }
    public decimal TotalPurchases { get; set; }
    public decimal Profit { get; set; }
    public int TotalTransactions { get; set; }
    public List<PartSalesDto> TopSellingParts { get; set; } = new();
    public List<DailySummaryDto> DailyBreakdown { get; set; } = new();
}

public class YearlyReportDto
{
    public int Year { get; set; }
    public decimal TotalSales { get; set; }
    public decimal TotalPurchases { get; set; }
    public decimal Profit { get; set; }
    public int TotalTransactions { get; set; }
    public List<PartSalesDto> TopSellingParts { get; set; } = new();
    public List<MonthlySummaryDto> MonthlyBreakdown { get; set; } = new();
}