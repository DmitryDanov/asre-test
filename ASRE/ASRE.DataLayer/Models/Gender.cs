using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASRE.DataLayer.Models;

[Table("Genders")]
public sealed class Gender
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(LengthConstants.GenderLength)]
    public string Name { get; set; } = String.Empty;

    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
}