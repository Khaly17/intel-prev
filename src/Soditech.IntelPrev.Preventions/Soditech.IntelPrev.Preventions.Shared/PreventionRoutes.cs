namespace Soditech.IntelPrev.Preventions.Shared;

public static class PreventionRoutes
{
    public static class Buildings
    {
        public const string GetAll = "/api/buildings";
        public const string GetById = "/api/buildings/{id:guid}";
        public const string Create = "/api/buildings";
        public const string Update = "/api/buildings/{id:guid}";
        public const string Delete = "/api/buildings/{id:guid}";
        public const string Count = "/api/buildings/count";
        public const string Floors = "/api/buildings/{id:guid}/floors";
    }

    public static class Floors
    {
        public const string GetAll = "/api/floors";
        public const string GetById = "/api/floors/{id:guid}";
        public const string Create = "/api/floors";
        public const string Update = "/api/floors/{id:guid}";
        public const string Delete = "/api/floors/{id:guid}";
        public const string Count = "/api/floors/count";
    }
    
    public static class Campaigns
    {
        public const string GetAll = "/api/campaigns";
        public const string GetById = "/api/campaigns/{id:guid}";
        public const string Create = "/api/campaigns";
        public const string Update = "/api/campaigns/{id:guid}";
        public const string Delete = "/api/campaigns/{id:guid}";
        public const string Count = "/api/campaigns/count";
    }
    
    public static class CommitteeMembers
    {
        public const string GetAll = "/api/committee-members";
        public const string GetById = "/api/committee-members/{id:guid}";
        public const string Create = "/api/committee-members";
        public const string Update = "/api/committee-members/{id:guid}";
        public const string Delete = "/api/committee-members/{id:guid}";
        public const string Count = "/api/committee-members/count";
    }
    
    public static class Equipments
    {
        public const string GetAll = "/api/equipments";
        public const string GetById = "/api/equipments/{id:guid}";
        public const string Create = "/api/equipments";
        public const string Update = "/api/equipments/{id:guid}";
        public const string Delete = "/api/equipments/{id:guid}";
        public const string Count = "/api/equipments/count";
        public const string GetEquipmentsByType = "/api/equipments/types/{type}";
        public const string UpdateGeoLocation = "/api/equipments/geoLocation/";
    }
    
    public static class GatheringPoints
    {
        public const string GetAll = "/api/gathering-points";
        public const string GetById = "/api/gathering-points/{id:guid}";
        public const string Create = "/api/gathering-points";
        public const string Update = "/api/gathering-points/{id:guid}";
        public const string Delete = "/api/gathering-points/{id:guid}";
        public const string Count = "/api/gathering-points/count";
        public const string GetGatheringPointsByBuilding = "/api/gathering-points/buildings/";
        public const string UpdateGeoLocation = "/api/gathering-points/geoLocation/";
    }
    
    public static class Events
    {
        public const string GetAll = "/api/events";
        public const string GetById = "/api/events/{id:guid}";
        public const string Create = "/api/events";
        public const string Update = "/api/events/{id:guid}";
        public const string Delete = "/api/events/{id:guid}";
        public const string Count = "/api/events/count";
        public const string GetUpComing = "/api/events/upcoming";
    }
    
    public static class MedicalContacts
    {
        public const string GetAll = "/api/medical-contacts";
        public const string GetById = "/api/medical-contacts/{id:guid}";
        public const string Create = "/api/medical-contacts";
        public const string Update = "/api/medical-contacts/{id:guid}";
        public const string Delete = "/api/medical-contacts/{id:guid}";
        public const string Count = "/api/medical-contacts/count";
    }
    
    public static class StaticContents
    {
        public const string GetAll = "/api/static-contents";
        public const string GetById = "/api/static-contents/{id:guid}";
        public const string Create = "/api/static-contents";
        public const string Update = "/api/static-contents/{id:guid}";
        public const string Delete = "/api/static-contents/{id:guid}";
        public const string Count = "/api/static-contents/count";
    }
    
    public static class Statistics
    {
        public const string GetAll = "/api/statics";
        public const string GetById = "/api/statics/{id:guid}";
        public const string Create = "/api/statics";
        public const string Update = "/api/statics/{id:guid}";
        public const string Delete = "/api/statics/{id:guid}";
        public const string Count = "/api/statics/count";
    }

    public static class Materials
    {
        private const string Base = "api/materials";
        
        public static string GetExtinguishers = $"{Base}/extinguishers";
        public static string GetDefibrillators = $"{Base}/dae";
        public static string GetAssemblyPoints = $"{Base}/assembly-points";
    }
    
