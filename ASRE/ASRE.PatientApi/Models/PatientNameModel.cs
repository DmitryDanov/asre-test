using ASRE.DataLayer;
using System.ComponentModel.DataAnnotations;

namespace ASRE.PatientApi.Models;

public sealed class PatientNameModel : IValidatableObject
{
    /// <summary>
    /// Gets or sets an identifier of a patient's record.
    /// </summary>
    /// <example>d8ff176f-bd0a-4b8e-b329-871952e32e1f</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets a patient's Use (what is that?).
    /// </summary>
    /// <example>official</example>
    [StringLength(LengthConstants.UseLength)]
    public string? Use { get; set; } = null;

    /// <summary>
    /// Gets or sets a patient's family name.
    /// </summary>
    /// <example>Ivanov</example>
    [StringLength(LengthConstants.FamilyNameLength)]
    public string Family { get; set; } = null!;

    /// <summary>
    /// Gets or sets a patient's given names.
    /// </summary>
    /// <example>['Ivan','Ivanovich']</example>
    public IEnumerable<string>? Given { get; set; } = null!;

    /// <inheritdoc/>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var httpContextAccessor = validationContext.GetRequiredService<IHttpContextAccessor>();

        var validationResults = new List<ValidationResult>();

        if (Given != null && Given.Any(s => s.Length > LengthConstants.GivenNameLength))
        {
            validationResults.Add(new ValidationResult($"One of the given names exceeds allowed length of {LengthConstants.GivenNameLength} characters.", new[] { nameof(Given) }));
        }

        // Can't use RequiredAttribute on Family because for PATCH request it's not required. One way to overcome it is to separate POST and PATCH models.
        // For eliminating code duplication we validation Family only if the request method is not PATCH. Following KISS principle for this test project.
        if (httpContextAccessor.HttpContext!.Request.Method != HttpMethods.Patch && String.IsNullOrWhiteSpace(Family))
        {
            validationResults.Add(new ValidationResult($"The {nameof(Family)} field is required.", new[] { nameof(Family) }));
        }

        return validationResults;
    }
}