namespace ASRE.DataGenerator.Services.DataGenerator.Models;

internal sealed class PatientModel
{
    public PatientModel(string use, string family, IEnumerable<string> given, string gender, DateTime birthDate, bool active)
    {
        Name = new PatientNameModel(use, family, given);
        Gender = gender;
        BirthDate = birthDate;
        Active = active;
    }

    public PatientNameModel Name { get; init; }

    public string Gender { get; init; }

    public DateTime BirthDate { get; init; }

    public bool Active { get; init; }
}