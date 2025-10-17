using EquipmentMaintenanceTracker.Validation;

namespace EquipmentMaintenanceTracker.Tests.Validation;

public class ValidationResultTests
{
    [Fact]
    public void Constructor_Default_ShouldInitializeWithValidState()
    {
        var result = new ValidationResult();

        Assert.True(result.IsValid);
        Assert.NotNull(result.Errors);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Warnings);
        Assert.Empty(result.Warnings);

        //
    }

    [Fact]
    public void IsValid_DefaultValue_ShouldBeTrue()
    {
        var result = new ValidationResult();

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Errors_DefaultValue_ShouldBeEmptyList()
    {
        var result = new ValidationResult();

        Assert.NotNull(result.Errors);
        Assert.Empty(result.Errors);
        Assert.IsType<List<string>>(result.Errors);
    }

    [Fact]
    public void Warnings_DefaultValue_ShouldBeEmptyList()
    {
        var result = new ValidationResult();

        Assert.NotNull(result.Warnings);
        Assert.Empty(result.Warnings);
        Assert.IsType<List<string>>(result.Warnings);
    }

    [Fact]
    public void AddError_WithValidMessage_ShouldAddErrorAndSetIsValidToFalse()
    {
        var result = new ValidationResult();
        var errorMessage = "Test error message";

        result.AddError(errorMessage);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(errorMessage, result.Errors);
    }

    [Fact]
    public void AddError_WithNullMessage_ShouldAddNullToErrors()
    {
        var result = new ValidationResult();

        result.AddError(null);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(null, result.Errors);
    }

    [Fact]
    public void AddError_WithEmptyMessage_ShouldAddEmptyStringToErrors()
    {
        var result = new ValidationResult();

        result.AddError(string.Empty);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(string.Empty, result.Errors);
    }

    [Fact]
    public void AddError_WithWhitespaceMessage_ShouldAddWhitespaceToErrors()
    {
        var result = new ValidationResult();
        var whitespaceMessage = "   ";

        result.AddError(whitespaceMessage);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(whitespaceMessage, result.Errors);
    }

    [Fact]
    public void AddError_MultipleErrors_ShouldAddAllErrorsAndKeepIsValidFalse()
    {
        var result = new ValidationResult();
        var error1 = "First error";
        var error2 = "Second error";
        var error3 = "Third error";

        result.AddError(error1);
        result.AddError(error2);
        result.AddError(error3);

        Assert.False(result.IsValid);
        Assert.Equal(3, result.Errors.Count);
        Assert.Contains(error1, result.Errors);
        Assert.Contains(error2, result.Errors);
        Assert.Contains(error3, result.Errors);
    }

    [Fact]
    public void AddWarning_WithValidMessage_ShouldAddWarningAndKeepIsValidTrue()
    {
        var result = new ValidationResult();
        var warningMessage = "Test warning message";

        result.AddWarning(warningMessage);

        Assert.True(result.IsValid);
        Assert.Single(result.Warnings);
        Assert.Contains(warningMessage, result.Warnings);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void AddWarning_WithNullMessage_ShouldAddNullToWarnings()
    {
        var result = new ValidationResult();

        result.AddWarning(null);

        Assert.True(result.IsValid);
        Assert.Single(result.Warnings);
        Assert.Contains(null, result.Warnings);
    }

    [Fact]
    public void AddWarning_WithEmptyMessage_ShouldAddEmptyStringToWarnings()
    {
        var result = new ValidationResult();

        result.AddWarning(string.Empty);

        Assert.True(result.IsValid);
        Assert.Single(result.Warnings);
        Assert.Contains(string.Empty, result.Warnings);
    }

    [Fact]
    public void AddWarning_MultipleWarnings_ShouldAddAllWarningsAndKeepIsValidTrue()
    {
        var result = new ValidationResult();
        var warning1 = "First warning";
        var warning2 = "Second warning";

        result.AddWarning(warning1);
        result.AddWarning(warning2);

        Assert.True(result.IsValid);
        Assert.Equal(2, result.Warnings.Count);
        Assert.Contains(warning1, result.Warnings);
        Assert.Contains(warning2, result.Warnings);
    }

    [Fact]
    public void AddWarning_AfterAddingError_ShouldKeepIsValidFalse()
    {
        var result = new ValidationResult();

        result.AddError("Error message");
        result.AddWarning("Warning message");

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Single(result.Warnings);
    }

    [Fact]
    public void Combine_WithTwoValidResults_ShouldReturnValidCombinedResult()
    {
        var result1 = new ValidationResult();
        var result2 = new ValidationResult();
        result1.AddWarning("Warning 1");
        result2.AddWarning("Warning 2");

        var combined = result1.Combine(result2);

        Assert.True(combined.IsValid);
        Assert.Empty(combined.Errors);
        Assert.Equal(2, combined.Warnings.Count);
        Assert.Contains("Warning 1", combined.Warnings);
        Assert.Contains("Warning 2", combined.Warnings);
    }

    [Fact]
    public void Combine_WithOneInvalidResult_ShouldReturnInvalidCombinedResult()
    {
        var result1 = new ValidationResult();
        var result2 = new ValidationResult();
        result1.AddError("Error 1");
        result2.AddWarning("Warning 1");

        var combined = result1.Combine(result2);

        Assert.False(combined.IsValid);
        Assert.Single(combined.Errors);
        Assert.Single(combined.Warnings);
        Assert.Contains("Error 1", combined.Errors);
        Assert.Contains("Warning 1", combined.Warnings);
    }

    [Fact]
    public void Combine_WithBothInvalidResults_ShouldReturnInvalidCombinedResult()
    {
        var result1 = new ValidationResult();
        var result2 = new ValidationResult();
        result1.AddError("Error 1");
        result2.AddError("Error 2");

        var combined = result1.Combine(result2);

        Assert.False(combined.IsValid);
        Assert.Equal(2, combined.Errors.Count);
        Assert.Contains("Error 1", combined.Errors);
        Assert.Contains("Error 2", combined.Errors);
    }

    [Fact]
    public void Combine_WithNullResult_ShouldThrowNullReferenceException()
    {
        var result = new ValidationResult();

        Assert.Throws<NullReferenceException>(() => result.Combine(null));
    }

    [Fact]
    public void Combine_WithEmptyResults_ShouldReturnValidEmptyResult()
    {
        var result1 = new ValidationResult();
        var result2 = new ValidationResult();

        var combined = result1.Combine(result2);

        Assert.True(combined.IsValid);
        Assert.Empty(combined.Errors);
        Assert.Empty(combined.Warnings);
    }

    [Fact]
    public void Combine_ShouldNotModifyOriginalResults()
    {
        var result1 = new ValidationResult();
        var result2 = new ValidationResult();
        result1.AddError("Error 1");
        result2.AddWarning("Warning 1");

        var combined = result1.Combine(result2);
        combined.AddError("New error");

        Assert.Single(result1.Errors);
        Assert.Empty(result2.Errors);
        Assert.Equal(2, combined.Errors.Count);
    }

    [Fact]
    public void GetFormattedMessages_WithNoErrorsOrWarnings_ShouldReturnEmptyString()
    {
        var result = new ValidationResult();

        var formatted = result.GetFormattedMessages();

        Assert.Equal(string.Empty, formatted);
    }

    [Fact]
    public void GetFormattedMessages_WithErrorsOnly_ShouldFormatErrorsCorrectly()
    {
        var result = new ValidationResult();
        result.AddError("First error");
        result.AddError("Second error");

        var formatted = result.GetFormattedMessages();

        var expected = $"Errors:{Environment.NewLine}  - First error{Environment.NewLine}  - Second error";
        Assert.Equal(expected, formatted);
    }

    [Fact]
    public void GetFormattedMessages_WithWarningsOnly_ShouldFormatWarningsCorrectly()
    {
        var result = new ValidationResult();
        result.AddWarning("First warning");
        result.AddWarning("Second warning");

        var formatted = result.GetFormattedMessages();

        var expected = $"Warnings:{Environment.NewLine}  - First warning{Environment.NewLine}  - Second warning";
        Assert.Equal(expected, formatted);
    }

    [Fact]
    public void GetFormattedMessages_WithErrorsAndWarnings_ShouldFormatBothCorrectly()
    {
        var result = new ValidationResult();
        result.AddError("Test error");
        result.AddWarning("Test warning");

        var formatted = result.GetFormattedMessages();

        var expected = $"Errors:{Environment.NewLine}  - Test error{Environment.NewLine}Warnings:{Environment.NewLine}  - Test warning";
        Assert.Equal(expected, formatted);
    }

    [Fact]
    public void GetFormattedMessages_WithNullErrorMessage_ShouldHandleNullGracefully()
    {
        var result = new ValidationResult();
        result.AddError(null);

        var formatted = result.GetFormattedMessages();

        var expected = $"Errors:{Environment.NewLine}  - ";
        Assert.Equal(expected, formatted);
    }

    [Fact]
    public void GetFormattedMessages_WithEmptyErrorMessage_ShouldHandleEmptyStringGracefully()
    {
        var result = new ValidationResult();
        result.AddError(string.Empty);

        var formatted = result.GetFormattedMessages();

        var expected = $"Errors:{Environment.NewLine}  - ";
        Assert.Equal(expected, formatted);
    }

    [Fact]
    public void GetFormattedMessages_WithSpecialCharacters_ShouldPreserveSpecialCharacters()
    {
        var result = new ValidationResult();
        result.AddError("Error with special chars: !@#$%^&*()");
        result.AddWarning("Warning with unicode: ñáéíóú");

        var formatted = result.GetFormattedMessages();

        Assert.Contains("Error with special chars: !@#$%^&*()", formatted);
        Assert.Contains("Warning with unicode: ñáéíóú", formatted);
    }

    [Fact]
    public void GetFormattedMessages_WithLongMessages_ShouldPreserveFullLength()
    {
        var result = new ValidationResult();
        var longError = new string('E', 1000);
        var longWarning = new string('W', 1000);
        result.AddError(longError);
        result.AddWarning(longWarning);

        var formatted = result.GetFormattedMessages();

        Assert.Contains(longError, formatted);
        Assert.Contains(longWarning, formatted);
    }

    [Fact]
    public void IsValid_SetDirectly_ShouldUpdateValue()
    {
        var result = new ValidationResult();
        
        result.IsValid = false;

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Errors_InitProperty_ShouldAllowInitialization()
    {
        var errors = new List<string> { "Pre-existing error" };
        var result = new ValidationResult { Errors = { } };

        result.Errors.Add("New error");

        Assert.Single(result.Errors);
        Assert.Contains("New error", result.Errors);
    }

    [Fact]
    public void Warnings_InitProperty_ShouldAllowInitialization()
    {
        var warnings = new List<string> { "Pre-existing warning" };
        var result = new ValidationResult { Warnings = { } };

        result.Warnings.Add("New warning");

        Assert.Single(result.Warnings);
        Assert.Contains("New warning", result.Warnings);
    }
}