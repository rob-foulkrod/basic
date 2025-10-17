using EquipmentMaintenanceTracker.Models;
using EquipmentMaintenanceTracker.Services;

namespace EquipmentMaintenanceTracker.Validation;

/// <summary>
/// Provides a composite validation service that applies multiple validation strategies to equipment records.
/// This service coordinates the execution of different validation strategies and combines their results.
/// </summary>
public class EquipmentValidationService
{
    private readonly List<IValidationStrategy> _strategies;
    private readonly EquipmentService _equipmentService;
    
    /// <summary>
    /// Initializes a new instance of the EquipmentValidationService class.
    /// </summary>
    /// <param name="equipmentService">The equipment service to access existing equipment for uniqueness validation.</param>
    public EquipmentValidationService(EquipmentService equipmentService)
    {
        _equipmentService = equipmentService ?? throw new ArgumentNullException(nameof(equipmentService));
        _strategies = new List<IValidationStrategy>();
        InitializeDefaultStrategies();
    }
    
    /// <summary>
    /// Initializes the default validation strategies.
    /// </summary>
    private void InitializeDefaultStrategies()
    {
        _strategies.Add(new BasicEquipmentValidationStrategy());
        _strategies.Add(new SerialNumberValidationStrategy(_equipmentService.GetAllEquipment()));
        _strategies.Add(new BusinessRulesValidationStrategy());
    }
    
    /// <summary>
    /// Adds a custom validation strategy to the service.
    /// </summary>
    /// <param name="strategy">The validation strategy to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when strategy is null.</exception>
    public void AddStrategy(IValidationStrategy strategy)
    {
        ArgumentNullException.ThrowIfNull(strategy, nameof(strategy));
        _strategies.Add(strategy);
    }
    
    /// <summary>
    /// Removes a validation strategy from the service.
    /// </summary>
    /// <param name="strategyType">The type of strategy to remove.</param>
    /// <returns>True if a strategy was removed; otherwise, false.</returns>
    public bool RemoveStrategy<T>() where T : IValidationStrategy
    {
        var strategy = _strategies.FirstOrDefault(s => s is T);
        if (strategy != null)
        {
            _strategies.Remove(strategy);
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Validates equipment using all configured validation strategies.
    /// </summary>
    /// <param name="equipment">The equipment to validate.</param>
    /// <returns>A combined ValidationResult from all strategies.</returns>
    /// <exception cref="ArgumentNullException">Thrown when equipment is null.</exception>
    public ValidationResult ValidateEquipment(Equipment equipment)
    {
        ArgumentNullException.ThrowIfNull(equipment, nameof(equipment));
        
        var combinedResult = new ValidationResult();
        
        foreach (var strategy in _strategies)
        {
            try
            {
                var strategyResult = strategy.Validate(equipment);
                combinedResult = combinedResult.Combine(strategyResult);
            }
            catch (Exception ex)
            {
                combinedResult.AddError($"Validation strategy '{strategy.StrategyName}' failed: {ex.Message}");
            }
        }
        
        return combinedResult;
    }
    
    /// <summary>
    /// Gets information about all configured validation strategies.
    /// </summary>
    /// <returns>A list of strategy information including names and descriptions.</returns>
    public List<(string Name, string Description)> GetStrategyInfo()
    {
        return _strategies.Select(s => (s.StrategyName, s.Description)).ToList();
    }
    
    /// <summary>
    /// Validates equipment and throws an exception if validation fails.
    /// </summary>
    /// <param name="equipment">The equipment to validate.</param>
    /// <exception cref="ValidationException">Thrown when validation fails.</exception>
    /// <exception cref="ArgumentNullException">Thrown when equipment is null.</exception>
    public void ValidateAndThrow(Equipment equipment)
    {
        var result = ValidateEquipment(equipment);
        
        if (!result.IsValid)
        {
            throw new ValidationException(result);
        }
    }
}