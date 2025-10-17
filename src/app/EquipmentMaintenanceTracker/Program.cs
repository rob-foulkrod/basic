using EquipmentMaintenanceTracker.Models;
using EquipmentMaintenanceTracker.Services;
using EquipmentMaintenanceTracker.Validation;
using EquipmentMaintenanceTracker.Validation.Strategies;

namespace EquipmentMaintenanceTracker;

class Program
{
    private static readonly EquipmentService _service = CreateEquipmentService();

    static void Main(string[] args)
    {
        Console.WriteLine("=== Equipment Maintenance Tracker ===\n");
        RunMainMenu();
    }

    /// <summary>
    /// Creates and configures the EquipmentService with validation strategies.
    /// </summary>
    /// <returns>A configured EquipmentService instance.</returns>
    private static EquipmentService CreateEquipmentService()
    {
        var validationContext = new ValidationContext();
        var service = new EquipmentService(validationContext);
        
        // Register validation strategies after service creation so they can access the equipment list
        validationContext.RegisterStrategy("SerialNumber", new SerialNumberValidationStrategy(service.GetAllEquipment()));
        
        return service;
    }

    /// <summary>
    /// Pauses the console application by prompting the user and waiting for the Enter key.
    /// </summary>
    /// <remarks>
    /// Writes "Press enter to continue" to the standard output and blocks execution until the user presses Enter.
    /// Intended for use in console applications to allow the user to read output before the program continues or exits.
    /// </remarks>
    private static void Pause()
    {
        //Pause the terminal
        Console.WriteLine("Press enter to continue");
        Console.ReadLine();

    }

    static void RunMainMenu()
    {
        bool running = true;
        while (running)
        {
            DisplayMenu();
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListAllEquipment();
                    break;
                case "2":
                    AddNewEquipment();
                    break;
                case "3":
                    ViewEquipmentDetails();
                    break;
                case "4":
                    UpdateEquipment();
                    break;
                case "5":
                    DeleteEquipment();
                    break;
                case "6":
                    AddMaintenanceRecord();
                    break;
                case "7":
                    ViewMaintenanceHistory();
                    break;
                case "8":
                    ViewAllMaintenanceRecords();
                    break;
                case "9":
                    running = false;
                    Console.WriteLine("\nThank you for using Equipment Maintenance Tracker!");
                    break;
                default:
                    Console.WriteLine("\nInvalid choice. Please try again.");
                    break;
            }

            if (running)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("\n=== Main Menu ===");
        Console.WriteLine("1) List All Equipment");
        Console.WriteLine("2) Add New Equipment");
        Console.WriteLine("3) View Equipment Details");
        Console.WriteLine("4) Update Equipment");
        Console.WriteLine("5) Delete Equipment");
        Console.WriteLine("6) Add Maintenance Record");
        Console.WriteLine("7) View Maintenance History for Equipment");
        Console.WriteLine("8) View All Maintenance Records");
        Console.WriteLine("9) Exit");
        Console.Write("\nEnter your choice: ");
    }

    static void ListAllEquipment()
    {
        Console.WriteLine("\n=== All Equipment ===");
        var equipments = _service.GetAllEquipment();

        if (equipments.Count == 0)
        {
            Console.WriteLine("No equipment found.");
            return;
        }

        Console.WriteLine($"{"ID",-5} {"Name",-25} {"Serial Number",-15} {"Category",-15} {"Status",-10}");
        Console.WriteLine(new string('-', 75));

        foreach (var equipment in equipments)
        {
            Console.WriteLine($"{equipment.Id,-5} {equipment.Name,-25} {equipment.SerialNumber,-15} {equipment.Category,-15} {equipment.Status,-10}");
        }
    }

