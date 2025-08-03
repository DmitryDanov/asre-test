namespace ASRE.PatientApi.Core;

public interface IOperationResult
{
    /// <summary>
    /// Gets or sets a collection of errors.
    /// </summary>
    ICollection<string> Errors { get; init; }

    /// <summary>
    /// Gets whether result is successful.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Returns flattened list of errors.
    /// </summary>
    /// <returns>Flattened list of errors.</returns>
    string GetErrors();
}