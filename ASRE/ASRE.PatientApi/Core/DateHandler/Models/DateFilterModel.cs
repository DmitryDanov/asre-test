namespace ASRE.PatientApi.Core.DateHandler.Models;

public sealed class DateFilterModel
{
    /// <summary>
    /// Gets or sets a date that the data in a storage is compared with.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets a comparison operator.
    /// </summary>
    public DateComparisonOperator ComparisonOperator { get; set; }

    /// <summary>
    /// Gets or sets whether the <see cref="Date"/> property should include time portion.
    /// </summary>
    public TimePortion TimePortion { get; set; }
}