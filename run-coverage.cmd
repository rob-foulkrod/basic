@echo off
REM Equipment Maintenance Tracker - Code Coverage Batch Script

echo Running tests with code coverage...

REM Clean previous coverage results
if exist "coverage" rmdir /s /q "coverage"
mkdir "coverage"

REM Run tests with coverage
dotnet test src/test/EquipmentMaintenanceTracker.Tests/EquipmentMaintenanceTracker.Tests.csproj ^
    --collect:"XPlat Code Coverage" ^
    --results-directory:"coverage/raw" ^
    --logger:"console;verbosity=detailed"

if %ERRORLEVEL% neq 0 (
    echo Test execution failed!
    exit /b 1
)

echo.
echo Coverage data collected in coverage/raw directory
echo To generate HTML reports, run: powershell -ExecutionPolicy Bypass -File run-coverage.ps1
pause