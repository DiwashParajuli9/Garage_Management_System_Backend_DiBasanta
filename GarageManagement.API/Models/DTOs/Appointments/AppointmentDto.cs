namespace GarageManagement.API.Models.DTOs.Appointments;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string VehicleNumber { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateAppointmentDto
{
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class UpdateAppointmentDto
{
    public string Status { get; set; } = string.Empty;
    public DateTime? AppointmentDate { get; set; }
    public TimeSpan? AppointmentTime { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class AppointmentStatsDto
{
    public int PendingCount { get; set; }
    public int ConfirmedCount { get; set; }
    public int CompletedCount { get; set; }
    public int RejectedCount { get; set; }
}

public class AppointmentFilterDto
{
    public string? SearchTerm { get; set; }
    public List<string>? Statuses { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
