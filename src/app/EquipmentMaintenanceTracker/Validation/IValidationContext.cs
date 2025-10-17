using EquipmentMaintenanceTracker.Models;

namespace EquipmentMaintenanceTracker.Validation;

/// <summary>
/// Defines a contract for validation context managers that can dynamically
/// select and execute different validation strategies.
/// </summary>
public interface IValidationContext
{
    /// <summary>
    /// Registers a validation strategy with the specified name.
    /// </summary>
    /// <param name="name">The unique name to identify this strategy.</param>
    /// <param name="strategy">The validation strategy implementation.</param>
    void RegisterStrategy(string name, IValidationStrategy strategy);
    
    /// <summary>
    /// Unregisters a validation strategy by name.
    /// </summary>
    /// <param name="name">The name of the strategy to remove.</param>
    /// <returns>True if the strategy was found and removed, false otherwise.</returns>
    bool UnregisterStrategy(string name);
    
    /// <summary>
    /// Gets all registered strategy names.
    /// </summary>
    /// <returns>A collection of registered strategy names.</returns>
    IEnumerable<string> GetRegisteredStrategyNames();
    
    /// <summary>
    /// Validates equipment using multiple strategies and combines the results.
    /// </summary>
    /// <param name="equipment">The equipment to validate.</param>
    /// <param name="strategyNames">The names of strategies to apply.</param>
    /// <returns>A combined validation result from all specified strategies.</returns>
    ValidationResult Validate(Equipment equipment, params string[] strategyNames);
    
    /// <summary>
    /// Checks if a strategy with the specified name is registered.
    /// </summary>
    /// <param name="strategyName">The name of the strategy to check.</param>
    /// <returns>True if the strategy is registered, false otherwise.</returns>
    bool HasStrategy(string strategyName);
}