# Equipment Maintenance Record Flow Diagram

## Complete Application Flow

```mermaid
flowchart TD
    A[Program.cs - Main Entry] --> B[RunMainMenu]
    B --> C{User Menu Selection}
    
    %% Menu Options
    C -->|Option 6| D[AddMaintenanceRecord]
    C -->|Option 7| E[ViewMaintenanceHistory]
    C -->|Option 8| F[ViewAllMaintenanceRecords]
    
    %% Add Maintenance Record Flow
    D --> G[Get Equipment ID]
    G --> H{Equipment Exists?}
    H -->|No| I[Equipment Not Found Error]
    H -->|Yes| J[Collect Maintenance Data]
    J --> K[Create MaintenanceRecord Object]
    K --> L[EquipmentService.AddMaintenanceRecord]
    L --> M[Assign Unique ID]
    M --> N[Add to _maintenanceRecords List]
    N --> O[Success Message]
    
    %% View Equipment History Flow
    E --> P[Get Equipment ID]
    P --> Q{Equipment Exists?}
    Q -->|No| R[Equipment Not Found Error]
    Q -->|Yes| S[EquipmentService.GetMaintenanceRecordsByEquipmentId]
    S --> T[Filter Records by EquipmentId]
    T --> U[Display Equipment Records]
    
    %% View All Records Flow
    F --> V[EquipmentService.GetAllMaintenanceRecords]
    V --> W[Return All _maintenanceRecords]
    W --> X[Display All Records with Equipment Names]
    
    %% Equipment Service Layer
    subgraph ServiceLayer ["Equipment Service Layer"]
        L
        S
        V
        Y[_equipments List<Equipment>]
        Z[_maintenanceRecords List<MaintenanceRecord>]
        AA[_nextMaintenanceId Counter]
    end
    
    %% Data Model
    subgraph DataModel ["Data Models"]
        BB[MaintenanceRecord Model]
        CC[Equipment Model]
    end
    
    %% Service Dependencies
    L -.-> AA
    L -.-> Z
    S -.-> Z
    V -.-> Z
    
    %% Model Usage
    K -.-> BB
    L -.-> BB
    S -.-> BB
    V -.-> BB
    
    %% Equipment Validation
    H -.-> Y
    Q -.-> Y
    
    %% Return to Menu
    O --> B
    I --> B
    U --> B
    R --> B
    X --> B
    
    %% Styling
    classDef userInterface fill:#e1f5fe
    classDef service fill:#f3e5f5
    classDef dataModel fill:#e8f5e8
    classDef error fill:#ffebee
    
    class A,B,C,D,E,F,G,J,P userInterface
    class L,S,V,Y,Z,AA service
    class BB,CC dataModel
    class I,R error
```

## Data Model Structure

```mermaid
classDiagram
    class Equipment {
        +int Id
        +string Name
        +string SerialNumber
        +string Category
        +DateTime PurchaseDate
        +string Status
    }
    
    class MaintenanceRecord {
        +int Id
        +int EquipmentId
        +DateTime MaintenanceDate
        +string MaintenanceType
        +string Description
        +string PerformedBy
        +decimal Cost
    }
    
    class EquipmentService {
        -List~Equipment~ _equipments
        -List~MaintenanceRecord~ _maintenanceRecords
        -int _nextEquipmentId
        -int _nextMaintenanceId
        +AddEquipment(Equipment equipment)
        +GetEquipmentById(int id) Equipment?
        +UpdateEquipment(Equipment equipment)
        +DeleteEquipment(int id)
        +AddMaintenanceRecord(MaintenanceRecord record)
        +GetMaintenanceRecordsByEquipmentId(int equipmentId) List~MaintenanceRecord~
        +GetAllMaintenanceRecords() List~MaintenanceRecord~
    }
    
    Equipment ||--o{ MaintenanceRecord : "has many"
    EquipmentService --> Equipment : manages
    EquipmentService --> MaintenanceRecord : manages
```

## Sequence Diagram - Adding Maintenance Record

```mermaid
sequenceDiagram
    participant User
    participant Program
    participant EquipmentService
    participant Equipment
    participant MaintenanceRecord
    
    User->>Program: Select "Add Maintenance Record"
    Program->>User: Request Equipment ID
    User->>Program: Provide Equipment ID
    Program->>EquipmentService: GetEquipmentById(id)
    EquipmentService->>Equipment: Find equipment
    Equipment-->>EquipmentService: Return equipment or null
    EquipmentService-->>Program: Return equipment or null
    
    alt Equipment Found
        Program->>User: Request maintenance details
        User->>Program: Provide date, type, description, etc.
        Program->>MaintenanceRecord: Create new instance
        MaintenanceRecord-->>Program: New record object
        Program->>EquipmentService: AddMaintenanceRecord(record)
        EquipmentService->>EquipmentService: Assign unique ID
        EquipmentService->>EquipmentService: Add to _maintenanceRecords
        EquipmentService-->>Program: Success
        Program->>User: Display success message
    else Equipment Not Found
        Program->>User: Display error message
    end
```

## Component Architecture

```mermaid
graph TB
    subgraph "Presentation Layer"
        A[Program.cs Main Menu]
        B[Console Input/Output]
    end
    
    subgraph "Business Logic Layer"
        C[EquipmentService]
        D[Data Validation]
        E[ID Management]
    end
    
    subgraph "Data Layer"
        F[In-Memory Collections]
        G[Equipment List]
        H[MaintenanceRecord List]
    end
    
    subgraph "Models"
        I[Equipment Model]
        J[MaintenanceRecord Model]
    end
    
    A --> C
    B --> A
    C --> D
    C --> E
    C --> F
    F --> G
    F --> H
    C -.-> I
    C -.-> J
    
    classDef presentation fill:#bbdefb
    classDef business fill:#c8e6c9
    classDef data fill:#fff3e0
    classDef models fill:#f8bbd9
    
    class A,B presentation
    class C,D,E business
    class F,G,H data
    class I,J models
```