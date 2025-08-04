namespace ASRE.DataGenerator.Services.HttpSender;

internal sealed class ApiServiceOptions
{
    public const string Options = "ApiService";

    /// <summary>
    /// Gets or sets a base URI of the API service.
    /// </summary>
    public Uri? BaseUri { get; set; }

    /// <summary>
    /// Gets or sets a timeout to wait before the request times out.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);
}