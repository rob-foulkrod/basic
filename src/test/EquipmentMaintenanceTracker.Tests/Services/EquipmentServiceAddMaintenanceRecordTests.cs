using EquipmentMaintenanceTracker.Models;
using EquipmentMaintenanceTracker.Services;

namespace EquipmentMaintenanceTracker.Tests.Services;

public class EquipmentServiceAddMaintenanceRecordTests
{
    [Fact]
    public void AddMaintenanceRecord_WithNullRecord_ShouldThrowArgumentNullException()
    {
        // Arrange
        var service = new EquipmentService();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => service.AddMaintenanceRecord(null!));

        Assert.NotNull(exception);
        Assert.Equal("record", exception.ParamName);
        Assert.Contains("Maintenance record cannot be null", exception.Message);
    }

    [Fact]
    public void AddMaintenanceRecord_WithValidRecord_ShouldAssignIdAndAddToList()
    {
        // Arrange
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "TEST-001",
            Category = "Testing",
            PurchaseDate = new DateTime(2023, 1, 1),
            Status = "Active"
        };
        service.AddEquipment(equipment);

        var record = new MaintenanceRecord
        {
            EquipmentId = equipment.Id,
            MaintenanceDate = DateTime.Now,
            MaintenanceType = "Preventive",
            Description = "Test maintenance",
            PerformedBy = "Technician",
            Cost = 100.00m
        };
        var initialCount = service.GetAllMaintenanceRecords().Count;

        // Act
        service.AddMaintenanceRecord(record);

        // Assert
        Assert.True(record.Id > 0);
        Assert.Equal(initialCount + 1, service.GetAllMaintenanceRecords().Count);
        Assert.Contains(record, service.GetAllMaintenanceRecords());
    }
}
