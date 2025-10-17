using EquipmentMaintenanceTracker.Validation;
using Xunit;

namespace EquipmentMaintenanceTracker.Tests.Validation;

/// <summary>
/// Tests for <see cref="ValidationException"/> ensuring all constructors behave as expected.
/// </summary>
public class ValidationExceptionTests
{
    [Fact]
    public void Ctor_WithValidationResult_BuildsMessageFromErrors()
    {
        var result = new ValidationResult();
        result.AddError("Name is required");
        result.AddError("Serial number must be unique");

        var ex = new ValidationException(result);

        Assert.Same(result, ex.ValidationResult);
        Assert.Equal("Validation failed: Name is required, Serial number must be unique", ex.Message);
    }

    [Fact]
    public void Ctor_WithValidationResult_NoErrors_ProducesMessageWithSuffix()
    {
        var result = new ValidationResult(); // no errors added

        var ex = new ValidationException(result);

        // string.Join on empty list yields empty string, so expected trailing space after colon
        Assert.Equal("Validation failed: ", ex.Message);
        Assert.True(result.IsValid); // sanity: no errors means IsValid stays true
    }

    [Fact]
    public void Ctor_WithCustomMessage_SetsMessageAndResult()
    {
        var result = new ValidationResult();
        result.AddError("Id must be positive");

        var ex = new ValidationException("Custom validation failure", result);

        Assert.Same(result, ex.ValidationResult);
        Assert.Equal("Custom validation failure", ex.Message);
    }

    [Fact]
    public void Ctor_WithCustomMessageAndInnerException_PreservesInner()
    {
        var result = new ValidationResult();
        result.AddError("Category is required");
        var inner = new InvalidOperationException("Root cause");

        var ex = new ValidationException("Wrapper message", result, inner);

        Assert.Same(result, ex.ValidationResult);
        Assert.Equal("Wrapper message", ex.Message);
        Assert.Same(inner, ex.InnerException);
    }
}
