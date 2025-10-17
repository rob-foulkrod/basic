using EquipmentMaintenanceTracker.Models;
using EquipmentMaintenanceTracker.Services;
using EquipmentMaintenanceTracker.Validation;
using EquipmentMaintenanceTracker.Validation.Strategies;

namespace EquipmentMaintenanceTracker.Tests.Services;

public class EquipmentServiceAddEquipmentTests
{
    /// <summary>
    /// Creates a properly configured EquipmentService with validation context and strategies.
    /// </summary>
    private static EquipmentService CreateEquipmentService()
    {
        var validationContext = new ValidationContext();
        var service = new EquipmentService(validationContext);
        validationContext.RegisterStrategy("SerialNumber", new SerialNumberValidationStrategy(service.GetAllEquipment()));
        return service;
    }

    [Fact]
    public void AddEquipment_WithValidEquipment_ShouldAssignIdAndAddToList()
    {
        var service = CreateEquipmentService();
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "TEST-001",
            Category = "Testing",
            PurchaseDate = new DateTime(2023, 1, 1),
            Status = "Active"
        };
        var initialCount = service.GetAllEquipment().Count;

        service.AddEquipment(equipment);

        Assert.True(equipment.Id > 0);
        Assert.Equal(initialCount + 1, service.GetAllEquipment().Count);
        Assert.Contains(equipment, service.GetAllEquipment());
    }

    [Fact]
    public void AddEquipment_WithNullEquipment_ShouldThrowArgumentNullException()
    {
        var service = CreateEquipmentService();

        var exception = Assert.Throws<ArgumentNullException>(() => service.AddEquipment(null!));

        Assert.NotNull(exception);
    }







    [Fact]
    public void AddEquipment_MultipleEquipments_ShouldAssignIncrementingIds()
    {
        var service = CreateEquipmentService();
        var equipment1 = new Equipment
        {
            Name = "Equipment 1",
            SerialNumber = "TEST-001",
            Category = "Testing",
            PurchaseDate = new DateTime(2023, 1, 1),
            Status = "Active"
        };
        var equipment2 = new Equipment
        {
            Name = "Equipment 2",
            SerialNumber = "TEST-002",
            Category = "Testing",
            PurchaseDate = new DateTime(2023, 1, 2),
            Status = "Active"
        };

        service.AddEquipment(equipment1);
        service.AddEquipment(equipment2);

        Assert.True(equipment1.Id > 0);
        Assert.True(equipment2.Id > 0);
        Assert.True(equipment2.Id > equipment1.Id);
        Assert.Equal(equipment1.Id + 1, equipment2.Id);
    }

 

    [Fact]
    public void AddEquipment_AfterServiceInitialization_ShouldContinueFromLastId()
    {
        var service = CreateEquipmentService();
        var existingEquipmentCount = service.GetAllEquipment().Count;
        var lastExistingId = service.GetAllEquipment().Max(e => e.Id);
        
        var equipment = new Equipment
        {
            Name = "New Equipment",
            SerialNumber = "NEW-001",
            Category = "Testing",
            PurchaseDate = new DateTime(2023, 1, 1),
            Status = "Active"
        };

        service.AddEquipment(equipment);

        Assert.Equal(lastExistingId + 1, equipment.Id);
        Assert.Equal(existingEquipmentCount + 1, service.GetAllEquipment().Count);
    }
}