namespace ASRE.PatientApi.Core;

// Perhaps it'd be better to create Success and Failure as extension methods. But keeping it simple for this test project.
public sealed class OperationDataResult<TData> : OperationResult, IOperationDataResult<TData> where TData : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OperationDataResult{TData}"/> class.
    /// </summary>
    /// <param name="data">A data that <see cref="OperationDataResult{TData}"/> carries over.</param>
    /// <param name="errors">Optional collection of errors.</param>
    public OperationDataResult(TData? data, params string[] errors)
        : base(errors)
    {
        Data = data;
    }

    /// <inheritdoc/>
    public TData? Data { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="OperationDataResult{TData}"/> for a successfully completed operation.
    /// </summary>
    /// <param name="data">A data that <see cref="OperationDataResult{TData}"/> carries over.</param>
    /// <returns>The instance of <see cref="OperationDataResult{TData}"/>.</returns>
    public static OperationDataResult<TData> Success(TData? data)
    {
        return new OperationDataResult<TData>(data);
    }

    /// <summary>
    /// Creates an instance of <see cref="OperationDataResult{TData}"/> for a failed operation.
    /// </summary>
    /// <param name="errors">A collection of errors.</param>
    /// <returns>The instance of <see cref="OperationDataResult{TData}"/>.</returns>
    public new static OperationDataResult<TData> Failure(params string[] errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        if (!errors.Any())
        {
            return new OperationDataResult<TData>(null!, GenericErrorMessage);
        }

        return new OperationDataResult<TData>(null!, errors);
    }

    /// <summary>
    /// Creates an instance of <see cref="OperationDataResult{TData}"/> for a failed operation.
    /// </summary>
    /// <param name="exception">An instance of the <see cref="Exception"/> class.</param>
    /// <returns>The instance of <see cref="OperationDataResult{TData}"/>.</returns>
    public new static OperationDataResult<TData> Failure(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return Failure(exception.Message);
    }
}