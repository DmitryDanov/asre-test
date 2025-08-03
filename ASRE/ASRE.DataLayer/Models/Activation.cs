using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASRE.DataLayer.Models;

/// <summary>
/// I don't think this entity should exist in the first place, but it's here to satisfy requirements.
/// </summary>
[Table("Activations")]
public sealed class Activation
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(LengthConstants.ActivationLength)]
    public string Name { get; set; } = String.Empty;

    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
}