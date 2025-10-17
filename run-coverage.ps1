#!/usr/bin/env pwsh

# Equipment Maintenance Tracker - Code Coverage Script
# This script runs tests with code coverage and generates HTML reports

Write-Host "Running Equipment Maintenance Tracker tests with code coverage..." -ForegroundColor Green

# Clean previous coverage results
if (Test-Path "coverage") {
    Remove-Item -Recurse -Force "coverage"
}
New-Item -ItemType Directory -Force -Path "coverage" | Out-Null

# Run tests with coverage
Write-Host "Executing tests..." -ForegroundColor Yellow
dotnet test src/test/EquipmentMaintenanceTracker.Tests/EquipmentMaintenanceTracker.Tests.csproj `
    --collect:"XPlat Code Coverage" `
    --results-directory:"coverage/raw" `
    --logger:"console;verbosity=detailed"

# Find the coverage file
$coverageFiles = Get-ChildItem -Path "coverage/raw" -Recurse -Filter "*.cobertura.xml"

if ($coverageFiles.Count -eq 0) {
    Write-Host "No coverage files found!" -ForegroundColor Red
    exit 1
}

$latestCoverageFile = $coverageFiles | Sort-Object LastWriteTime -Descending | Select-Object -First 1

Write-Host "Generating HTML coverage report..." -ForegroundColor Yellow

# Generate HTML report using ReportGenerator
dotnet tool install --global dotnet-reportgenerator-globaltool 2>$null

reportgenerator `
    -reports:"$($latestCoverageFile.FullName)" `
    -targetdir:"coverage/html" `
    -reporttypes:"Html;HtmlSummary;Badges;TextSummary" `
    -verbosity:Info

# Display summary
Write-Host "`nCoverage report generated successfully!" -ForegroundColor Green
Write-Host "HTML Report: coverage/html/index.html" -ForegroundColor Cyan
Write-Host "Coverage File: $($latestCoverageFile.FullName)" -ForegroundColor Cyan

# Show text summary if available
$summaryFile = "coverage/html/Summary.txt"
if (Test-Path $summaryFile) {
    Write-Host "`nCoverage Summary:" -ForegroundColor Yellow
    Get-Content $summaryFile
}

Write-Host "`nTo view the HTML report, run: Start-Process 'coverage/html/index.html'" -ForegroundColor Magenta