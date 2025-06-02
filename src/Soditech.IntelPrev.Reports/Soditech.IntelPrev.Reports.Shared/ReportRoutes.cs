namespace Soditech.IntelPrev.Reports.Shared;

public static class ReportRoutes
{
    
    public static class ReportAttachments
    {
        public const string GetAll = "/api/reportAttachments";
        public const string GetById = "/api/reportAttachments/{id:guid}";
        public const string Create = "/api/reportAttachments";
        public const string Update = "/api/reportAttachments/{id:guid}";
        public const string Delete = "/api/reportAttachments/{id:guid}";
        public const string Count = "/api/reportAttachments/count";
    }
    
    public static class ReportComments
    {
        public const string GetAll = "/api/reportComments";
        public const string GetById = "/api/reportComments/{id:guid}";
        public const string Create = "/api/reportComments";
        public const string Update = "/api/reportComments/{id:guid}";
        public const string Delete = "/api/reportComments/{id:guid}";
        public const string Count = "/api/reportComments/count";
    }
    
    public static class Reports
    {
        public const string GetAll = "/api/reports";
        public const string GetById = "/api/reports/{id:guid}";
        public const string Create = "/api/reports";
        public const string Update = "/api/reports/{id:guid}";
        public const string Delete = "/api/reports/{id:guid}";
        public const string Count = "/api/reports/count";

        public const string GetReportsGroupedByRegister = "/api/reports/reportsGroupedByRegister";
        public const string GetCountReportsGroupedByRegister = "/api/reports/countReportsGroupedByRegister";
        
        // New draft routes for saving and retrieving draft reports
        public const string SaveDraft = "/api/reports/draft";
        public const string GetDraft = "/api/reports/draft/{registerTypeId:guid}";
    }
    public static class Alerts
    {
        public const string GetAll = "/api/alerts";
        public const string GetById = "/api/alerts/{id:guid}";
        public const string Create = "/api/alerts";
        public const string Update = "/api/alerts/{id:guid}";
        public const string Delete = "/api/alerts/{id:guid}";
        public const string Count = "/api/alerts/count";

        public const string GetCountAlertsGroupedByType = "/api/reports/countAlertsGroupedByType";
        public const string GetAlertsByPeriod = "/api/reports/AlertsGroupedByType";


    }

    public static class RegisterTypes
    {
        public const string GetAll = "/api/registerTypes";
        public const string GetById = "/api/registerTypes/{id:guid}";
        public const string Create = "/api/registerTypes";
        public const string Update = "/api/registerTypes/{id:guid}";
        public const string Delete = "/api/registerTypes/{id:guid}";
        public const string Count = "/api/registerTypes/count";
    }
    
    public static class RegisterFields
    {
        public const string GetAll = "/api/registerFields";
        public const string GetById = "/api/registerFields/{id:guid}";
        public const string Create = "/api/registerFields";
        public const string Update = "/api/registerFields/{id:guid}";
        public const string Delete = "/api/registerFields/{id:guid}";
        public const string Count = "/api/registerFields/count";
    }
    
    public static class ReportDatas
    {
        public const string GetAll = "/api/reportDatas";
        public const string GetById = "/api/reportDatas/{id:guid}";
        public const string Create = "/api/reportDatas";
        public const string Update = "/api/reportDatas/{id:guid}";
        public const string Delete = "/api/reportDatas/{id:guid}";
        public const string Count = "/api/reportDatas/count";
    }
}