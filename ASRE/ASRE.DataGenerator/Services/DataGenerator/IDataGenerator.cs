using ASRE.DataGenerator.Services.DataGenerator.Models;

namespace ASRE.DataGenerator.Services.DataGenerator;

internal interface IDataGenerator
{
    /// <summary>
    /// Generates a fake instance of <see cref="PatientModel"/>.
    /// </summary>
    /// <returns>The instance of <see cref="PatientModel"/>.</returns>
    PatientModel GeneratePatient();
}