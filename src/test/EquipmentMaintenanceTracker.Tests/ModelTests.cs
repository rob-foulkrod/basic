using EquipmentMaintenanceTracker.Models;

namespace EquipmentMaintenanceTracker.Tests;

public class ModelTests
{
    [Fact]
    public void Equipment_ShouldHaveAllRequiredProperties()
    {
        // Arrange & Act
        var equipment = new Equipment
        {
            Id = 1,
            Name = "Test Equipment",
            SerialNumber = "TEST-001",
            Category = "Test Category",
            PurchaseDate = new DateTime(2024, 1, 1),
            Status = "Active"
        };

        // Assert
        Assert.Equal(1, equipment.Id);
        Assert.Equal("Test Equipment", equipment.Name);
        Assert.Equal("TEST-001", equipment.SerialNumber);
        Assert.Equal("Test Category", equipment.Category);
        Assert.Equal(new DateTime(2024, 1, 1), equipment.PurchaseDate);
        Assert.Equal("Active", equipment.Status);
    }

    [Fact]
    public void MaintenanceRecord_ShouldHaveAllRequiredProperties()
    {
        // Arrange & Act
        var record = new MaintenanceRecord
        {
            Id = 1,
            EquipmentId = 5,
            MaintenanceDate = new DateTime(2024, 6, 15),
            MaintenanceType = "Preventive",
            Description = "Regular maintenance",
            PerformedBy = "John Doe",
            Cost = 150.50m
        };

        // Assert
        Assert.Equal(1, record.Id);
        Assert.Equal(5, record.EquipmentId);
        Assert.Equal(new DateTime(2024, 6, 15), record.MaintenanceDate);
        Assert.Equal("Preventive", record.MaintenanceType);
        Assert.Equal("Regular maintenance", record.Description);
        Assert.Equal("John Doe", record.PerformedBy);
        Assert.Equal(150.50m, record.Cost);
    }

    [Fact]
    public void Equipment_DefaultStatus_ShouldBeActive()
    {
        // Arrange & Act
        var equipment = new Equipment();

        // Assert
        Assert.Equal("Active", equipment.Status);
    }

    [Fact]
    public void Equipment_DefaultStringProperties_ShouldBeEmpty()
    {
        // Arrange & Act
        var equipment = new Equipment();

        // Assert
        Assert.Equal(string.Empty, equipment.Name);
        Assert.Equal(string.Empty, equipment.SerialNumber);
        Assert.Equal(string.Empty, equipment.Category);
    }

    [Fact]
    public void MaintenanceRecord_DefaultStringProperties_ShouldBeEmpty()
    {
        // Arrange & Act
        var record = new MaintenanceRecord();

        // Assert
        Assert.Equal(string.Empty, record.MaintenanceType);
        Assert.Equal(string.Empty, record.Description);
        Assert.Equal(string.Empty, record.PerformedBy);
    }
}
