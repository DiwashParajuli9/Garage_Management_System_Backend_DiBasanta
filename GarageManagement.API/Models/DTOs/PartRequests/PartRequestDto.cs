namespace GarageManagement.API.Models.DTOs.PartRequests;

public class PartRequestDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string PartName { get; set; } = string.Empty;
    public Guid VehicleId { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public string Status { get; set; } = "Pending";
    public string CustomerNotes { get; set; } = string.Empty;
    public string StaffComment { get; set; } = string.Empty;
}

public class CreatePartRequestDto
{
    public Guid CustomerId { get; set; }
    public string PartName { get; set; } = string.Empty;
    public Guid VehicleId { get; set; }
    public string CustomerNotes { get; set; } = string.Empty;
}

public class UpdatePartRequestDto
{
    public string Status { get; set; } = string.Empty;
    public string StaffComment { get; set; } = string.Empty;
}

public class PartRequestCommentHistoryDto
{
    public Guid Id { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;
}
