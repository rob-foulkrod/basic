using EquipmentMaintenanceTracker.Models;

namespace EquipmentMaintenanceTracker.Services;

/// <summary>
/// Provides services for managing equipment and maintenance records in the equipment maintenance tracking system.
/// This service maintains in-memory collections of equipment and maintenance records with automatic ID assignment.
/// </summary>
/// <remarks>
/// This service is not thread-safe. If used in a multi-threaded environment, external synchronization is required.
/// Data is stored in memory and will be lost when the application terminates.
/// </remarks>
public class EquipmentService
{
    private readonly List<Equipment> _equipments = new();
    private readonly List<MaintenanceRecord> _maintenanceRecords = new();
    private int _nextEquipmentId = 1;
    private int _nextMaintenanceId = 1;

    /// <summary>
    /// Initializes a new instance of the EquipmentService class and populates it with sample data.
    /// </summary>
    public EquipmentService()
    {
        SeedData();
    }

    private void SeedData()
    {
        AddEquipment(new Equipment
        {
            Name = "MRI Scanner",
            SerialNumber = "MRI-001",
            Category = "Imaging",
            PurchaseDate = new DateTime(2020, 5, 15),
            Status = "Active"
        });

        AddEquipment(new Equipment
        {
            Name = "X-Ray Machine",
            SerialNumber = "XR-002",
            Category = "Imaging",
            PurchaseDate = new DateTime(2019, 3, 10),
            Status = "Active"
        });
    }

    /// <summary>
    /// Adds the specified equipment to the service's internal collection and assigns it a unique identifier.
    /// </summary>
    /// <param name="equipment">The equipment to add. This instance will have its <c>Id</c> property set by the method.</param>
    /// <remarks>
    /// The method sets <c>equipment.Id</c> using the service's internal <c>_nextEquipmentId</c> counter (post-increment)
    /// and then appends the equipment to the internal <c>_equipments</c> collection. The passed instance is mutated
    /// by this call (its <c>Id</c> is changed). This method is not guaranteed to be thread-safe; callers should
    /// synchronize access if multiple threads may call it concurrently.
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="equipment"/> is <c>null</c>.</exception>
    public void AddEquipment(Equipment equipment)
    {
        // Validate that equipment is not null to provide clear error message
        if (equipment == null)
        {
            throw new ArgumentNullException(nameof(equipment), "Equipment cannot be null.");
        }

        equipment.Id = _nextEquipmentId++;
        _equipments.Add(equipment);
    }

    /// <summary>
    /// Retrieves all equipment in the system.
    /// </summary>
    /// <returns>A list containing all equipment records. Returns an empty list if no equipment exists.</returns>
    /// <example>
    /// <code>
    /// var service = new EquipmentService();
    /// var allEquipment = service.GetAllEquipment();
    /// Console.WriteLine($"Total equipment count: {allEquipment.Count}");
    /// </code>
    /// </example>
    public List<Equipment> GetAllEquipment()
    {
        return _equipments;
    }

    /// <summary>
    /// Retrieves equipment by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the equipment to retrieve.</param>
    /// <returns>The equipment with the specified ID, or <c>null</c> if no equipment is found.</returns>
    /// <example>
    /// <code>
    /// var service = new EquipmentService();
    /// var equipment = service.GetEquipmentById(1);
    /// if (equipment != null)
    /// {
    ///     Console.WriteLine($"Found equipment: {equipment.Name}");
    /// }
    /// else
    /// {
    ///     Console.WriteLine("Equipment not found");
    /// }
    /// </code>
    /// </example>
    public Equipment? GetEquipmentById(int id)
    {
        return _equipments.FirstOrDefault(e => e.Id == id);
    }

