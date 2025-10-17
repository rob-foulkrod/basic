using EquipmentMaintenanceTracker.Models;

namespace EquipmentMaintenanceTracker.Validation;

/// <summary>
/// Validates basic required fields and data integrity for equipment records.
/// This strategy ensures that all essential equipment properties are properly populated.
/// </summary>
public class BasicEquipmentValidationStrategy : IValidationStrategy
{
    /// <summary>
    /// Gets the name of this validation strategy.
    /// </summary>
    public string StrategyName => "Basic Equipment Validation";
    
    /// <summary>
    /// Gets a description of what this validation strategy validates.
    /// </summary>
    public string Description => "Validates required fields, data types, and basic integrity constraints for equipment records.";
    
    /// <summary>
    /// Validates the specified equipment according to basic validation rules.
    /// </summary>
    /// <param name="equipment">The equipment to validate.</param>
    /// <returns>A ValidationResult containing validation status and any error messages.</returns>
    /// <exception cref="ArgumentNullException">Thrown when equipment is null.</exception>
    public ValidationResult Validate(Equipment equipment)
    {
        ArgumentNullException.ThrowIfNull(equipment, nameof(equipment));
        
        var result = new ValidationResult();
        
        // Validate Name
        if (string.IsNullOrWhiteSpace(equipment.Name))
        {
            result.AddError("Equipment name is required and cannot be empty.");
        }
        else if (equipment.Name.Length > 100)
        {
            result.AddError("Equipment name cannot exceed 100 characters.");
        }
        else if (equipment.Name.Length < 2)
        {
            result.AddError("Equipment name must be at least 2 characters long.");
        }
        
        // Validate Serial Number
        if (string.IsNullOrWhiteSpace(equipment.SerialNumber))
        {
            result.AddError("Serial number is required and cannot be empty.");
        }
        else if (equipment.SerialNumber.Length > 50)
        {
            result.AddError("Serial number cannot exceed 50 characters.");
        }
        
        // Validate Category
        if (string.IsNullOrWhiteSpace(equipment.Category))
        {
            result.AddError("Equipment category is required and cannot be empty.");
        }
        else if (equipment.Category.Length > 50)
        {
            result.AddError("Equipment category cannot exceed 50 characters.");
        }
        
        // Validate Status
        if (string.IsNullOrWhiteSpace(equipment.Status))
        {
            result.AddError("Equipment status is required and cannot be empty.");
        }
        else if (equipment.Status.Length > 20)
        {
            result.AddError("Equipment status cannot exceed 20 characters.");
        }
        
        // Validate Purchase Date
        if (equipment.PurchaseDate == default)
        {
            result.AddError("Purchase date is required and must be a valid date.");
        }
        
        // Add warnings for potential issues
        if (!string.IsNullOrWhiteSpace(equipment.Name) && char.IsLower(equipment.Name[0]))
        {
            result.AddWarning("Equipment name should start with a capital letter.");
        }
        
        if (!string.IsNullOrWhiteSpace(equipment.SerialNumber) && equipment.SerialNumber.Contains(' '))
        {
            result.AddWarning("Serial number contains spaces, which may cause issues in some systems.");
        }
        
        return result;
    }
}