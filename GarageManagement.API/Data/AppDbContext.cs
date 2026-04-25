using GarageManagement.API.Models.Entities;
using GarageManagement.API.Models.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<VehiclePart> VehicleParts => Set<VehiclePart>();
    public DbSet<PurchaseInvoice> PurchaseInvoices => Set<PurchaseInvoice>();
    public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems => Set<PurchaseInvoiceItem>();
    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
    public DbSet<SalesInvoiceItem> SalesInvoiceItems => Set<SalesInvoiceItem>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<PartRequest> PartRequests => Set<PartRequest>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(user => user.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Appointment>()
            .Property(appointment => appointment.Status)
            .HasConversion<string>();

        modelBuilder.Entity<PartRequest>()
            .Property(partRequest => partRequest.Status)
            .HasConversion<string>();

        modelBuilder.Entity<User>()
            .HasMany(user => user.Vehicles)
            .WithOne(vehicle => vehicle.Customer)
            .HasForeignKey(vehicle => vehicle.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(user => user.CustomerSalesInvoices)
            .WithOne(salesInvoice => salesInvoice.Customer)
            .HasForeignKey(salesInvoice => salesInvoice.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(user => user.StaffSalesInvoices)
            .WithOne(salesInvoice => salesInvoice.Staff)
            .HasForeignKey(salesInvoice => salesInvoice.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Vendor>()
            .HasMany(vendor => vendor.VehicleParts)
            .WithOne(vehiclePart => vehiclePart.Vendor)
            .HasForeignKey(vehiclePart => vehiclePart.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Vendor>()
            .HasMany(vendor => vendor.PurchaseInvoices)
            .WithOne(purchaseInvoice => purchaseInvoice.Vendor)
            .HasForeignKey(purchaseInvoice => purchaseInvoice.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseInvoice>()
            .HasMany(purchaseInvoice => purchaseInvoice.Items)
            .WithOne(item => item.PurchaseInvoice)
            .HasForeignKey(item => item.PurchaseInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SalesInvoice>()
            .HasMany(salesInvoice => salesInvoice.Items)
            .WithOne(item => item.SalesInvoice)
            .HasForeignKey(item => item.SalesInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<VehiclePart>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}