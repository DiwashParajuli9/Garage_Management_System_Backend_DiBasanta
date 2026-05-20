namespace GarageManagement.API.Models.DTOs.Staff;

public class StaffProfileDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public List<string> Permissions { get; set; } = new();
}

public class StaffSalesHistoryDto
{
    public Guid InvoiceId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public decimal Amount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public int ItemCount { get; set; }
}

public class UpdateStaffProfileDto
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}
