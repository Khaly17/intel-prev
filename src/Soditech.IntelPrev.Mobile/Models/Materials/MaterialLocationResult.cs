using System;

namespace Soditech.IntelPrev.Mobile.Models.Materials;

public class MaterialLocationResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public MaterialType Type { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    public string FloorNumber { get; set; } = string.Empty;
}