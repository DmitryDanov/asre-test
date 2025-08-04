namespace ASRE.DataGenerator.Services.DataGenerator.Models;

internal sealed class PatientNameModel
{
    public PatientNameModel(string use, string family, IEnumerable<string> given)
    {
        Use = use;
        Family = family;
        Given = given;
    }

    public string? Use { get; set; }

    public string? Family { get; set; }

    public IEnumerable<string>? Given { get; set; }
}