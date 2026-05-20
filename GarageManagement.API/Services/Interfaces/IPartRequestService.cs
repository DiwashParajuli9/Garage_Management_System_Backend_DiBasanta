using GarageManagement.API.Models.DTOs.PartRequests;

namespace GarageManagement.API.Services.Interfaces;

public interface IPartRequestService
{
    Task<PartRequestDto> CreatePartRequestAsync(CreatePartRequestDto dto);
    Task<List<PartRequestDto>> GetPartRequestsAsync(string? status = null, string? searchTerm = null);
    Task<PartRequestDto> GetPartRequestByIdAsync(Guid partRequestId);
    Task<PartRequestDto> UpdatePartRequestAsync(Guid partRequestId, UpdatePartRequestDto dto);
    Task<List<PartRequestCommentHistoryDto>> GetPartRequestCommentHistoryAsync(Guid partRequestId);
    Task DeletePartRequestAsync(Guid partRequestId);
}
