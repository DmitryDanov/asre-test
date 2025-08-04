using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASRE.DataLayer.ValueConverters;

internal sealed class DateTimeWithoutMillisecondsConverter : ValueConverter<DateTime, DateTime>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeWithoutMillisecondsConverter"/> class.
    /// </summary>
    public DateTimeWithoutMillisecondsConverter()
        : base(t => t.AddTicks(-(t.Ticks % TimeSpan.TicksPerSecond)), f => f)
    {
    }
}