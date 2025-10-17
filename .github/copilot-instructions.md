# Equipment Maintenance Tracker - Copilot Instructions

## 📋 Project Overview
C# console application (.NET 9.0) for medical equipment inventory and maintenance tracking. In-memory storage, no database.

## 🏗️ Structure

```
src/
├── app/EquipmentMaintenanceTracker/
│   ├── Models/
│   │   ├── Equipment.cs
│   │   └── MaintenanceRecord.cs
│   ├── Services/EquipmentService.cs
│   ├── Validation/ (IValidationStrategy, ValidationResult, Strategies/)
│   └── Program.cs
└── test/EquipmentMaintenanceTracker.Tests/
    ├── ModelTests.cs
    ├── UnitTest1.cs
    └── Services/, Validation/ (test subdirectories)
```

## 🔑 Core Components

**Equipment.cs** - Id, Name, SerialNumber, Category, PurchaseDate, Status (immutable IDs)

**MaintenanceRecord.cs** - Id, EquipmentId (FK), MaintenanceDate, Type, Description, PerformedBy, Cost

**EquipmentService.cs** - Manages `_equipments` and `_maintenanceRecords` collections, auto-generates IDs, pre-seeds 3 sample items, enforces cascading deletes

**Program.cs** - Menu-driven console UI (options 1-9: equipment management, maintenance records, exit)

**Validation** - Strategy pattern: BasicEquipmentValidationStrategy, BusinessRulesValidationStrategy, SerialNumberValidationStrategy

## 🧪 Testing

Xunit framework. Test files in `src/test/`:
- **ModelTests.cs** - Model validation
- **UnitTest1.cs** - Service CRUD
- **Services/** - Detailed scenarios
- **Validation/** - Strategy tests

Run: `dotnet test` | Coverage: `./run-coverage.ps1` → `coverage/html/index.html`

## ⚡ Critical Rules

1. **ID Immutability** - Never changes after creation
2. **Cascading Delete** - Deleting equipment removes all maintenance records
3. **Serial Number Uniqueness** - No duplicates
4. **Foreign Key Integrity** - Records must reference existing equipment
5. **In-Memory Only** - Data lost on exit
6. **Pre-seeded Data** - 3 sample items on startup

## 🚀 Quick Commands

```bash
dotnet build EquipmentMaintenanceTracker.sln
dotnet test
cd src/app/EquipmentMaintenanceTracker && dotnet run
./run-coverage.ps1
```

## 📊 Architecture

**Layered:** Presentation (Program.cs) → Business Logic (EquipmentService + Validation) → Data (in-memory List<T>) → Models

**Relationships:** One Equipment → Many MaintenanceRecords (1:N)

## ⚙️ Key Dependencies

- .NET 9.0
- Xunit
- ReportGenerator

## ⚠️ Important Constraints

- Single-user, no concurrency
- Console-only UI
- In-memory (not scalable)
- No authentication
- Learning project

---

**Last Updated:** October 17, 2025 | **.NET 9.0** | **C#**
