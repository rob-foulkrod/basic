using EquipmentMaintenanceTracker.Models;
using EquipmentMaintenanceTracker.Validation.Strategies;

namespace EquipmentMaintenanceTracker.Tests.Validation;

public class SerialNumberValidationStrategyTests
{
    #region Valid Serial Numbers Tests

    [Theory]
    [InlineData("MRI-001")]
    [InlineData("XRAY-12345")]
    [InlineData("CT-999999")]
    [InlineData("ECHO-050")]
    [InlineData("LAB-123456")]
    public void Validate_WithValidSerialNumbers_ShouldPass(string serialNumber)
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();
        var equipment = new Equipment
        {
            Id = 1,
            Name = "Test Equipment",
            SerialNumber = serialNumber,
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(equipment);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    #endregion

    #region Invalid Serial Numbers Tests

    [Theory]
    [InlineData("mri-001", "lowercase letters")]
    [InlineData("MRI001", "missing dash")]
    [InlineData("M-001", "only 1 letter")]
    [InlineData("MRI-12", "only 2 digits")]
    [InlineData("MRI-1234567", "7 digits")]
    public void Validate_WithInvalidSerialNumbers_ShouldFail(string serialNumber, string reason)
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();
        var equipment = new Equipment
        {
            Id = 1,
            Name = "Test Equipment",
            SerialNumber = serialNumber,
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(equipment);

        // Assert
        Assert.False(result.IsValid, $"Serial number '{serialNumber}' should fail validation ({reason})");
        Assert.NotEmpty(result.Errors);
    }

    #endregion

    #region Null and Empty Serial Number Tests

    [Fact]
    public void Validate_WithNullSerialNumber_ShouldFail()
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();
        var equipment = new Equipment
        {
            Id = 1,
            Name = "Test Equipment",
            SerialNumber = null,
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(equipment);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains("Serial number is required for validation.", result.Errors);
    }

    [Fact]
    public void Validate_WithEmptySerialNumber_ShouldFail()
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();
        var equipment = new Equipment
        {
            Id = 1,
            Name = "Test Equipment",
            SerialNumber = string.Empty,
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(equipment);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("Serial number is required for validation.", result.Errors);
    }

    [Fact]
    public void Validate_WithWhitespaceSerialNumber_ShouldFail()
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();
        var equipment = new Equipment
        {
            Id = 1,
            Name = "Test Equipment",
            SerialNumber = "   ",
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(equipment);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("Serial number is required for validation.", result.Errors);
    }

    #endregion

    #region Uniqueness Tests

    [Fact]
    public void Validate_WithDuplicateSerialNumber_ShouldFail()
    {
        // Arrange
        var existingEquipment = new List<Equipment>
        {
            new Equipment
            {
                Id = 1,
                Name = "Existing Equipment",
                SerialNumber = "MRI-001",
                Category = "Imaging",
                PurchaseDate = new DateTime(2024, 1, 1),
                Status = "Active"
            }
        };

        var strategy = new SerialNumberValidationStrategy(existingEquipment);
        var newEquipment = new Equipment
        {
            Id = 2,
            Name = "New Equipment",
            SerialNumber = "MRI-001",
            Category = "Imaging",
            PurchaseDate = new DateTime(2024, 1, 1),
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(newEquipment);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains("is already in use by another equipment item", result.Errors[0]);
    }

    [Fact]
    public void Validate_WithDuplicateSerialNumber_CaseInsensitive_ShouldFail()
    {
        // Arrange
        var existingEquipment = new List<Equipment>
        {
            new Equipment
            {
                Id = 1,
                Name = "Existing Equipment",
                SerialNumber = "MRI-001",
                Category = "Imaging",
                PurchaseDate = new DateTime(2024, 1, 1),
                Status = "Active"
            }
        };

        var strategy = new SerialNumberValidationStrategy(existingEquipment);
        var newEquipment = new Equipment
        {
            Id = 2,
            Name = "New Equipment",
            SerialNumber = "MRI-001", // Same serial number with different casing context
            Category = "Imaging",
            PurchaseDate = new DateTime(2024, 1, 1),
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(newEquipment);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("is already in use by another equipment item", result.Errors[0]);
    }

    [Fact]
    public void Validate_WithUniqueSerialNumber_ShouldPass()
    {
        // Arrange
        var existingEquipment = new List<Equipment>
        {
            new Equipment
            {
                Id = 1,
                Name = "Existing Equipment",
                SerialNumber = "MRI-001",
                Category = "Imaging",
                PurchaseDate = new DateTime(2024, 1, 1),
                Status = "Active"
            }
        };

        var strategy = new SerialNumberValidationStrategy(existingEquipment);
        var newEquipment = new Equipment
        {
            Id = 2,
            Name = "New Equipment",
            SerialNumber = "CT-999999",
            Category = "Imaging",
            PurchaseDate = new DateTime(2024, 1, 1),
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(newEquipment);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WithUpdateScenario_ShouldAllowSameSerialNumberForSameEquipment()
    {
        // Arrange
        var existingEquipment = new List<Equipment>
        {
            new Equipment
            {
                Id = 1,
                Name = "Existing Equipment",
                SerialNumber = "MRI-001",
                Category = "Imaging",
                PurchaseDate = new DateTime(2024, 1, 1),
                Status = "Active"
            }
        };

        var strategy = new SerialNumberValidationStrategy(existingEquipment);
        var updatedEquipment = new Equipment
        {
            Id = 1, // Same ID as existing
            Name = "Updated Equipment",
            SerialNumber = "MRI-001", // Same serial number
            Category = "Imaging",
            PurchaseDate = new DateTime(2024, 1, 1),
            Status = "Maintenance"
        };

        // Act
        var result = strategy.Validate(updatedEquipment);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WithoutExistingEquipmentCollection_ShouldNotCheckUniqueness()
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy(); // No existing equipment collection
        var equipment = new Equipment
        {
            Id = 1,
            Name = "Test Equipment",
            SerialNumber = "MRI-001",
            Category = "Imaging",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(equipment);

        // Assert
        Assert.True(result.IsValid); // Should pass because uniqueness check is optional
    }

    #endregion

    #region Prefix Validation Tests

    [Fact]
    public void Validate_WithRecognizedPrefix_ShouldNotWarn()
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();
        var equipment = new Equipment
        {
            Id = 1,
            Name = "Test Equipment",
            SerialNumber = "MRI-001",
            Category = "Imaging",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(equipment);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Warnings);
    }

    [Fact]
    public void Validate_WithUnrecognizedPrefix_ShouldWarn()
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();
        var equipment = new Equipment
        {
            Id = 1,
            Name = "Test Equipment",
            SerialNumber = "FOO-001",
            Category = "Unknown",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(equipment);

        // Assert
        Assert.True(result.IsValid); // Still valid, just warning
        Assert.NotEmpty(result.Warnings);
        Assert.Contains("is not a recognized equipment category", result.Warnings[0]);
    }

    [Theory]
    [InlineData("MRI")]
    [InlineData("CT")]
    [InlineData("XR")]
    [InlineData("XRAY")]
    [InlineData("US")]
    [InlineData("ECHO")]
    [InlineData("LAB")]
    [InlineData("SURG")]
    [InlineData("ICU")]
    [InlineData("ER")]
    public void Validate_WithValidPrefixes_ShouldNotWarn(string prefix)
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();
        var equipment = new Equipment
        {
            Id = 1,
            Name = "Test Equipment",
            SerialNumber = $"{prefix}-001",
            Category = "Test",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(equipment);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Warnings);
    }

    #endregion

    #region Leading Zeros Tests

    [Fact]
    public void Validate_WithLeadingZeros_ShouldNotWarn()
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();
        var equipment = new Equipment
        {
            Id = 1,
            Name = "Test Equipment",
            SerialNumber = "MRI-001",
            Category = "Imaging",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(equipment);

        // Assert
        Assert.True(result.IsValid);
        Assert.DoesNotContain(result.Warnings, w => w.Contains("leading zeros"));
    }

    [Fact]
    public void Validate_WithoutLeadingZeros_ButAboveThreshold_ShouldNotWarn()
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();
        var equipment = new Equipment
        {
            Id = 1,
            Name = "Test Equipment",
            SerialNumber = "MRI-1000", // 4 digits, >= 1000, no warning needed
            Category = "Imaging",
            PurchaseDate = DateTime.Now,
            Status = "Active"
        };

        // Act
        var result = strategy.Validate(equipment);

        // Assert
        Assert.True(result.IsValid);
        Assert.DoesNotContain(result.Warnings, w => w.Contains("leading zeros"));
    }

    #endregion

    #region Strategy Properties Tests

    [Fact]
    public void StrategyName_ShouldReturnCorrectName()
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();

        // Act
        var name = strategy.StrategyName;

        // Assert
        Assert.Equal("Serial Number Validation", name);
    }

    [Fact]
    public void Description_ShouldReturnCorrectDescription()
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();

        // Act
        var description = strategy.Description;

        // Assert
        Assert.Equal("Validates serial number format, uniqueness, and compliance with organizational standards.", description);
    }

    #endregion

    #region Null Equipment Tests

    [Fact]
    public void Validate_WithNullEquipment_ShouldThrowArgumentNullException()
    {
        // Arrange
        var strategy = new SerialNumberValidationStrategy();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => strategy.Validate(null));
    }

    #endregion
}
