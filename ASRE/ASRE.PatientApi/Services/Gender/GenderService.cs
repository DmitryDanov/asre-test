using ASRE.DataLayer.Context;
using ASRE.PatientApi.Core;
using Microsoft.EntityFrameworkCore;

namespace ASRE.PatientApi.Services.Gender;

public sealed class GenderService : IGenderService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenderService"/> class.
    /// </summary>
    /// <param name="dbContext">An instance of the <see cref="AppDbContext"/> class.</param>
    /// <param name="logger">An instance of a class which implements <see cref="ILogger{GenderService}"/>.</param>
    public GenderService(AppDbContext dbContext, ILogger<GenderService> logger)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(logger);

        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IOperationDataResult<DataLayer.Models.Gender>> GetGenderAsync(string? name, CancellationToken cancellationToken = default)
    {
        try
        {
            return OperationDataResult<DataLayer.Models.Gender>.Success(await _dbContext.Genders.FirstOrDefaultAsync(w => w.Name == name, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception exception)
        {
            if (exception is ArgumentNullException)
            {
                _logger.LogError(exception, "An exception has happened during retrieving '{Gender}' gender.", name);

                return OperationDataResult<DataLayer.Models.Gender>.Failure(exception);
            }
            else
            {
                throw;
            }
        }
    }
}