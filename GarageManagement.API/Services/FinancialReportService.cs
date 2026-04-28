using System.Globalization;
using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.Reports;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

public class FinancialReportService : IFinancialReportService
{
    private readonly AppDbContext _context;

    public FinancialReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DailyReportDto> GetDailyReport(DateTime date)
    {
        var targetDate = date.Date;

        var totalSales = await _context.SalesInvoices
            .Where(invoice => invoice.CreatedAt.Date == targetDate)
            .SumAsync(invoice => (decimal?)invoice.FinalAmount) ?? 0m;

        var totalPurchases = await _context.PurchaseInvoices
            .Where(invoice => invoice.CreatedAt.Date == targetDate)
            .SumAsync(invoice => (decimal?)invoice.TotalAmount) ?? 0m;

        var totalTransactions = await _context.SalesInvoices
            .CountAsync(invoice => invoice.CreatedAt.Date == targetDate);

        var topSellingParts = await _context.SalesInvoiceItems
            .Where(item => item.SalesInvoice.CreatedAt.Date == targetDate)
            .GroupBy(item => item.VehiclePart.Name)
            .Select(group => new PartSalesDto
            {
                PartName = group.Key,
                QuantitySold = group.Sum(item => item.Quantity),
                Revenue = group.Sum(item => item.Quantity * item.UnitPrice)
            })
            .OrderByDescending(item => item.QuantitySold)
            .ThenByDescending(item => item.Revenue)
            .Take(5)
            .ToListAsync();

        return new DailyReportDto
        {
            Date = targetDate,
            TotalSales = totalSales,
            TotalPurchases = totalPurchases,
            Profit = totalSales - totalPurchases,
            TotalTransactions = totalTransactions,
            TopSellingParts = topSellingParts
        };
    }

    public async Task<MonthlyReportDto> GetMonthlyReport(int year, int month)
    {
        var totalSales = await _context.SalesInvoices
            .Where(invoice => invoice.CreatedAt.Year == year && invoice.CreatedAt.Month == month)
            .SumAsync(invoice => (decimal?)invoice.FinalAmount) ?? 0m;

        var totalPurchases = await _context.PurchaseInvoices
            .Where(invoice => invoice.CreatedAt.Year == year && invoice.CreatedAt.Month == month)
            .SumAsync(invoice => (decimal?)invoice.TotalAmount) ?? 0m;

        var totalTransactions = await _context.SalesInvoices
            .CountAsync(invoice => invoice.CreatedAt.Year == year && invoice.CreatedAt.Month == month);

        var topSellingParts = await _context.SalesInvoiceItems
            .Where(item => item.SalesInvoice.CreatedAt.Year == year && item.SalesInvoice.CreatedAt.Month == month)
            .GroupBy(item => item.VehiclePart.Name)
            .Select(group => new PartSalesDto
            {
                PartName = group.Key,
                QuantitySold = group.Sum(item => item.Quantity),
                Revenue = group.Sum(item => item.Quantity * item.UnitPrice)
            })
            .OrderByDescending(item => item.QuantitySold)
            .ThenByDescending(item => item.Revenue)
            .Take(5)
            .ToListAsync();

        var salesByDay = await _context.SalesInvoices
            .Where(invoice => invoice.CreatedAt.Year == year && invoice.CreatedAt.Month == month)
            .GroupBy(invoice => invoice.CreatedAt.Date)
            .Select(group => new
            {
                Date = group.Key,
                Sales = group.Sum(invoice => invoice.FinalAmount)
            })
            .ToListAsync();

        var purchasesByDay = await _context.PurchaseInvoices
            .Where(invoice => invoice.CreatedAt.Year == year && invoice.CreatedAt.Month == month)
            .GroupBy(invoice => invoice.CreatedAt.Date)
            .Select(group => new
            {
                Date = group.Key,
                Purchases = group.Sum(invoice => invoice.TotalAmount)
            })
            .ToListAsync();

        var salesLookup = salesByDay.ToDictionary(item => item.Date, item => item.Sales);
        var purchasesLookup = purchasesByDay.ToDictionary(item => item.Date, item => item.Purchases);
        var allDays = salesLookup.Keys
            .Union(purchasesLookup.Keys)
            .OrderBy(day => day)
            .ToList();

        var dailyBreakdown = allDays
            .Select(day =>
            {
                salesLookup.TryGetValue(day, out var dailySales);
                purchasesLookup.TryGetValue(day, out var dailyPurchases);

                return new DailySummaryDto
                {
                    Date = day,
                    Sales = dailySales,
                    Purchases = dailyPurchases,
                    Profit = dailySales - dailyPurchases
                };
            })
            .ToList();

        return new MonthlyReportDto
        {
            Year = year,
            Month = month,
            MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
            TotalSales = totalSales,
            TotalPurchases = totalPurchases,
            Profit = totalSales - totalPurchases,
            TotalTransactions = totalTransactions,
            TopSellingParts = topSellingParts,
            DailyBreakdown = dailyBreakdown
        };
    }