    /// <summary>
    /// Updates an existing equipment record with new information.
    /// </summary>
    /// <param name="equipment">The equipment with updated information. The <c>Id</c> property is used to locate the existing record.</param>
    /// <remarks>
    /// If no equipment with the specified <c>Id</c> exists, the method performs no operation.
    /// Only the following properties are updated: Name, SerialNumber, Category, PurchaseDate, and Status.
    /// </remarks>
    /// <example>
    /// <code>
    /// var service = new EquipmentService();
    /// var equipment = service.GetEquipmentById(1);
    /// if (equipment != null)
    /// {
    ///     equipment.Status = "Maintenance";
    ///     service.UpdateEquipment(equipment);
    /// }
    /// </code>
    /// </example>
    public void UpdateEquipment(Equipment equipment)
    {
        var existing = GetEquipmentById(equipment.Id);
        if (existing != null)
        {
            existing.Name = equipment.Name;
            existing.SerialNumber = equipment.SerialNumber;
            existing.Category = equipment.Category;
            existing.PurchaseDate = equipment.PurchaseDate;
            existing.Status = equipment.Status;
        }
    }

    /// <summary>
    /// Deletes equipment and all associated maintenance records from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the equipment to delete.</param>
    /// <remarks>
    /// This operation cascades to delete all maintenance records associated with the equipment.
    /// If no equipment with the specified ID exists, the method performs no operation.
    /// </remarks>
    /// <example>
    /// <code>
    /// var service = new EquipmentService();
    /// service.DeleteEquipment(1);
    /// // Equipment with ID 1 and all its maintenance records are now deleted
    /// </code>
    /// </example>
    public void DeleteEquipment(int id)
    {
        var equipment = GetEquipmentById(id);
        if (equipment != null)
        {
            _equipments.Remove(equipment);
            _maintenanceRecords.RemoveAll(m => m.EquipmentId == id);
        }
    }

    /// <summary>
    /// Adds a maintenance record to the system and assigns it a unique identifier.
    /// </summary>
    /// <param name="record">The maintenance record to add. This instance will have its <c>Id</c> property set by the method.</param>
    /// <remarks>
    /// The method sets <c>record.Id</c> using the service's internal counter and adds the record to the collection.
    /// The passed instance is mutated by this call (its <c>Id</c> is changed).
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="record"/> is <c>null</c>.</exception>
    /// <example>
    /// <code>
    /// var service = new EquipmentService();
    /// var maintenanceRecord = new MaintenanceRecord
    /// {
    ///     EquipmentId = 1,
    ///     MaintenanceDate = DateTime.Now,
    ///     MaintenanceType = "Preventive",
    ///     Description = "Regular calibration",
    ///     TechnicianName = "John Doe"
    /// };
    /// service.AddMaintenanceRecord(maintenanceRecord);
    /// // maintenanceRecord.Id is now set to a unique value
    /// </code>
    /// </example>
    public void AddMaintenanceRecord(MaintenanceRecord record)
    {
        // Validate that record is not null to provide clear error message
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record), "Maintenance record cannot be null.");
        }

        record.Id = _nextMaintenanceId++;
        _maintenanceRecords.Add(record);
    }

    /// <summary>
    /// Retrieves all maintenance records for a specific piece of equipment.
    /// </summary>
    /// <param name="equipmentId">The unique identifier of the equipment whose maintenance records to retrieve.</param>
    /// <returns>A list of maintenance records for the specified equipment. Returns an empty list if no records exist.</returns>
    /// <example>
    /// <code>
    /// var service = new EquipmentService();
    /// var records = service.GetMaintenanceRecordsByEquipmentId(1);
    /// Console.WriteLine($"Equipment 1 has {records.Count} maintenance records");
    /// foreach (var record in records)
    /// {
    ///     Console.WriteLine($"- {record.MaintenanceDate}: {record.Description}");
    /// }
    /// </code>
    /// </example>
    public List<MaintenanceRecord> GetMaintenanceRecordsByEquipmentId(int equipmentId)
    {
        return _maintenanceRecords.Where(m => m.EquipmentId == equipmentId).ToList();
    }

    /// <summary>
    /// Retrieves all maintenance records in the system.
    /// </summary>
    /// <returns>A list containing all maintenance records. Returns an empty list if no records exist.</returns>
    /// <example>
    /// <code>
    /// var service = new EquipmentService();
    /// var allRecords = service.GetAllMaintenanceRecords();
    /// Console.WriteLine($"Total maintenance records: {allRecords.Count}");
    /// </code>
    /// </example>
    public List<MaintenanceRecord> GetAllMaintenanceRecords()
    {
        return _maintenanceRecords;
    }
}