    static void AddNewEquipment()
    {
        Console.WriteLine("\n=== Add New Equipment ===");

        Console.Write("Enter equipment name: ");
        var name = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter serial number: ");
        var serialNumber = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter category: ");
        var category = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter purchase date (yyyy-mm-dd): ");
        DateTime purchaseDate;
        while (!DateTime.TryParse(Console.ReadLine(), out purchaseDate))
        {
            Console.Write("Invalid date format. Please enter date (yyyy-mm-dd): ");
        }

        var equipment = new Equipment
        {
            Name = name,
            SerialNumber = serialNumber,
            Category = category,
            PurchaseDate = purchaseDate,
            Status = "Active"
        };

        try
        {
            _service.AddEquipment(equipment);
            Console.WriteLine("\nEquipment added successfully!");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"\n❌ Validation error: {ex.Message}");
        }
    }

    static void ViewEquipmentDetails()
    {
        Console.WriteLine("\n=== View Equipment Details ===");
        Console.Write("Enter equipment ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var equipment = _service.GetEquipmentById(id);
        if (equipment == null)
        {
            Console.WriteLine("Equipment not found.");
            return;
        }

        Console.WriteLine("\n--- Equipment Information ---");
        Console.WriteLine($"ID: {equipment.Id}");
        Console.WriteLine($"Name: {equipment.Name}");
        Console.WriteLine($"Serial Number: {equipment.SerialNumber}");
        Console.WriteLine($"Category: {equipment.Category}");
        Console.WriteLine($"Purchase Date: {equipment.PurchaseDate:yyyy-MM-dd}");
        Console.WriteLine($"Status: {equipment.Status}");
    }

    static void UpdateEquipment()
    {
        Console.WriteLine("\n=== Update Equipment ===");
        Console.Write("Enter equipment ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var equipment = _service.GetEquipmentById(id);
        if (equipment == null)
        {
            Console.WriteLine("Equipment not found.");
            return;
        }

        Console.WriteLine($"\nCurrent Name: {equipment.Name}");
        Console.Write("Enter new name (or press Enter to keep current): ");
        var name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name))
            equipment.Name = name;

        Console.WriteLine($"Current Serial Number: {equipment.SerialNumber}");
        Console.Write("Enter new serial number (or press Enter to keep current): ");
        var serialNumber = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(serialNumber))
            equipment.SerialNumber = serialNumber;

        Console.WriteLine($"Current Category: {equipment.Category}");
        Console.Write("Enter new category (or press Enter to keep current): ");
        var category = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(category))
            equipment.Category = category;

        Console.WriteLine($"Current Status: {equipment.Status}");
        Console.Write("Enter new status (Active/Inactive) (or press Enter to keep current): ");
        var status = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(status))
            equipment.Status = status;

        _service.UpdateEquipment(equipment);
        Console.WriteLine("\nEquipment updated successfully!");
    }

    static void DeleteEquipment()
    {
        Console.WriteLine("\n=== Delete Equipment ===");
        Console.Write("Enter equipment ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var equipment = _service.GetEquipmentById(id);
        if (equipment == null)
        {
            Console.WriteLine("Equipment not found.");
            return;
        }

        Console.WriteLine($"\nAre you sure you want to delete '{equipment.Name}'? (y/n): ");
        var confirm = Console.ReadLine()?.ToLower();

        if (confirm == "y" || confirm == "yes")
        {
            _service.DeleteEquipment(id);
            Console.WriteLine("\nEquipment deleted successfully!");
        }
        else
        {
            Console.WriteLine("\nDeletion cancelled.");
        }
    }

    static void AddMaintenanceRecord()
    {
        Console.WriteLine("\n=== Add Maintenance Record ===");
        Console.Write("Enter equipment ID: ");

        if (!int.TryParse(Console.ReadLine(), out int equipmentId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var equipment = _service.GetEquipmentById(equipmentId);
        if (equipment == null)
        {
            Console.WriteLine("Equipment not found.");
            return;
        }

        Console.WriteLine($"Adding maintenance record for: {equipment.Name}");

        Console.Write("Enter maintenance date (yyyy-mm-dd): ");
        DateTime maintenanceDate;
        while (!DateTime.TryParse(Console.ReadLine(), out maintenanceDate))
        {
            Console.Write("Invalid date format. Please enter date (yyyy-mm-dd): ");
        }

        Console.Write("Enter maintenance type (e.g., Preventive, Corrective, Calibration): ");
        var maintenanceType = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter description: ");
        var description = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter performed by: ");
        var performedBy = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter cost: ");
        decimal cost;
        while (!decimal.TryParse(Console.ReadLine(), out cost))
        {
            Console.Write("Invalid cost. Please enter a valid number: ");
        }

        var record = new MaintenanceRecord
        {
            EquipmentId = equipmentId,
            MaintenanceDate = maintenanceDate,
            MaintenanceType = maintenanceType,
            Description = description,
            PerformedBy = performedBy,
            Cost = cost
        };

        _service.AddMaintenanceRecord(record);
        Console.WriteLine("\nMaintenance record added successfully!");
    }

    static void ViewMaintenanceHistory()
    {
        Console.WriteLine("\n=== View Maintenance History ===");
        Console.Write("Enter equipment ID: ");

        if (!int.TryParse(Console.ReadLine(), out int equipmentId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var equipment = _service.GetEquipmentById(equipmentId);
        if (equipment == null)
        {
            Console.WriteLine("Equipment not found.");
            return;
        }

        Console.WriteLine($"\nMaintenance History for: {equipment.Name} (ID: {equipment.Id})");
        var records = _service.GetMaintenanceRecordsByEquipmentId(equipmentId);

        if (records.Count == 0)
        {
            Console.WriteLine("No maintenance records found.");
            return;
        }

        Console.WriteLine($"\n{"ID",-5} {"Date",-12} {"Type",-15} {"Performed By",-20} {"Cost",-10}");
        Console.WriteLine(new string('-', 70));

        foreach (var record in records)
        {
            Console.WriteLine($"{record.Id,-5} {record.MaintenanceDate:yyyy-MM-dd}  {record.MaintenanceType,-15} {record.PerformedBy,-20} ${record.Cost,-9:F2}");
            Console.WriteLine($"       Description: {record.Description}");
        }
    }

    static void ViewAllMaintenanceRecords()
    {
        Console.WriteLine("\n=== All Maintenance Records ===");
        var records = _service.GetAllMaintenanceRecords();

        if (records.Count == 0)
        {
            Console.WriteLine("No maintenance records found.");
            return;
        }

        Console.WriteLine($"{"ID",-5} {"Equipment ID",-13} {"Date",-12} {"Type",-15} {"Performed By",-20} {"Cost",-10}");
        Console.WriteLine(new string('-', 80));

        foreach (var record in records)
        {
            var equipment = _service.GetEquipmentById(record.EquipmentId);
            var equipmentName = equipment?.Name ?? "Unknown";
            
            Console.WriteLine($"{record.Id,-5} {record.EquipmentId,-13} {record.MaintenanceDate:yyyy-MM-dd}  {record.MaintenanceType,-15} {record.PerformedBy,-20} ${record.Cost,-9:F2}");
            Console.WriteLine($"       Equipment: {equipmentName}");
            Console.WriteLine($"       Description: {record.Description}");
        }
    }
}
