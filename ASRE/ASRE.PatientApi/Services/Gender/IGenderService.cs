using ASRE.PatientApi.Core;

namespace ASRE.PatientApi.Services.Gender
{
    public interface IGenderService
    {
        /// <summary>
        /// Gets gender by its unique identifier.
        /// </summary>
        /// <param name="name">A gender name.</param>
        /// <param name="cancellationToken">A cancellation token which is used to cancel get operation.</param>
        /// <returns>The tas that represents an asynchronous get operation which result is <see cref="IOperationDataResult{DataLayer.Models.Gender}"/>.</returns>
        Task<IOperationDataResult<DataLayer.Models.Gender>> GetGenderAsync(string? name, CancellationToken cancellationToken = default);
    }
}