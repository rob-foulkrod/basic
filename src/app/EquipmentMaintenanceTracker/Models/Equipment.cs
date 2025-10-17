namespace EquipmentMaintenanceTracker.Models;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public string Status { get; set; } = "Active";
}
