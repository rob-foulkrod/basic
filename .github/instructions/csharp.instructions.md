---
applyTo: '**/*.cs'
---
Provide project context and coding guidelines that AI should follow when generating code, answering questions, or reviewing changes.

## C# Coding Style Guide

### Naming Conventions
- **PascalCase**: Classes, methods, properties, public fields (`Equipment`, `AddEquipment()`, `SerialNumber`)
- **camelCase**: Local variables, parameters, private fields (`_equipments`, `equipmentId`)
- **I prefix**: Interfaces (`IValidationStrategy`, `IValidationContext`)
- **UPPER_SNAKE_CASE**: Constants (if used)

### Code Organization
- One class per file
- Member order: Fields → Properties → Constructors → Methods
- Group `using` statements: System first, then third-party, then project
- Keep files under 500 lines

### Access Modifiers
- **private** by default, expand access as needed
- Use **readonly** for immutable fields (critical for ID immutability)
- Make validation strategies **public** for DI

### Formatting
- **4 spaces** indentation (no tabs)
- Lines under 100-120 characters
- Blank lines between logical sections
- Consistent brace style (Allman or Stroustrup)

### Methods & Functions
- Single responsibility principle
- Max 3-4 parameters; use objects for more
- Descriptive names reflecting behavior
- Return early to avoid deep nesting

### Exception Handling
- Use custom exceptions for domain errors (`ValidationException`)
- Avoid generic `Exception` catches
- Include meaningful error messages

### Key Project Rules
1. **ID Immutability** - IDs never change after creation
2. **Cascading Delete** - Deleting equipment removes maintenance records
3. **Serial Number Uniqueness** - No duplicate serial numbers
4. **Foreign Key Integrity** - Records must reference existing equipment
5. **Strategy Pattern** - Use for validation extensibility