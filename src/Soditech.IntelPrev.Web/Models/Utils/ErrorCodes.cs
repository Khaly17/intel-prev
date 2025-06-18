namespace  Soditech.IntelPrev.Web.Models.Utils;

/// <summary>
/// A static class containing predefined error codes used throughout the application.
/// </summary>
/// <remarks>
/// This class provides a centralized location for common error codes, allowing for consistent error handling and easier debugging.
/// </remarks>
public static class ErrorCodes
{
    /// <summary>
    /// Error code used when attempting to create a duplicate record.
    /// </summary>
    public const string DuplicateRecord = "DuplicateRecord";

    /// <summary>
    /// Error code used when invalid input is provided.
    /// </summary>
    public const string InvalidInput = "InvalidInput";

    /// <summary>
    /// Error code used when the server encounters an issue processing a request.
    /// </summary>
    public const string ServerError = "ServerError";

    /// <summary>
    /// Error code used when the requested resource cannot be found.
    /// </summary>
    public const string NotFound = "NotFound";

    /// <summary>
    /// Error code used when there is an issue deleting a record.
    /// </summary>
    public const string DeletionError = "DeleteError";
}