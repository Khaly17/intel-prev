namespace  Soditech.IntelPrev.Web.Models.Utils;

/// <summary>
/// A static class containing predefined error messages used throughout the application.
/// </summary>
/// <remarks>
/// This class provides a centralized location for common error messages to ensure consistency across the application.
/// </remarks>
public static class ErrorMessages
{
    /// <summary>
    /// Error message used when attempting to create a duplicate record.
    /// </summary>
    public const string DuplicateRecordMessage = "A record with the same name already exists. Please choose a different name.";

    /// <summary>
    /// Error message used when invalid input is provided.
    /// </summary>
    public const string InvalidInputMessage = "The input provided is invalid. Please check and try again.";

    /// <summary>
    /// Generic error message used when the server encounters an issue processing a request.
    /// </summary>
    public const string ServerErrorMessage = "Sorry, something went wrong. We're not able to process the request.";

    /// <summary>
    /// Error message used when the requested resource cannot be found.
    /// </summary>
    public const string NotFoundMessage = "The requested resource could not be found. Please check the information and try again.";

    /// <summary>
    /// Error message used when a record cannot be deleted.
    /// </summary>
    public const string DeletionErrorMessage = "The record could not be deleted. Please try again or contact support if the issue persists.";
}