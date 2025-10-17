using EquipmentMaintenanceTracker.Models;

namespace EquipmentMaintenanceTracker.Validation;

/// <summary>
/// Manages and executes multiple validation strategies for equipment validation.
/// This class implements the Strategy pattern to allow flexible, composable validation logic.
/// </summary>
public class ValidationContext : IValidationContext
{
    private readonly Dictionary<string, IValidationStrategy> _strategies = new();

    /// <summary>
    /// Registers a validation strategy with the specified name.
    /// </summary>
    /// <param name="name">The unique name to identify this strategy.</param>
    /// <param name="strategy">The validation strategy implementation.</param>
    /// <exception cref="ArgumentNullException">Thrown if name or strategy is null.</exception>
    /// <exception cref="ArgumentException">Thrown if a strategy with the same name is already registered.</exception>
    public void RegisterStrategy(string name, IValidationStrategy strategy)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(strategy, nameof(strategy));
        
        if (_strategies.ContainsKey(name))
        {
            throw new ArgumentException($"A strategy with the name '{name}' is already registered.", nameof(name));
        }
        
        _strategies[name] = strategy;
    }

    /// <summary>
    /// Unregisters a validation strategy by name.
    /// </summary>
    /// <param name="name">The name of the strategy to remove.</param>
    /// <returns>True if the strategy was found and removed, false otherwise.</returns>
    public bool UnregisterStrategy(string name)
    {
        return _strategies.Remove(name);
    }

    /// <summary>
    /// Gets all registered strategy names.
    /// </summary>
    /// <returns>A collection of registered strategy names.</returns>
    public IEnumerable<string> GetRegisteredStrategyNames()
    {
        return _strategies.Keys.ToList();
    }

    /// <summary>
    /// Checks if a strategy with the specified name is registered.
    /// </summary>
    /// <param name="strategyName">The name of the strategy to check.</param>
    /// <returns>True if the strategy is registered, false otherwise.</returns>
    public bool HasStrategy(string strategyName)
    {
        return _strategies.ContainsKey(strategyName);
    }

    /// <summary>
    /// Validates equipment using multiple strategies and combines the results.
    /// If no strategy names are provided, all registered strategies are used.
    /// </summary>
    /// <param name="equipment">The equipment to validate.</param>
    /// <param name="strategyNames">The names of strategies to apply. If empty, all strategies are used.</param>
    /// <returns>A combined validation result from all specified strategies.</returns>
    /// <exception cref="ArgumentNullException">Thrown if equipment is null.</exception>
    /// <exception cref="ArgumentException">Thrown if a requested strategy name is not registered.</exception>
    public ValidationResult Validate(Equipment equipment, params string[] strategyNames)
    {
        ArgumentNullException.ThrowIfNull(equipment, nameof(equipment));
        
        // Determine which strategies to use
        var strategiesToUse = strategyNames.Length == 0 
            ? _strategies.Values.ToList() 
            : strategyNames.Select(name =>
            {
                if (!_strategies.TryGetValue(name, out var strategy))
                {
                    throw new ArgumentException($"Validation strategy '{name}' is not registered.", nameof(strategyNames));
                }
                return strategy;
            }).ToList();
        
        // Execute all strategies and combine results
        ValidationResult? combinedResult = null;
        foreach (var strategy in strategiesToUse)
        {
            var result = strategy.Validate(equipment);
            combinedResult = combinedResult == null ? result : combinedResult.Combine(result);
        }
        
        return combinedResult ?? new ValidationResult();
    }
}
