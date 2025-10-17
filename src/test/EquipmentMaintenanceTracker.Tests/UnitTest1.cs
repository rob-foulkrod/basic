using EquipmentMaintenanceTracker.Models;
using EquipmentMaintenanceTracker.Services;

namespace EquipmentMaintenanceTracker.Tests;

public class EquipmentServiceTests
{
    [Fact]
    public void AddEquipment_ShouldAddEquipmentWithUniqueId()
    {
        // Arrange
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "TEST-001",
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Act
        service.AddEquipment(equipment);
        var result = service.GetEquipmentById(equipment.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Equipment", result.Name);
        Assert.True(equipment.Id > 0);
    }

    [Fact]
    public void GetAllEquipment_ShouldReturnAllEquipment()
    {
        // Arrange
        var service = new EquipmentService();

        // Act
        var equipments = service.GetAllEquipment();

        // Assert - service seeds 2 equipment items by default
        Assert.NotEmpty(equipments);
        Assert.True(equipments.Count >= 2);
    }

    [Fact]
    public void GetEquipmentById_ShouldReturnCorrectEquipment()
    {
        // Arrange
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "TEST-001",
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };
        service.AddEquipment(equipment);

        // Act
        var result = service.GetEquipmentById(equipment.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(equipment.Id, result.Id);
        Assert.Equal("Test Equipment", result.Name);
    }

    [Fact]
    public void GetEquipmentById_ShouldReturnNullForNonExistentId()
    {
        // Arrange
        var service = new EquipmentService();

        // Act
        var result = service.GetEquipmentById(9999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void UpdateEquipment_ShouldUpdateExistingEquipment()
    {
        // Arrange
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = "Original Name",
            SerialNumber = "TEST-001",
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };
        service.AddEquipment(equipment);

        // Act
        equipment.Name = "Updated Name";
        equipment.Status = "Inactive";
        service.UpdateEquipment(equipment);
        var result = service.GetEquipmentById(equipment.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Name", result.Name);
        Assert.Equal("Inactive", result.Status);
    }

    [Fact]
    public void DeleteEquipment_ShouldRemoveEquipmentAndRelatedRecords()
    {
        // Arrange
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "TEST-001",
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };
        service.AddEquipment(equipment);
        var equipmentId = equipment.Id;

        // Act
        service.DeleteEquipment(equipmentId);
        var result = service.GetEquipmentById(equipmentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void AddMaintenanceRecord_ShouldAddRecordWithUniqueId()
    {
        // Arrange
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "TEST-001",
            Category = "Test",
            PurchaseDate = DateTime.Now,
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

        // Act
        service.AddMaintenanceRecord(record);
        var records = service.GetMaintenanceRecordsByEquipmentId(equipment.Id);

        // Assert
        Assert.NotEmpty(records);
        Assert.True(record.Id > 0);
        Assert.Contains(records, r => r.Id == record.Id);
    }

    [Fact]
    public void GetMaintenanceRecordsByEquipmentId_ShouldReturnOnlyRecordsForSpecifiedEquipment()
    {
        // Arrange
        var service = new EquipmentService();
        var equipment1 = new Equipment
        {
            Name = "Equipment 1",
            SerialNumber = "TEST-001",
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };
        var equipment2 = new Equipment
        {
            Name = "Equipment 2",
            SerialNumber = "TEST-002",
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };
        service.AddEquipment(equipment1);
        service.AddEquipment(equipment2);

        service.AddMaintenanceRecord(new MaintenanceRecord
        {
            EquipmentId = equipment1.Id,
            MaintenanceDate = DateTime.Now,
            MaintenanceType = "Preventive",
            Description = "Maintenance for equipment 1",
            PerformedBy = "Tech",
            Cost = 100m
        });

        service.AddMaintenanceRecord(new MaintenanceRecord
        {
            EquipmentId = equipment2.Id,
            MaintenanceDate = DateTime.Now,
            MaintenanceType = "Corrective",
            Description = "Maintenance for equipment 2",
            PerformedBy = "Tech",
            Cost = 200m
        });

        // Act
        var records = service.GetMaintenanceRecordsByEquipmentId(equipment1.Id);

        // Assert
        Assert.Single(records);
        Assert.All(records, r => Assert.Equal(equipment1.Id, r.EquipmentId));
    }

    [Fact]
    public void GetAllMaintenanceRecords_ShouldReturnAllRecords()
    {
        // Arrange
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "TEST-001",
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };
        service.AddEquipment(equipment);

        service.AddMaintenanceRecord(new MaintenanceRecord
        {
            EquipmentId = equipment.Id,
            MaintenanceDate = DateTime.Now,
            MaintenanceType = "Preventive",
            Description = "First maintenance",
            PerformedBy = "Tech",
            Cost = 100m
        });

        service.AddMaintenanceRecord(new MaintenanceRecord
        {
            EquipmentId = equipment.Id,
            MaintenanceDate = DateTime.Now,
            MaintenanceType = "Corrective",
            Description = "Second maintenance",
            PerformedBy = "Tech",
            Cost = 200m
        });

        // Act
        var records = service.GetAllMaintenanceRecords();

        // Assert
        Assert.True(records.Count >= 2);
    }
}

