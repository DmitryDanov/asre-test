using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASRE.DataLayer.Models;

[Table("PatientGivenNames")]
public sealed class PatientGivenName
{
    [Key]
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    [StringLength(LengthConstants.GivenNameLength)]
    public string Name { get; set; } = String.Empty;

    public Patient Patient { get; set; }
}