    public async Task<YearlyReportDto> GetYearlyReport(int year)
    {
        var totalSales = await _context.SalesInvoices
            .Where(invoice => invoice.CreatedAt.Year == year)
            .SumAsync(invoice => (decimal?)invoice.FinalAmount) ?? 0m;

        var totalPurchases = await _context.PurchaseInvoices
            .Where(invoice => invoice.CreatedAt.Year == year)
            .SumAsync(invoice => (decimal?)invoice.TotalAmount) ?? 0m;

        var totalTransactions = await _context.SalesInvoices
            .CountAsync(invoice => invoice.CreatedAt.Year == year);

        var topSellingParts = await _context.SalesInvoiceItems
            .Where(item => item.SalesInvoice.CreatedAt.Year == year)
            .GroupBy(item => item.VehiclePart.Name)
            .Select(group => new PartSalesDto
            {
                PartName = group.Key,
                QuantitySold = group.Sum(item => item.Quantity),
                Revenue = group.Sum(item => item.Quantity * item.UnitPrice)
            })
            .OrderByDescending(item => item.QuantitySold)
            .ThenByDescending(item => item.Revenue)
            .Take(5)
            .ToListAsync();

        var salesByMonth = await _context.SalesInvoices
            .Where(invoice => invoice.CreatedAt.Year == year)
            .GroupBy(invoice => invoice.CreatedAt.Month)
            .Select(group => new
            {
                Month = group.Key,
                Sales = group.Sum(invoice => invoice.FinalAmount)
            })
            .ToListAsync();

        var purchasesByMonth = await _context.PurchaseInvoices
            .Where(invoice => invoice.CreatedAt.Year == year)
            .GroupBy(invoice => invoice.CreatedAt.Month)
            .Select(group => new
            {
                Month = group.Key,
                Purchases = group.Sum(invoice => invoice.TotalAmount)
            })
            .ToListAsync();

        var salesLookup = salesByMonth.ToDictionary(item => item.Month, item => item.Sales);
        var purchasesLookup = purchasesByMonth.ToDictionary(item => item.Month, item => item.Purchases);
        var allMonths = salesLookup.Keys
            .Union(purchasesLookup.Keys)
            .OrderBy(month => month)
            .ToList();

        var monthlyBreakdown = allMonths
            .Select(month =>
            {
                salesLookup.TryGetValue(month, out var monthlySales);
                purchasesLookup.TryGetValue(month, out var monthlyPurchases);

                return new MonthlySummaryDto
                {
                    Month = month,
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    Sales = monthlySales,
                    Purchases = monthlyPurchases,
                    Profit = monthlySales - monthlyPurchases
                };
            })
            .ToList();

        return new YearlyReportDto
        {
            Year = year,
            TotalSales = totalSales,
            TotalPurchases = totalPurchases,
            Profit = totalSales - totalPurchases,
            TotalTransactions = totalTransactions,
            TopSellingParts = topSellingParts,
            MonthlyBreakdown = monthlyBreakdown
        };
    }
}