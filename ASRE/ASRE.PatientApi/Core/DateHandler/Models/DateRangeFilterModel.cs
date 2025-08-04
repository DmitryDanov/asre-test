using Microsoft.AspNetCore.Mvc;

namespace ASRE.PatientApi.Core.DateHandler.Models;

[ModelBinder(BinderType = typeof(DateFilterBinder))]
public sealed class DateRangeFilterModel
{
    /// <summary>
    /// Gets or sets if the expression defined for a date range.
    /// </summary>
    public bool IsRangeExpression { get; set; }

    /// <summary>
    /// Gets or sets a left date expression. If range is not defined it's used for a regular comparison.
    /// </summary>
    public DateFilterModel? LeftOperand { get; set; } = null;

    /// <summary>
    /// Gets or sets a right date expression.
    /// </summary>
    public DateFilterModel? RightOperand { get; set; } = null;
}