    public static class GeoLocations
    {
        public const string GetAll = "/api/geo-locations";
        public const string GetAllByType = "/api/geo-locations/type/{type}";
        public const string GetById = "/api/geo-locations/{id:guid}";
        public const string Create = "/api/geo-locations";
        public const string Update = "/api/geo-locations/{id:guid}";
        public const string Delete = "/api/geo-locations/{id:guid}";
        public const string Count = "/api/geo-locations/count";
    }
    
    public static class ProPrevSettings
    {
        public const string UpdateRiskAnalysisProtocolContent = "/api/pro-prev-settings/riskAnalysisProtocolContent";
        public const string GetRiskAnalysisProtocolContent = "/api/pro-prev-settings/riskAnalysisProtocolContent";
        
        public const string UpdateAnalysisToolsContent = "/api/pro-prev-settings/analysisToolsContent";
        public const string GetAnalysisToolsContent = "/api/pro-prev-settings/analysisToolsContent";
        
        public const string UpdateActionsOrganizerContent = "/api/pro-prev-settings/actionsOrganizerContent";
        public const string GetActionsOrganizerContent = "/api/pro-prev-settings/actionsOrganizerContent";
        
        public const string UpdateCseAgendaContent = "/api/pro-prev-settings/cseAgendaContent";
        public const string GetCseAgendaContent = "/api/pro-prev-settings/cseAgendaContent";
        
        public const string UpdateDataSheetContent = "/api/pro-prev-settings/dataSheetContent";
        public const string GetDataSheetContent = "/api/pro-prev-settings/dataSheetContent";
        
        public const string UpdateEpiControlContent = "/api/pro-prev-settings/epiControlContent";
        public const string GetEpiControlContent = "/api/pro-prev-settings/epiControlContent";
        
        public const string UpdateHealthFormationContent = "/api/pro-prev-settings/healthFormationContent";
        public const string GetHealthFormationContent = "/api/pro-prev-settings/healthFormationContent";
        
        public const string UpdateMyLibraryContent = "/api/pro-prev-settings/myLibraryContent";
        public const string GetMyLibraryContent = "/api/pro-prev-settings/myLibraryContent";
        
        public const string UpdateSecurityQuarterContent = "/api/pro-prev-settings/securityQuarterContent";
        public const string GetSecurityQuarterContent = "/api/pro-prev-settings/securityQuarterContent";
        
        public const string UpdateSitesVisitContent = "/api/pro-prev-settings/sitesVisitContent";
        public const string GetSitesVisitContent = "/api/pro-prev-settings/sitesVisitContent";
        
        public const string UpdateFirstAidKitContent = "/api/pro-prev-settings/firstAidKitContent";
        public const string GetFirstAidKitContent = "/api/pro-prev-settings/firstAidKitContent";
    }
    
    public static class FireSecuritySettings
    {
        public const string UpdateDefinitionContent = "/api/fire-security-settings/definitionContent";
        public const string GetDefinitionContent = "/api/fire-security-settings/definitionContent";
        
        public const string UpdateKnownMyEnterpriseContent = "/api/fire-security-settings/knownMyEnterpriseContent";
        public const string GetKnownMyEnterpriseContent = "/api/fire-security-settings/knownMyEnterpriseContent";
        
        public const string UpdateFireSecurityServiceContent = "/fire-security-settings/fireSecurityServiceContent";
        public const string GetFireSecurityServiceContent = "/api/fire-security-settings/fireSecurityServiceContent";
        
        public const string UpdateFireConsignsContent = "/api/fire-security-settings/fireConsignsContent";
        public const string GetFireConsignsContent = "/api/fire-security-settings/fireConsignsContent";
        
        public const string UpdateFireMaterialsContent = "/api/fire-security-settings/sireMaterialsContent";
        public const string GetFireMaterialsContent = "/api/fire-security-settings/fireMaterialsContent";
        
        public const string UpdateEvacuationCaseContent = "/api/fire-security-settings/evacuationCaseContent";
        public const string GetEvacuationCaseContent = "/api/fire-security-settings/evacuationCaseContent";
    }
      
    public static class PreventionSettings
    {
        public const string UpdateDefinitionContent = "/api/prevention-settings/definitionContent";
        public const string GetDefinitionContent = "/api/prevention-settings/definitionContent";
        
        public const string UpdateSensibilisationContent = "/api/prevention-settings/sensibilisationContent";
        public const string GetSensibilisationContent = "/api/prevention-settings/sensibilisationContent";
    }
    
}