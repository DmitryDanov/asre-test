using ASRE.PatientApi.Core;

namespace ASRE.PatientApi.Services.Activation;

public interface IActivationService
{
    /// <summary>
    /// Gets activation by its name.
    /// </summary>
    /// <param name="name">An activation name.</param>
    /// <param name="cancellationToken">A cancellation token which is used to cancel get operation.</param>
    /// <returns>The task that represents an asynchronous get operation which result is <see cref="IOperationDataResult{DataLayer.Models.Activation}"/>.</returns>
    Task<IOperationDataResult<DataLayer.Models.Activation>> GetActivationAsync(string? name, CancellationToken cancellationToken = default);
}