namespace EquipmentMaintenanceTracker.Validation;

/// <summary>
/// Represents errors that occur during equipment validation.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Gets the validation result that caused this exception.
    /// </summary>
    public ValidationResult ValidationResult { get; }
    
    /// <summary>
    /// Initializes a new instance of the ValidationException class.
    /// </summary>
    /// <param name="validationResult">The validation result containing the errors.</param>
    public ValidationException(ValidationResult validationResult)
        : base($"Validation failed: {string.Join(", ", validationResult.Errors)}")
    {
        ValidationResult = validationResult;
    }
    
    /// <summary>
    /// Initializes a new instance of the ValidationException class with a custom message.
    /// </summary>
    /// <param name="message">The custom error message.</param>
    /// <param name="validationResult">The validation result containing the errors.</param>
    public ValidationException(string message, ValidationResult validationResult)
        : base(message)
    {
        ValidationResult = validationResult;
    }
    
    /// <summary>
    /// Initializes a new instance of the ValidationException class with a custom message and inner exception.
    /// </summary>
    /// <param name="message">The custom error message.</param>
    /// <param name="validationResult">The validation result containing the errors.</param>
    /// <param name="innerException">The inner exception.</param>
    public ValidationException(string message, ValidationResult validationResult, Exception innerException)
        : base(message, innerException)
    {
        ValidationResult = validationResult;
    }
}