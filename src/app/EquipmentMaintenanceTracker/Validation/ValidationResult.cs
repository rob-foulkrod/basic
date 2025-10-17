namespace EquipmentMaintenanceTracker.Validation;

/// <summary>
/// Represents the result of a validation operation, containing success status and error messages.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Gets or sets whether the validation passed successfully.
    /// </summary>
    public bool IsValid { get; set; } = true;
    
    /// <summary>
    /// Gets the list of validation error messages. Empty if validation passed.
    /// </summary>
    public List<string> Errors { get; init; } = new();
    
    /// <summary>
    /// Gets the list of validation warning messages that don't prevent success.
    /// </summary>
    public List<string> Warnings { get; init; } = new();
    
    /// <summary>
    /// Adds an error message to the result and marks validation as failed.
    /// </summary>
    /// <param name="errorMessage">The error message to add.</param>
    public void AddError(string errorMessage)
    {
        Errors.Add(errorMessage);
        IsValid = false;
    }
    
    /// <summary>
    /// Adds a warning message to the result without affecting validation success.
    /// </summary>
    /// <param name="warningMessage">The warning message to add.</param>
    public void AddWarning(string warningMessage)
    {
        Warnings.Add(warningMessage);
    }
    
    /// <summary>
    /// Combines this validation result with another, merging errors and warnings.
    /// </summary>
    /// <param name="other">The other validation result to merge.</param>
    /// <returns>A new ValidationResult containing combined results.</returns>
    public ValidationResult Combine(ValidationResult other)
    {
        var combined = new ValidationResult
        {
            IsValid = IsValid && other.IsValid
        };
        
        combined.Errors.AddRange(Errors);
        combined.Errors.AddRange(other.Errors);
        combined.Warnings.AddRange(Warnings);
        combined.Warnings.AddRange(other.Warnings);
        
        return combined;
    }
    
    /// <summary>
    /// Gets a formatted string representation of all errors and warnings.
    /// </summary>
    /// <returns>Formatted validation messages.</returns>
    public string GetFormattedMessages()
    {
        var messages = new List<string>();
        
        if (Errors.Any())
        {
            messages.Add("Errors:");
            messages.AddRange(Errors.Select(e => $"  - {e}"));
        }
        
        if (Warnings.Any())
        {
            messages.Add("Warnings:");
            messages.AddRange(Warnings.Select(w => $"  - {w}"));
        }
        
        return string.Join(Environment.NewLine, messages);
    }
}