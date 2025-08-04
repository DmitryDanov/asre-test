using ASRE.DataGenerator.Services.DataGenerator;
using ASRE.DataGenerator.Services.DataGenerator.Models;
using ASRE.DataGenerator.Services.HttpSender;

namespace ASRE.DataGenerator;

internal sealed class Application
{
    private readonly IDataGenerator _dataGenerator;
    private readonly IHttpSenderService _httpSenderService;

    private const int NumberOfFakeRecords = 100;

    /// <summary>
    /// Initializes a new instance of the <see cref="Application"/> class.
    /// </summary>
    /// <param name="dataGenerator">An instance of a class which implements <see cref="IDataGenerator"/>.</param>
    /// <param name="httpSenderService">An instance of a class which implements <see cref="IHttpSenderService"/>.</param>
    public Application(IDataGenerator dataGenerator, IHttpSenderService httpSenderService)
    {
        ArgumentNullException.ThrowIfNull(dataGenerator);
        ArgumentNullException.ThrowIfNull(httpSenderService);

        _dataGenerator = dataGenerator;
        _httpSenderService = httpSenderService;
    }

    public async Task Run(string?[] args, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("You are about to generate {0} patients. Press enter to continue", NumberOfFakeRecords);
        Console.ReadLine();

        for (int i = 0; i < NumberOfFakeRecords; i++)
        {
            PatientModel patient = _dataGenerator.GeneratePatient();

            if (await _httpSenderService.PostPatient(patient, cancellationToken).ConfigureAwait(false))
            {
                Console.WriteLine("{0}: Patient {1} {2} has been added", i + 1, patient.Name.Given!.FirstOrDefault(), patient.Name.Family);
            }
            else
            {
                Console.WriteLine("{0}: Cannot add a patient. See error in event log viewer", i + 1);
            }
        }

        Console.WriteLine("Operations completed");
    }
}