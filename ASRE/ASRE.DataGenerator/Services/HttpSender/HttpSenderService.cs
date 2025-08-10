using ASRE.DataGenerator.Services.DataGenerator.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace ASRE.DataGenerator.Services.HttpSender;

internal sealed class HttpSenderService : IHttpSenderService, IDisposable
{
    private readonly IOptions<ApiServiceOptions> _options;
    private readonly ILogger _logger;

    private HttpClient _httpClient;

    private const string PatientsUri = "/patients";

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpSenderService"/> class.
    /// </summary>
    /// <param name="httpClientFactory">An instance of a class which implements <see cref="IHttpClientFactory"/>.</param>
    /// <param name="options">An instance of a class which implements <see cref="IOptions{ApiServiceOptions}"/>.</param>
    /// <param name="logger">An instance of a class which implements <see cref="ILogger{HttpSenderService}"/>.</param>
    public HttpSenderService(IHttpClientFactory httpClientFactory, IOptions<ApiServiceOptions> options, ILogger<HttpSenderService> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        if (options.Value.BaseUri == null)
        {
            throw new OptionsValidationException(nameof(ApiServiceOptions.BaseUri), typeof(Uri), new[] { $"{nameof(ApiServiceOptions.BaseUri)} is required." });
        }

        _options = options;
        _logger = logger;

        _httpClient = httpClientFactory.CreateClient();

        _httpClient.BaseAddress = _options.Value.BaseUri;
        _httpClient.Timeout = _options.Value.Timeout;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _httpClient.Dispose();

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public async Task<bool> PostPatient(PatientModel patient, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(patient);

        try
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync(PatientsUri, patient, cancellationToken).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return true;
            }

            _logger.LogError("A request has failed with status code: {Status}{NewLine}Details:{NewLine}{Response}", httpResponseMessage.StatusCode, Environment.NewLine, Environment.NewLine, httpResponseMessage.Content.ReadAsStringAsync());
        }
        catch (Exception exception)
        {
            if (exception is InvalidOperationException || exception is HttpRequestException)
            {
                _logger.LogError(exception, "An exception has occurred during POST {Uri} request.", PatientsUri);
            }
            else
            {
                throw;
            }
        }

        return false;
    }
}