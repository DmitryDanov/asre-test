using ASRE.PatientApi.Core;
using ASRE.PatientApi.Core.DateHandler.Models;

namespace ASRE.PatientApi.Services.Patient;

public interface IPatientService
{
    /// <summary>
    /// Creates a new <see cref="DataLayer.Models.Patient"/> entity in a storage.
    /// </summary>
    /// <param name="patient">An instance of a <see cref="DataLayer.Models.Patient"/>.</param>
    /// <param name="cancellationToken">A cancellation token which is used to cancel add operation.</param>
    /// <returns>The task that represents an asynchronous create operation which result is <see cref="IOperationDataResult{DataLayer.Models.Patient}"/>.</returns>
    Task<IOperationDataResult<DataLayer.Models.Patient>> AddPatientAsync(DataLayer.Models.Patient patient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="DataLayer.Models.Patient"/> entity from a storage.
    /// </summary>
    /// <param name="id">A patient's unique identifier.</param>
    /// <param name="cancellationToken">A cancellation token which is used to cancel delete operation.</param>
    /// <returns>The task that represents an asynchronous delete operation which result is <see cref="IOperationResult"/>.</returns>
    Task<IOperationResult> DeletePatientAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a <see cref="DataLayer.Models.Patient"/> entity from a storage by a <paramref name="id"/>.
    /// </summary>
    /// <param name="id">A patient's unique identifier.</param>
    /// <param name="cancellationToken">A cancellation token which is used to cancel get operation.</param>
    /// <returns>The tas that represents an asynchronous get operation which result is <see cref="IOperationDataResult{DataLayer.Models.Patient}"/>.</returns>
    Task<IOperationDataResult<DataLayer.Models.Patient>> GetPatientAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a collection of <see cref="DataLayer.Models.Patient"/> entities from a storage. The collection can be filtered out using optional <paramref name="dateRangeFilter"/>.
    /// </summary>
    /// <param name="dateRangeFilter">An optional date range filter.</param>
    /// <param name="cancellationToken">A cancellation token which is used to cancel get operation.</param>
    /// <returns>The tas that represents an asynchronous get operation which result is <see cref="IOperationDataResult{DataLayer.Models.Patient[]}"/>.</returns>
    Task<IOperationDataResult<DataLayer.Models.Patient[]>> GetPatientsAsync(DateRangeFilterModel? dateRangeFilter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a <see cref="DataLayer.Models.Patient"/> entity in a storage.
    /// </summary>
    /// <param name="patient">An instance of a <see cref="DataLayer.Models.Patient"/>.</param>
    /// <param name="cancellationToken">A cancellation token which is used to cancel update operation.</param>
    /// <returns>The task that represents an asynchronous update operation which result is <see cref="IOperationResult"/>.</returns>
    Task<IOperationResult> UpdatePatientAsync(DataLayer.Models.Patient patient, CancellationToken cancellationToken = default);
}