using ASRE.DataGenerator.Services.DataGenerator.Models;

namespace ASRE.DataGenerator.Services.HttpSender;

internal interface IHttpSenderService
{
    /// <summary>
    /// Sends POST request to /patients endpoint to add patient.
    /// </summary>
    /// <param name="patient">A <see cref="PatientModel"/>.</param>
    /// <param name="cancellationToken">A cancellation token which is used to cancel post operation.</param>
    /// <returns>The task that represents an asynchronous operation.</returns>
    Task<bool> PostPatient(PatientModel patient, CancellationToken cancellationToken = default);
}