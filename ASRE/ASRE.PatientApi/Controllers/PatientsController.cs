using ASRE.DataLayer.Models;
using ASRE.PatientApi.Core;
using ASRE.PatientApi.Core.DateHandler.Models;
using ASRE.PatientApi.Models;
using ASRE.PatientApi.Services.Activation;
using ASRE.PatientApi.Services.Gender;
using ASRE.PatientApi.Services.Patient;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASRE.PatientApi.Controllers;

[ApiController]
[Route("patients")]
public sealed class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly IGenderService _genderService;
    private readonly IActivationService _activationService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientsController"/> class.
    /// </summary>
    /// <param name="patientService">An instance of a class which implements <see cref="IPatientService"/>.</param>
    /// <param name="genderService">An instance of a class which implements <see cref="IGenderService"/>.</param>
    /// <param name="activationService">An instance of a class which implements <see cref="IActivationService"/>.</param>
    /// <param name="mapper">An instance of a class which implements <see cref="IMapper"/>.</param>
    public PatientsController(IPatientService patientService, IGenderService genderService, IActivationService activationService, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(patientService);
        ArgumentNullException.ThrowIfNull(genderService);
        ArgumentNullException.ThrowIfNull(activationService);
        ArgumentNullException.ThrowIfNull(mapper);

        _patientService = patientService;
        _genderService = genderService;
        _activationService = activationService;
        _mapper = mapper;
    }

    /// <summary>
    /// Deletes a patient.
    /// </summary>
    /// <response code="204">Patient successfully deleted.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="404">Patient has not been found.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        IOperationResult patient = await _patientService.DeletePatientAsync(id, cancellationToken).ConfigureAwait(false);

        if (!patient.IsSuccess)
        {
            return Problem(patient.GetErrors(), statusCode: StatusCodes.Status400BadRequest);
        }

        return NoContent();
    }

    /// <summary>
    /// Gets all patients.
    /// </summary>
    /// <response code="200">Patients successfully returned.</response>
    /// <response code="400">Incorrect filter query.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PatientModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // This is just a simple implementation of date filter for this assessment. It's not generic.
    public async Task<IActionResult> Get([FromQuery] DateRangeFilterModel? birthDate, CancellationToken cancellationToken = default)
    {
        IOperationDataResult<Patient[]> patients = await _patientService.GetPatientsAsync(birthDate, cancellationToken).ConfigureAwait(false);

        if (!patients.IsSuccess)
        {
            return Problem(patients.GetErrors(), statusCode: StatusCodes.Status400BadRequest);
        }

        return Ok(_mapper.Map<PatientModel[]>(patients.Data));
    }

    /// <summary>
    /// Gets a patient by their unique identifier.
    /// </summary>
    /// <response code="200">Patient successfully returned.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="404">Patient has not been found.</response>
    [HttpGet("{id}", Name = "get-patient")]
    [ProducesResponseType(typeof(PatientModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
    {
        IOperationDataResult<Patient> patient = await _patientService.GetPatientAsync(id, cancellationToken).ConfigureAwait(false);

        if (!patient.IsSuccess)
        {
            return Problem(patient.GetErrors(), statusCode: StatusCodes.Status400BadRequest);
        }

        if (patient.Data == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PatientModel>(patient.Data));
    }

    /// <summary>
    /// Creates a new patient.
    /// </summary>
    /// <response code="201">Patient successfully created.</response>
    /// <response code="400">Either payload is incorrectly formatted or patient cannot be created.</response>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PatientModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);

        var patient = _mapper.Map<Patient>(model);

        if (!String.IsNullOrEmpty(model.Gender))
        {
            IOperationDataResult<Gender> gender = await _genderService.GetGenderAsync(model.Gender, cancellationToken).ConfigureAwait(false);

            if (!gender.IsSuccess)
            {
                return Problem(gender.GetErrors(), statusCode: StatusCodes.Status400BadRequest);
            }

            if (gender.IsSuccess && gender.Data == null)
            {
                return Problem("Gender does not exist.", statusCode: StatusCodes.Status400BadRequest);
            }

            patient.GenderId = gender.Data!.Id;
        }

        if (model.Active.HasValue)
        {
            IOperationDataResult<Activation> activation = await _activationService.GetActivationAsync(model.Active.ToString(), cancellationToken).ConfigureAwait(false);

            if (!activation.IsSuccess)
            {
                return Problem(activation.GetErrors(), statusCode: StatusCodes.Status400BadRequest);
            }

            if (activation.IsSuccess && activation.Data == null)
            {
                return Problem("Activation does not exist.", statusCode: StatusCodes.Status400BadRequest);
            }

            patient.ActivationId = activation.Data!.Id;
        }

        IOperationDataResult<Patient> patientResult = await _patientService.AddPatientAsync(patient, cancellationToken).ConfigureAwait(false);

        if (!patientResult.IsSuccess)
        {
            return Problem(patientResult.GetErrors(), statusCode: StatusCodes.Status400BadRequest);
        }

        return Created(Url.Link("get-patient", new { patientResult.Data!.Id })!, _mapper.Map<PatientModel>(patientResult.Data));
    }

    /// <summary>
    /// Partially updates patient.
    /// </summary>
    /// <response code="204">Patient successfully updated.</response>
    /// <response code="400">Either payload is incorrectly formatted or patient cannot be updated.</response>
    /// <response code="404">Patient has not been found.</response>
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] PatientModel model, CancellationToken cancellationToken = default)
    {
        IOperationDataResult<Patient> existingPatient = await _patientService.GetPatientAsync(id, cancellationToken).ConfigureAwait(false);

        if (!existingPatient.IsSuccess)
        {
            return Problem(existingPatient.GetErrors(), statusCode: StatusCodes.Status400BadRequest);
        }

        if (existingPatient.Data == null)
        {
            return NotFound();
        }

        Patient updatedPatient = _mapper.Map(model, existingPatient.Data);

        if (!String.IsNullOrEmpty(model.Gender))
        {
            IOperationDataResult<Gender> gender = await _genderService.GetGenderAsync(model.Gender, cancellationToken).ConfigureAwait(false);

            if (!gender.IsSuccess)
            {
                return Problem(gender.GetErrors(), statusCode: StatusCodes.Status400BadRequest);
            }

            if (gender.IsSuccess && gender.Data == null)
            {
                return Problem("Gender does not exist.", statusCode: StatusCodes.Status400BadRequest);
            }

            updatedPatient.GenderId = gender.Data!.Id;
        }

        if (model.Active.HasValue)
        {
            IOperationDataResult<Activation> activation = await _activationService.GetActivationAsync(model.Active.ToString(), cancellationToken).ConfigureAwait(false);

            if (!activation.IsSuccess)
            {
                return Problem(activation.GetErrors(), statusCode: StatusCodes.Status400BadRequest);
            }

            if (activation.IsSuccess && activation.Data == null)
            {
                return Problem("Activation does not exist.", statusCode: StatusCodes.Status400BadRequest);
            }

            updatedPatient.ActivationId = activation.Data!.Id;
        }

        IOperationResult updateResult = await _patientService.UpdatePatientAsync(updatedPatient, cancellationToken).ConfigureAwait(false);

        if (!updateResult.IsSuccess)
        {
            return Problem(existingPatient.GetErrors(), statusCode: StatusCodes.Status400BadRequest);
        }

        return NoContent();
    }
}