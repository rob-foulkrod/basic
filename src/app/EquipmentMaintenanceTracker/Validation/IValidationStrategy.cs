using EquipmentMaintenanceTracker.Models;

namespace EquipmentMaintenanceTracker.Validation;

/// <summary>
/// Defines a contract for equipment validation strategies.
/// Implementations of this interface can provide different validation rules
/// based on specific scenarios or business requirements.
/// </summary>
public interface IValidationStrategy
{
    /// <summary>
    /// Validates the specified equipment according to the strategy's rules.
    /// </summary>
    /// <param name="equipment">The equipment to validate.</param>
    /// <returns>A ValidationResult containing validation status and any error messages.</returns>
    ValidationResult Validate(Equipment equipment);
    
    /// <summary>
    /// Gets the name of this validation strategy for identification purposes.
    /// </summary>
    string StrategyName { get; }
    
    /// <summary>
    /// Gets a description of what this validation strategy validates.
    /// </summary>
    string Description { get; }
}