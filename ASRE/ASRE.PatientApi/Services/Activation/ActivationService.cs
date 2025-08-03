using ASRE.DataLayer.Context;
using ASRE.PatientApi.Core;
using Microsoft.EntityFrameworkCore;

namespace ASRE.PatientApi.Services.Activation;

public sealed class ActivationService : IActivationService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivationService"/> class.
    /// </summary>
    /// <param name="dbContext">An instance of the <see cref="AppDbContext"/> class.</param>
    /// <param name="logger">An instance of a class which implements <see cref="ILogger{ActivationService}"/>.</param>
    public ActivationService(AppDbContext dbContext, ILogger<ActivationService> logger)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IOperationDataResult<DataLayer.Models.Activation>> GetActivationAsync(string? name, CancellationToken cancellationToken = default)
    {
        try
        {
            return OperationDataResult<DataLayer.Models.Activation>.Success(await _dbContext.Activations.FirstOrDefaultAsync(w => w.Name == name, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception exception)
        {
            if (exception is ArgumentNullException)
            {
                _logger.LogError(exception, "An exception has happened during retrieving '{Activation}' activation.", name);

                return OperationDataResult<DataLayer.Models.Activation>.Failure(exception);
            }
            else
            {
                throw;
            }
        }
    }
}