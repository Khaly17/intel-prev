namespace Soditech.IntelPrev.Mediatheques.Shared;

public static class MediathequeRoutes
{
    public static class Documents
    {
        public const string GetAll = "/api/documents";
        public const string GetAllByType = "/api/documents/{type}";
        public const string GetBytes = "/api/documents/files/{path}";
        public const string GetById = "/api/documents/{id:guid}";
        public const string Create = "/api/documents";
        public const string Update = "/api/documents/{id:guid}";
        public const string Delete = "/api/documents/{id:guid}";
        public const string Count = "/api/documents/count";
    }
}