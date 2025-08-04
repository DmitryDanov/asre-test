using ASRE.PatientApi.Core.DateHandler.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ASRE.PatientApi.Core.DateHandler;

public sealed class DateFilterBinder : IModelBinder
{
    private const string DateFilterExpression = "^(?<operator>eq|ne|gt|lt|ge|le|sa|eb|ap)(?<date>\\d{4}-\\d{2}-\\d{2}(?:T\\d{2}:\\d{2}(?::\\d{2})?)?)$";
    private const string DateFormat = "yyyy-MM-dd";
    private const string DateTimeNoSecondsFormat = "yyyy-MM-ddTHH:mm";
    private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

    /// <inheritdoc/>
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        string modelName = bindingContext.ModelName;

        ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        string? value = valueProviderResult.FirstValue;

        if (String.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        if (valueProviderResult.Values.Count > 2)
        {
            bindingContext.ModelState.TryAddModelError(modelName, "Too many arguments.");

            return Task.CompletedTask;
        }

        DateFilterModel? leftOperand = null;
        DateFilterModel? rightOperand = null;

        if (!TryResolveDateFilter(value, out leftOperand, out string? error))
        {
            bindingContext.ModelState.TryAddModelError(modelName, error!);

            return Task.CompletedTask;
        }

        if (valueProviderResult.Values.Count > 1 && !TryResolveDateFilter(valueProviderResult.Values.Last()!, out rightOperand, out error))
        {
            bindingContext.ModelState.TryAddModelError(modelName, error!);

            return Task.CompletedTask;
        }

        bindingContext.Result = ModelBindingResult.Success(new DateRangeFilterModel
        {
            IsRangeExpression = rightOperand != null,
            LeftOperand = leftOperand,
            RightOperand = rightOperand
        });

        return Task.CompletedTask;
    }

    private static bool TryResolveDateFilter(string parameter, out DateFilterModel? dateFilter, out string? error)
    {
        Match operandMatch = Regex.Match(parameter, DateFilterExpression, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));

        dateFilter = null;
        error = null;

        if (!operandMatch.Success)
        {
            error = $"Value '{parameter}' is not in a valid format.";

            return false;
        }

        if (!Enum.TryParse(operandMatch.Groups["operator"].Value, true, out DateComparisonOperator @operator))
        {
            error = $"Operand '{operandMatch.Groups["operator"].Value}' is not valid. Allowed operators are 'eq', 'ne', 'gt', 'lt', 'ge', 'sa', 'eb' and 'ap.";

            return false;
        }

        if (!TryParseDate(operandMatch.Groups["date"].Value, out DateTime date, out TimePortion timePortion))
        {
            error = $"Date '{operandMatch.Groups["date"].Value}' is not correctly formatter.";

            return false;
        }

        dateFilter = new DateFilterModel
        {
            ComparisonOperator = @operator,
            Date = date,
            TimePortion = timePortion
        };

        return true;
    }

    private static bool TryParseDate(string? value, out DateTime date, out TimePortion timePortion)
    {
        CultureInfo cultureInfo = CultureInfo.InvariantCulture;
        DateTimeStyles dateTimeStyles = DateTimeStyles.None;

        timePortion = TimePortion.NoTimePortion;

        if (DateTime.TryParseExact(value, DateFormat, cultureInfo, dateTimeStyles, out date))
        {
            return true;
        }

        if (DateTime.TryParseExact(value, DateTimeNoSecondsFormat, cultureInfo, dateTimeStyles, out date))
        {
            timePortion = TimePortion.TimePortionWithoutSeconds;

            return true;
        }

        if (DateTime.TryParseExact(value, DateTimeFormat, cultureInfo, dateTimeStyles, out date))
        {
            timePortion = TimePortion.FullTimePortion;

            return true;
        }

        return false;
    }
}