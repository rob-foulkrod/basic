using EquipmentMaintenanceTracker.Models;

namespace EquipmentMaintenanceTracker.Validation.Strategies;

/// <summary>
/// Validates equipment records against business-specific rules and organizational policies.
/// This strategy enforces business logic constraints such as valid status values, purchase date rules, and category standards.
/// </summary>
public class BusinessRulesValidationStrategy : IValidationStrategy
{
    private static readonly string[] ValidStatuses = { "Active", "Inactive", "Maintenance", "Retired", "Out of Service" };
    private static readonly string[] ValidCategories = { "Imaging", "Laboratory", "Surgical", "Monitoring", "Support", "Emergency", "Rehabilitation" };
    
    /// <summary>
    /// Gets the name of this validation strategy.
    /// </summary>
    public string StrategyName => "Business Rules Validation";
    
    /// <summary>
    /// Gets a description of what this validation strategy validates.
    /// </summary>
    public string Description => "Validates equipment records against business-specific rules, organizational policies, and operational constraints.";
    
    /// <summary>
    /// Validates the specified equipment according to business rules and organizational policies.
    /// </summary>
    /// <param name="equipment">The equipment to validate.</param>
    /// <returns>A ValidationResult containing validation status and any error messages.</returns>
    /// <exception cref="ArgumentNullException">Thrown when equipment is null.</exception>
    public ValidationResult Validate(Equipment equipment)
    {
        ArgumentNullException.ThrowIfNull(equipment, nameof(equipment));
        
        var result = new ValidationResult();
        
        // Validate Status against business rules
        ValidateStatus(equipment, result);
        
        // Validate Category against organizational standards
        ValidateCategory(equipment, result);
        
        // Validate Purchase Date business rules
        ValidatePurchaseDate(equipment, result);
        
        // Validate Equipment Lifecycle Rules
        ValidateEquipmentLifecycle(equipment, result);
        
        return result;
    }
    
    /// <summary>
    /// Validates equipment status against allowed business values.
    /// </summary>
    /// <param name="equipment">The equipment to validate.</param>
    /// <param name="result">The validation result to add errors/warnings to.</param>
    private static void ValidateStatus(Equipment equipment, ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(equipment.Status))
        {
            return; // Basic validation should catch this
        }
        
        if (!ValidStatuses.Contains(equipment.Status, StringComparer.OrdinalIgnoreCase))
        {
            result.AddError($"Status '{equipment.Status}' is not valid. Allowed values are: {string.Join(", ", ValidStatuses)}.");
        }
        
        // Business rule: New equipment should start as "Active"
        if (equipment.Id == 0 && !string.Equals(equipment.Status, "Active", StringComparison.OrdinalIgnoreCase))
        {
            result.AddWarning("New equipment items should typically start with 'Active' status unless there's a specific reason.");
        }
    }
    
    /// <summary>
    /// Validates equipment category against organizational standards.
    /// </summary>
    /// <param name="equipment">The equipment to validate.</param>
    /// <param name="result">The validation result to add errors/warnings to.</param>
    private static void ValidateCategory(Equipment equipment, ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(equipment.Category))
        {
            return; // Basic validation should catch this
        }
        
        if (!ValidCategories.Contains(equipment.Category, StringComparer.OrdinalIgnoreCase))
        {
            result.AddWarning($"Category '{equipment.Category}' is not in the standard list. " +
                            $"Consider using one of: {string.Join(", ", ValidCategories)}.");
        }
    }
    
    /// <summary>
    /// Validates purchase date against business rules and constraints.
    /// </summary>
    /// <param name="equipment">The equipment to validate.</param>
    /// <param name="result">The validation result to add errors/warnings to.</param>
    private static void ValidatePurchaseDate(Equipment equipment, ValidationResult result)
    {
        var now = DateTime.Now;
        var minDate = new DateTime(1990, 1, 1); // Reasonable minimum for medical equipment
        
        if (equipment.PurchaseDate == default)
        {
            return; // Basic validation should catch this
        }
        
        // Purchase date cannot be in the future
        if (equipment.PurchaseDate > now)
        {
            result.AddError("Purchase date cannot be in the future.");
        }
        
        // Purchase date should be reasonable (not too old)
        if (equipment.PurchaseDate < minDate)
        {
            result.AddError($"Purchase date cannot be earlier than {minDate:yyyy-MM-dd}. Please verify the date.");
        }
        
        // Warn about very old equipment
        if (equipment.PurchaseDate < now.AddYears(-20))
        {
            result.AddWarning("Equipment is over 20 years old. Consider reviewing maintenance schedules and replacement planning.");
        }
        
        // Warn about very recent purchases that might not be in service yet
        if (equipment.PurchaseDate > now.AddDays(-30) && 
            string.Equals(equipment.Status, "Active", StringComparison.OrdinalIgnoreCase))
        {
            result.AddWarning("Recently purchased equipment marked as 'Active'. Ensure installation and commissioning are complete.");
        }
    }
    
    /// <summary>
    /// Validates equipment lifecycle rules and status consistency.
    /// </summary>
    /// <param name="equipment">The equipment to validate.</param>
    /// <param name="result">The validation result to add errors/warnings to.</param>
    private static void ValidateEquipmentLifecycle(Equipment equipment, ValidationResult result)
    {
        var equipmentAge = DateTime.Now.Year - equipment.PurchaseDate.Year;
        
        // Lifecycle validation based on status
        switch (equipment.Status?.ToLowerInvariant())
        {
            case "retired":
                if (equipmentAge < 5)
                {
                    result.AddWarning("Equipment marked as 'Retired' but is less than 5 years old. Verify retirement reason.");
                }
                break;
                
            case "out of service":
                result.AddWarning("Equipment marked as 'Out of Service'. Ensure maintenance tickets are created for resolution.");
                break;
                
            case "active":
                if (equipmentAge > 15)
                {
                    result.AddWarning("Active equipment is over 15 years old. Consider maintenance review and potential replacement planning.");
                }
                break;
        }
        
        // Category-specific lifecycle rules
        if (string.Equals(equipment.Category, "Imaging", StringComparison.OrdinalIgnoreCase) && equipmentAge > 10)
        {
            result.AddWarning("Imaging equipment over 10 years old may require more frequent calibration and maintenance.");
        }
        
        if (string.Equals(equipment.Category, "Laboratory", StringComparison.OrdinalIgnoreCase) && equipmentAge > 12)
        {
            result.AddWarning("Laboratory equipment over 12 years old should be evaluated for accuracy and compliance standards.");
        }
    }
}