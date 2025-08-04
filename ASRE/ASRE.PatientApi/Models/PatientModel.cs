using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASRE.PatientApi.Models;

public sealed class PatientModel : IValidatableObject
{
    /// <summary>
    /// Gets or sets a patient's name.
    /// </summary>
    public PatientNameModel? Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets a patient's gender.
    /// </summary>
    /// <example>male</example>
    [StringLength(8)]
    public string? Gender { get; set; } = null;

    /// <summary>
    /// Gets or sets a patient's birth date.
    /// </summary>
    /// <example>2024-01-13T18:25:43</example>
    public DateTime? BirthDate { get; set; } = null;

    /// <summary>
    /// Gets or sets whether patient is active.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(null)]
    public bool? Active { get; set; }

    /// <inheritdoc/>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var httpContextAccessor = validationContext.GetRequiredService<IHttpContextAccessor>();

        // Can't use RequiredAttribute on BirthDate because for PATCH request it's not required. One way to overcome it is to separate POST and PATCH models.
        // For eliminating code duplication we validation BirthDate only if the request method is not PATCH. Following KISS principle for this test project.
        if (httpContextAccessor.HttpContext!.Request.Method != HttpMethods.Patch && !BirthDate.HasValue)
        {
            return new[]
            {
                new ValidationResult($"The {nameof(BirthDate)} field is required.", new[] { nameof(BirthDate) })
            };
        }

        return Array.Empty<ValidationResult>();
    }
}