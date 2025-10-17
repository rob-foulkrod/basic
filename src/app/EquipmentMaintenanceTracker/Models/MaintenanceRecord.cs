namespace EquipmentMaintenanceTracker.Models;

public class MaintenanceRecord
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string MaintenanceType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PerformedBy { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}
