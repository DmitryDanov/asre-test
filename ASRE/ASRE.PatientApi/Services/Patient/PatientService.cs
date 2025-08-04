using ASRE.DataLayer.Context;
using ASRE.PatientApi.Core;
using ASRE.PatientApi.Core.DateHandler.Models;
using Microsoft.EntityFrameworkCore;

namespace ASRE.PatientApi.Services.Patient;

public sealed class PatientService : IPatientService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientService"/> class.
    /// </summary>
    /// <param name="dbContext">An instance of the <see cref="AppDbContext"/> class.</param>
    /// <param name="logger">An instance of a class which implements <see cref="ILogger{PatientService}"/>.</param>
    public PatientService(AppDbContext dbContext, ILogger<PatientService> logger)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(logger);

        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IOperationDataResult<DataLayer.Models.Patient>> AddPatientAsync(DataLayer.Models.Patient patient, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(patient);

        try
        {
            await _dbContext.Patients.AddAsync(patient, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            IOperationDataResult<DataLayer.Models.Patient> newPatient = await GetPatientAsync(patient.Id, cancellationToken).ConfigureAwait(false);

            return OperationDataResult<DataLayer.Models.Patient>.Success(newPatient.Data);
        }
        catch (Exception exception)
        {
            // Security warning! Need to consider if FamilyName can be exposed in logs.
            _logger.LogError(exception, "An exception occurred during inserting patient '{Name}' record.", patient.FamilyName);

            if (exception is DbUpdateException || exception is DbUpdateConcurrencyException)
            {
                return OperationDataResult<DataLayer.Models.Patient>.Failure(exception);
            }
            else
            {
                throw;
            }
        }
    }

    /// <inheritdoc/>
    public async Task<IOperationResult> DeletePatientAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            DataLayer.Models.Patient? patient = await _dbContext.Patients.Where(w => w.Id == id).SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (patient == null)
            {
                return OperationResult.Failure($"A patient with identifier '{id}' cannot be found.");
            }

            _dbContext.Patients.Remove(patient);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            if (exception is InvalidOperationException || exception is DbUpdateException || exception is DbUpdateConcurrencyException)
            {
                return OperationResult.Failure(exception);
            }
            else
            {
                throw;
            }
        }

        return OperationResult.Success();
    }

    /// <inheritdoc/>
    public async Task<IOperationDataResult<DataLayer.Models.Patient>> GetPatientAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            DataLayer.Models.Patient? patient =
                await _dbContext
                    .Patients
                    .Where(w => w.Id == id)
                    .Include(i => i.PatientGivenNames)
                    .Include(i => i.Gender)
                    .Include(i => i.Activation)
                    .SingleOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);

            return OperationDataResult<DataLayer.Models.Patient>.Success(patient);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An exception occurred during retrieving patient '{ID}' record.", id);

            if (exception is InvalidOperationException)
            {
                return OperationDataResult<DataLayer.Models.Patient>.Failure(exception);
            }
            else
            {
                throw;
            }
        }
    }

    /// <inheritdoc/>
    public async Task<IOperationDataResult<DataLayer.Models.Patient[]>> GetPatientsAsync(DateRangeFilterModel? dateRangeFilter = null, CancellationToken cancellationToken = default)
    {
        try
        {
            IQueryable<DataLayer.Models.Patient> patientsQuery = _dbContext.Patients.Include(i => i.PatientGivenNames).Include(i => i.Gender).Include(i => i.Activation);

            if (dateRangeFilter != null)
            {
                var filterExpressionBuilder = new PatientDateFilterExpressionBuilder();

                patientsQuery = patientsQuery.Where(filterExpressionBuilder.GetDateFilter(dateRangeFilter));
            }

            return OperationDataResult<DataLayer.Models.Patient[]>.Success(await patientsQuery.ToArrayAsync(cancellationToken).ConfigureAwait(false));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An exception occurred during retrieving patients.");

            if (exception is InvalidOperationException || exception is ArgumentNullException)
            {
                return OperationDataResult<DataLayer.Models.Patient[]>.Failure(exception);
            }
            else
            {
                throw;
            }
        }
    }

    /// <inheritdoc/>
    public async Task<IOperationResult> UpdatePatientAsync(DataLayer.Models.Patient patient, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(patient);

        try
        {
            _dbContext.Patients.Update(patient);

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An exception occurred during updating patient '{ID}' record.", patient.Id);

            if (exception is DbUpdateException || exception is DbUpdateConcurrencyException)
            {
                return OperationDataResult<DataLayer.Models.Patient>.Failure(exception);
            }
            else
            {
                throw;
            }
        }

        return OperationResult.Success();
    }
}