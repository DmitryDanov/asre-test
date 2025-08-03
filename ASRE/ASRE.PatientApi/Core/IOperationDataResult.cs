namespace ASRE.PatientApi.Core;

public interface IOperationDataResult<TData> : IOperationResult where TData : class
{
    /// <summary>
    /// Gets a carried over data.
    /// </summary>
    TData? Data {  get; init; }
}