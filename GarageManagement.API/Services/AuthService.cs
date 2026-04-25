using GarageManagement.API.Data;
using GarageManagement.API.Helpers;
using GarageManagement.API.Models.DTOs.Auth;
using GarageManagement.API.Models.Entities;
using GarageManagement.API.Models.Entities.Enums;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public AuthService(AppDbContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    public async Task<LoginResponseDto> RegisterCustomer(RegisterCustomerDto dto)
    {
        var existingUser = await _context.Users.AnyAsync(user => user.Email == dto.Email);
        if (existingUser)
        {
            throw new Exception("Email already registered");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Phone = dto.Phone,
            Role = UserRole.Customer
        };

        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            CustomerId = user.Id,
            Make = dto.VehicleMake,
            Model = dto.VehicleModel,
            VehicleNumber = dto.VehicleNumber,
            Year = dto.VehicleYear
        };

        _context.Users.Add(user);
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        var token = _jwtHelper.GenerateToken(user);
        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<LoginResponseDto> RegisterStaff(RegisterStaffDto dto)
    {
        var existingUser = await _context.Users.AnyAsync(user => user.Email == dto.Email);
        if (existingUser)
        {
            throw new Exception("Email already registered");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Phone = dto.Phone,
            Role = UserRole.Staff
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _jwtHelper.GenerateToken(user);
        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<LoginResponseDto> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == dto.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            throw new Exception("Invalid email or password");
        }

        var token = _jwtHelper.GenerateToken(user);
        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<List<UserProfileDto>> GetAllStaff()
    {
        return await _context.Users
            .Where(user => user.Role == UserRole.Staff)
            .Select(user => new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt
            })
            .ToListAsync();
    }

    public async Task DeleteStaff(Guid staffId)
    {
        var staff = await _context.Users.FirstOrDefaultAsync(user => user.Id == staffId && user.Role == UserRole.Staff);
        if (staff is null)
        {
            throw new Exception("Staff not found");
        }

        _context.Users.Remove(staff);
        await _context.SaveChangesAsync();
    }
}