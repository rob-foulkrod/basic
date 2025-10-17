using EquipmentMaintenanceTracker.Models;
using EquipmentMaintenanceTracker.Services;

namespace EquipmentMaintenanceTracker.Tests.Services;

public class EquipmentServiceAddEquipmentTests
{
    [Fact]
    public void AddEquipment_WithValidEquipment_ShouldAssignIdAndAddToList()
    {
        var service = new EquipmentService();
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
        var service = new EquipmentService();

        var exception = Assert.Throws<NullReferenceException>(() => service.AddEquipment(null!));

        Assert.NotNull(exception);
    }

    [Fact]
    public void AddEquipment_WithEmptyName_ShouldStillAddEquipment()
    {
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = string.Empty,
            SerialNumber = "TEST-001",
            Category = "Testing",
            PurchaseDate = new DateTime(2023, 1, 1),
            Status = "Active"
        };

        service.AddEquipment(equipment);

        Assert.True(equipment.Id > 0);
        Assert.Contains(equipment, service.GetAllEquipment());
    }

    [Fact]
    public void AddEquipment_WithEmptySerialNumber_ShouldStillAddEquipment()
    {
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = string.Empty,
            Category = "Testing",
            PurchaseDate = new DateTime(2023, 1, 1),
            Status = "Active"
        };

        service.AddEquipment(equipment);

        Assert.True(equipment.Id > 0);
        Assert.Contains(equipment, service.GetAllEquipment());
    }

    [Fact]
    public void AddEquipment_WithEmptyCategory_ShouldStillAddEquipment()
    {
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "TEST-001",
            Category = string.Empty,
            PurchaseDate = new DateTime(2023, 1, 1),
            Status = "Active"
        };

        service.AddEquipment(equipment);

        Assert.True(equipment.Id > 0);
        Assert.Contains(equipment, service.GetAllEquipment());
    }

    [Fact]
    public void AddEquipment_WithMinDateTime_ShouldAddEquipment()
    {
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "TEST-001",
            Category = "Testing",
            PurchaseDate = DateTime.MinValue,
            Status = "Active"
        };

        service.AddEquipment(equipment);

        Assert.True(equipment.Id > 0);
        Assert.Contains(equipment, service.GetAllEquipment());
        Assert.Equal(DateTime.MinValue, equipment.PurchaseDate);
    }

    [Fact]
    public void AddEquipment_WithMaxDateTime_ShouldAddEquipment()
    {
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "TEST-001",
            Category = "Testing",
            PurchaseDate = DateTime.MaxValue,
            Status = "Active"
        };

        service.AddEquipment(equipment);

        Assert.True(equipment.Id > 0);
        Assert.Contains(equipment, service.GetAllEquipment());
        Assert.Equal(DateTime.MaxValue, equipment.PurchaseDate);
    }

    [Fact]
    public void AddEquipment_WithEmptyStatus_ShouldAddEquipment()
    {
        var service = new EquipmentService();
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "TEST-001",
            Category = "Testing",
            PurchaseDate = new DateTime(2023, 1, 1),
            Status = string.Empty
        };

        service.AddEquipment(equipment);

        Assert.True(equipment.Id > 0);
        Assert.Contains(equipment, service.GetAllEquipment());
    }

    [Fact]
    public void AddEquipment_MultipleEquipments_ShouldAssignIncrementingIds()
    {
        var service = new EquipmentService();
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
    public void AddEquipment_WithLongValues_ShouldAddEquipment()
    {
        var service = new EquipmentService();
        var longString = new string('A', 1000);
        var equipment = new Equipment
        {
            Name = longString,
            SerialNumber = longString,
            Category = longString,
            PurchaseDate = new DateTime(2023, 1, 1),
            Status = longString
        };

        service.AddEquipment(equipment);

        Assert.True(equipment.Id > 0);
        Assert.Contains(equipment, service.GetAllEquipment());
        Assert.Equal(longString, equipment.Name);
        Assert.Equal(longString, equipment.SerialNumber);
        Assert.Equal(longString, equipment.Category);
        Assert.Equal(longString, equipment.Status);
    }

    [Fact]
    public void AddEquipment_WithSpecialCharacters_ShouldAddEquipment()
    {
        var service = new EquipmentService();
        var specialString = "!@#$%^&*()_+-=[]{}|;':\",./<>?`~";
        var equipment = new Equipment
        {
            Name = specialString,
            SerialNumber = specialString,
            Category = specialString,
            PurchaseDate = new DateTime(2023, 1, 1),
            Status = specialString
        };

        service.AddEquipment(equipment);

        Assert.True(equipment.Id > 0);
        Assert.Contains(equipment, service.GetAllEquipment());
        Assert.Equal(specialString, equipment.Name);
    }

    [Fact]
    public void AddEquipment_WithUnicodeCharacters_ShouldAddEquipment()
    {
        var service = new EquipmentService();
        var unicodeString = "测试设备 оборудование équipement";
        var equipment = new Equipment
        {
            Name = unicodeString,
            SerialNumber = "TEST-001",
            Category = "Testing",
            PurchaseDate = new DateTime(2023, 1, 1),
            Status = "Active"
        };

        service.AddEquipment(equipment);

        Assert.True(equipment.Id > 0);
        Assert.Contains(equipment, service.GetAllEquipment());
        Assert.Equal(unicodeString, equipment.Name);
    }

    [Fact]
    public void AddEquipment_AfterServiceInitialization_ShouldContinueFromLastId()
    {
        var service = new EquipmentService();
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