using ASRE.DataGenerator.Services.DataGenerator.Models;
using Bogus;
using Bogus.DataSets;

namespace ASRE.DataGenerator.Services.DataGenerator;

internal sealed class DataGenerator : IDataGenerator
{
    private readonly Faker _faker;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataGenerator"/> class.
    /// </summary>
    /// <param name="faker">An instance of the <see cref="Faker"/> class.</param>
    public DataGenerator(Faker faker)
    {
        ArgumentNullException.ThrowIfNull(faker);

        _faker = faker;
    }

    /// <inheritdoc/>
    public PatientModel GeneratePatient()
    {
        Person person = _faker.Person;

        return new PatientModel(
            _faker.PickRandom("official", "unofficial"),
            person.LastName,
            new[] { person.FirstName, person.UserName },
            person.Gender == Name.Gender.Female ? "female" : "male",
            person.DateOfBirth,
            _faker.PickRandom(true, false));
    }
}