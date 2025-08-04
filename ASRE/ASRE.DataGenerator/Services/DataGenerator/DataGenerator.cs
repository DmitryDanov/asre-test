using ASRE.DataGenerator.Services.DataGenerator.Models;
using Bogus;
using Bogus.DataSets;

namespace ASRE.DataGenerator.Services.DataGenerator;

internal sealed class DataGenerator : IDataGenerator
{
    /// <inheritdoc/>
    public PatientModel GeneratePatient()
    {
        Faker faker = new();
        Person person = faker.Person;

        return new PatientModel(
            faker.PickRandom("official", "unofficial"),
            person.LastName,
            new[] { person.FirstName, person.UserName },
            person.Gender == Name.Gender.Female ? "female" : "male",
            person.DateOfBirth,
            faker.PickRandom(true, false));
    }
}