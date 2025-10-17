# Equipment Maintenance Tracker

A C# console application for managing medical equipment and their maintenance records.

## Project Structure

```
Basic/
├── src/
│   ├── app/
│   │   └── EquipmentMaintenanceTracker/
│   │       ├── Models/
│   │       │   ├── Equipment.cs
│   │       │   └── MaintenanceRecord.cs
│   │       ├── Services/
│   │       │   └── EquipmentService.cs
│   │       └── Program.cs
│   └── test/
│       └── EquipmentMaintenanceTracker.Tests/
│           ├── ModelTests.cs
│           └── UnitTest1.cs (EquipmentServiceTests)
└── EquipmentMaintenanceTracker.sln
```

## Features

- **Equipment Management**: Add, view, update, and delete medical equipment
- **Maintenance Records**: Track maintenance history for each piece of equipment
- **Menu-Driven Interface**: Easy-to-use console interface with 9 menu options
- **In-Memory Storage**: Uses List<T> collections for data storage
- **Pre-Seeded Data**: Includes sample equipment to get started

## Requirements

- .NET 9.0 SDK or later

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/rob-foulkrod/Basic.git
cd Basic
```

### Build the Application

```bash
dotnet build EquipmentMaintenanceTracker.sln
```

### Run the Application

```bash
cd src/app/EquipmentMaintenanceTracker
dotnet run
```

### Run Tests

```bash
dotnet test EquipmentMaintenanceTracker.sln
```

## Usage

When you run the application, you'll see a menu with the following options:

1. **List All Equipment** - View all equipment in the system
2. **Add New Equipment** - Add a new piece of equipment
3. **View Equipment Details** - View detailed information about a specific equipment
4. **Update Equipment** - Modify existing equipment information
5. **Delete Equipment** - Remove equipment and its maintenance records
6. **Add Maintenance Record** - Add a new maintenance record for an equipment
7. **View Maintenance History for Equipment** - View all maintenance records for a specific equipment
8. **View All Maintenance Records** - View all maintenance records in the system
9. **Exit** - Close the application

## Models

### Equipment
- ID (int)
- Name (string)
- Serial Number (string)
- Category (string)
- Purchase Date (DateTime)
- Status (string)

### MaintenanceRecord
- ID (int)
- Equipment ID (int)
- Maintenance Date (DateTime)
- Maintenance Type (string)
- Description (string)
- Performed By (string)
- Cost (decimal)

## Testing

The project includes comprehensive unit tests covering:
- Model validation
- Equipment CRUD operations
- Maintenance record management
- Service layer functionality

All 14 tests pass successfully.

## License

MIT License - See LICENSE file for details
