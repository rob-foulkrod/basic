using EquipmentMaintenanceTracker.Models;
using EquipmentMaintenanceTracker.Services;
using EquipmentMaintenanceTracker.Validation;
using EquipmentMaintenanceTracker.Validation.Strategies;

namespace EquipmentMaintenanceTracker.Tests.Services;

/// <summary>
/// Tests to verify dependency injection functionality in EquipmentService.
/// </summary>
public class EquipmentServiceDependencyInjectionTests
{
    [Fact]
    public void Constructor_WithValidValidationContext_ShouldSucceed()
    {
        // Arrange
        var validationContext = new ValidationContext();

        // Act
        var service = new EquipmentService(validationContext);

        // Assert
        Assert.NotNull(service);
        Assert.NotNull(service.GetValidationContext());
    }

    [Fact]
    public void Constructor_WithNullValidationContext_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new EquipmentService(null!));
        Assert.Equal("validationContext", exception.ParamName);
    }

    [Fact]
    public void GetValidationContext_ShouldReturnInjectedContext()
    {
        // Arrange
        var validationContext = new ValidationContext();
        var service = new EquipmentService(validationContext);

        // Act
        var result = service.GetValidationContext();

        // Assert
        Assert.Same(validationContext, result);
    }

    [Fact]
    public void EquipmentService_WithCustomValidationStrategies_ShouldUseInjectedStrategies()
    {
        // Arrange
        var validationContext = new ValidationContext();
        var service = new EquipmentService(validationContext);
        
        // Register a custom validation strategy
        validationContext.RegisterStrategy("SerialNumber", new SerialNumberValidationStrategy(service.GetAllEquipment()));

        // Act - Try to add equipment with invalid serial number
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "INVALID", // Invalid format
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Assert
        var exception = Assert.Throws<ValidationException>(() => service.AddEquipment(equipment));
        Assert.False(exception.ValidationResult.IsValid);
        Assert.NotEmpty(exception.ValidationResult.Errors);
    }

    [Fact]
    public void EquipmentService_WithNoValidationStrategies_ShouldAllowAnyEquipment()
    {
        // Arrange
        var validationContext = new ValidationContext();
        var service = new EquipmentService(validationContext);
        
        // No validation strategies registered

        // Act - Try to add equipment with invalid serial number
        var equipment = new Equipment
        {
            Name = "Test Equipment",
            SerialNumber = "INVALID", // Would normally be invalid
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        service.AddEquipment(equipment);

        // Assert - Should succeed because no validation strategies are registered
        Assert.True(equipment.Id > 0);
        var result = service.GetEquipmentById(equipment.Id);
        Assert.NotNull(result);
    }

    [Fact]
    public void EquipmentService_CanSwitchValidationStrategiesAtRuntime()
    {
        // Arrange
        var validationContext = new ValidationContext();
        var service = new EquipmentService(validationContext);
        
        // Initially no validation
        var equipment1 = new Equipment
        {
            Name = "Equipment 1",
            SerialNumber = "INVALID",
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };
        
        service.AddEquipment(equipment1);
        Assert.True(equipment1.Id > 0);

        // Now register validation strategy
        validationContext.RegisterStrategy("SerialNumber", new SerialNumberValidationStrategy(service.GetAllEquipment()));

        // Act & Assert - Should now fail validation
        var equipment2 = new Equipment
        {
            Name = "Equipment 2",
            SerialNumber = "INVALID2",
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        Assert.Throws<ValidationException>(() => service.AddEquipment(equipment2));
    }
}
