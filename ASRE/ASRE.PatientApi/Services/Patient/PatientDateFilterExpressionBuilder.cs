using ASRE.PatientApi.Core.DateHandler.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ASRE.PatientApi.Services.Patient;

// Ideally it should be constructed using expression tree but it's too much work for this assessment.
public sealed class PatientDateFilterExpressionBuilder
{
    public Expression<Func<DataLayer.Models.Patient, bool>> GetDateFilter(DateRangeFilterModel dateRangeFilter)
    {
        if (!dateRangeFilter.IsRangeExpression)
        {
            return ResolveNonRangeExpression(dateRangeFilter.LeftOperand!);
        }

        return ResolveRangeExpression(dateRangeFilter.LeftOperand!, dateRangeFilter.RightOperand!);
    }

    private static Expression<Func<DataLayer.Models.Patient, bool>> ResolveNonRangeExpression(DateFilterModel dateFilter)
    {
        switch (dateFilter.ComparisonOperator)
        {
            case DateComparisonOperator.Eq:
                return patient => dateFilter.TimePortion == TimePortion.FullTimePortion ? patient.BirthDate == dateFilter.Date : (dateFilter.TimePortion == TimePortion.TimePortionWithoutSeconds ? EF.Functions.DateDiffMinute(patient.BirthDate, dateFilter.Date) == 0 : patient.BirthDate.Date == dateFilter.Date);
            case DateComparisonOperator.Ne:
                return patient => dateFilter.TimePortion == TimePortion.FullTimePortion ? patient.BirthDate != dateFilter.Date : (dateFilter.TimePortion == TimePortion.TimePortionWithoutSeconds ? EF.Functions.DateDiffMinute(patient.BirthDate, dateFilter.Date) != 0 : patient.BirthDate.Date != dateFilter.Date);
            case DateComparisonOperator.Gt:
                return patient => dateFilter.TimePortion == TimePortion.FullTimePortion ? patient.BirthDate > dateFilter.Date : (dateFilter.TimePortion == TimePortion.TimePortionWithoutSeconds ? EF.Functions.DateDiffMinute(patient.BirthDate, dateFilter.Date) < 0 : patient.BirthDate.Date > dateFilter.Date);
            case DateComparisonOperator.Lt:
                return patient => dateFilter.TimePortion == TimePortion.FullTimePortion ? patient.BirthDate < dateFilter.Date : (dateFilter.TimePortion == TimePortion.TimePortionWithoutSeconds ? EF.Functions.DateDiffMinute(patient.BirthDate, dateFilter.Date) > 0 : patient.BirthDate.Date < dateFilter.Date);
            case DateComparisonOperator.Ge:
                return patient => dateFilter.TimePortion == TimePortion.FullTimePortion ? patient.BirthDate >= dateFilter.Date : (dateFilter.TimePortion == TimePortion.TimePortionWithoutSeconds ? EF.Functions.DateDiffMinute(patient.BirthDate, dateFilter.Date) <= 0 : patient.BirthDate.Date >= dateFilter.Date);
            case DateComparisonOperator.Le:
                return patient => dateFilter.TimePortion == TimePortion.FullTimePortion ? patient.BirthDate <= dateFilter.Date : (dateFilter.TimePortion == TimePortion.TimePortionWithoutSeconds ? EF.Functions.DateDiffMinute(patient.BirthDate, dateFilter.Date) >= 0 : patient.BirthDate.Date <= dateFilter.Date);
            case DateComparisonOperator.Sa:
            case DateComparisonOperator.Eb:
                return patient => false; // Not applicable for non-range comparison.
            case DateComparisonOperator.Ap:
                long approximation = GetDateApproximation(dateFilter.Date);

                DateTime leftBorder = dateFilter.Date.AddTicks(-approximation);
                DateTime rightBorder = dateFilter.Date.AddTicks(approximation);

                return patient => patient.BirthDate >= leftBorder && patient.BirthDate <= rightBorder;
            default:
                throw new NotImplementedException();
        }
    }

    private static Expression<Func<DataLayer.Models.Patient, bool>> ResolveRangeExpression(DateFilterModel leftOperand, DateFilterModel rightOperand)
    {
        throw new NotImplementedException("Need more time to implement it. Also should be done with the help of expression trees rather than if-else statements or ternary expressions.");
    }

    private static long GetDateApproximation(DateTime dateTime)
    {
        return (long)Math.Round(Math.Abs(dateTime.Ticks - DateTime.UtcNow.Ticks) * 0.1);
    }
}