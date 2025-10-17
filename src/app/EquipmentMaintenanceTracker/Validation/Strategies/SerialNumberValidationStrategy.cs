using EquipmentMaintenanceTracker.Models;
using System.Text.RegularExpressions;

namespace EquipmentMaintenanceTracker.Validation.Strategies;

/// <summary>
/// Validates serial number format, uniqueness, and compliance with organizational standards.
/// This strategy ensures serial numbers follow proper formatting rules and are unique within the system.
/// </summary>
public class SerialNumberValidationStrategy : IValidationStrategy
{
    private readonly ICollection<Equipment>? _existingEquipment;
    private static readonly Regex SerialNumberPattern = new(@"^[A-Z]{2,4}-\d{3,6}$", RegexOptions.Compiled);
    
    /// <summary>
    /// Gets the name of this validation strategy.
    /// </summary>
    public string StrategyName => "Serial Number Validation";
    
    /// <summary>
    /// Gets a description of what this validation strategy validates.
    /// </summary>
    public string Description => "Validates serial number format, uniqueness, and compliance with organizational standards.";
    
    /// <summary>
    /// Initializes a new instance of the SerialNumberValidationStrategy class.
    /// </summary>
    /// <param name="existingEquipment">Optional collection of existing equipment to check for serial number uniqueness.</param>
    public SerialNumberValidationStrategy(ICollection<Equipment>? existingEquipment = null)
    {
        _existingEquipment = existingEquipment;
    }
    
    /// <summary>
    /// Validates the specified equipment's serial number according to format and uniqueness rules.
    /// </summary>
    /// <param name="equipment">The equipment to validate.</param>
    /// <returns>A ValidationResult containing validation status and any error messages.</returns>
    /// <exception cref="ArgumentNullException">Thrown when equipment is null.</exception>
    public ValidationResult Validate(Equipment equipment)
    {
        ArgumentNullException.ThrowIfNull(equipment, nameof(equipment));
        
        var result = new ValidationResult();
        
        if (string.IsNullOrWhiteSpace(equipment.SerialNumber))
        {
            result.AddError("Serial number is required for validation.");
            return result;
        }
        
        // Validate format - should be like "MRI-001", "XR-002", etc.
        if (!SerialNumberPattern.IsMatch(equipment.SerialNumber))
        {
            result.AddError($"Serial number '{equipment.SerialNumber}' does not match required format. " +
                           "Expected format: 2-4 uppercase letters followed by dash and 3-6 digits (e.g., 'MRI-001', 'XRAY-12345').");
        }
        
        // Check for uniqueness if existing equipment collection is provided
        if (_existingEquipment != null)
        {
            var duplicateSerial = _existingEquipment
                .Where(e => e.Id != equipment.Id) // Exclude current equipment for updates
                .Any(e => string.Equals(e.SerialNumber, equipment.SerialNumber, StringComparison.OrdinalIgnoreCase));
            
            if (duplicateSerial)
            {
                result.AddError($"Serial number '{equipment.SerialNumber}' is already in use by another equipment item.");
            }
        }
        
        // Validate serial number components
        if (SerialNumberPattern.IsMatch(equipment.SerialNumber))
        {
            var parts = equipment.SerialNumber.Split('-');
            var prefix = parts[0];
            var number = parts[1];
            
            // Check if prefix matches common equipment categories
            var validPrefixes = new[] { "MRI", "CT", "XR", "XRAY", "US", "ECHO", "LAB", "SURG", "ICU", "ER" };
            if (!validPrefixes.Contains(prefix))
            {
                result.AddWarning($"Serial number prefix '{prefix}' is not a recognized equipment category. " +
                                "Consider using standard prefixes like MRI, CT, XR, US, etc.");
            }
            
            // Check if number is sequential (starts with leading zeros)
            if (number.Length > 3 && !number.StartsWith('0') && int.Parse(number) < 1000)
            {
                result.AddWarning("Consider using leading zeros in serial numbers for better organization (e.g., '001' instead of '1').");
            }
        }
        
        return result;
    }
}