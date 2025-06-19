namespace Soditech.IntelPrev.Prevensions.Shared.Enums;

// enum for the type of the location ( it a building, floor, gathering point or equipment)
public enum GeoLocationType
{
    Building,
    Floor, 
    Dae,
    Extinguisher,
    Epi,
    GatheringPoint
}

public enum EquipmentType
{
    Dae,
    Extinguisher,
    Epi
}

// use EquipmentType to extend the GeoLocationType
public static class GeoLocationTypeExtensions
{
    public static GeoLocationType ToGeoLocationType(this EquipmentType equipmentType)
    {
        return equipmentType switch
        {
            EquipmentType.Dae => GeoLocationType.Dae,
            EquipmentType.Extinguisher => GeoLocationType.Extinguisher,
            EquipmentType.Epi => GeoLocationType.Epi,
            _ => GeoLocationType.Extinguisher
        };
    }
    
    // use string to extend the GeoLocationType
    public static GeoLocationType ToGeoLocationType(this string type)
    {
        return type switch
        {
            "Dae" => GeoLocationType.Dae,
            "Extinguisher" => GeoLocationType.Extinguisher,
            "Epi" => GeoLocationType.Epi,
            _ => GeoLocationType.Extinguisher
        };
    }
}
