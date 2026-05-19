namespace GarageManagement.API.Models.DTOs.Vendor
{
    public class CreateVendorDto
    {
        public required string Name { get; set; }
        public required string ContactPerson { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
    }
}