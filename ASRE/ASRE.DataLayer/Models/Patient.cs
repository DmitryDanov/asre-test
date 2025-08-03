using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASRE.DataLayer.Models;

[Table("Patients")]
public sealed class Patient
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(LengthConstants.FamilyNameLength)]
    public string FamilyName { get; set; } = String.Empty;

    [StringLength(LengthConstants.UseLength)]
    public string? Use { get; set; }

    public Guid? GenderId { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime BirthDate { get; set; }

    public Guid? ActivationId { get; set; }

    public ICollection<PatientGivenName> PatientGivenNames { get; set; } = new List<PatientGivenName>();

    public Gender? Gender { get; set; }

    public Activation? Activation { get; set; }
}