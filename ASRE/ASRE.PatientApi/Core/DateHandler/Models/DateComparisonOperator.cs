namespace ASRE.PatientApi.Core.DateHandler.Models;

public enum DateComparisonOperator
{
    /// <summary>
    /// Equals.
    /// </summary>
    Eq,

    /// <summary>
    /// Not equal.
    /// </summary>
    Ne,

    /// <summary>
    /// Greater than.
    /// </summary>
    Gt,

    /// <summary>
    /// Less than.
    /// </summary>
    Lt,

    /// <summary>
    /// Greater or equal than.
    /// </summary>
    Ge,

    /// <summary>
    /// Less or equal than.
    /// </summary>
    Le,

    /// <summary>
    /// Starts after.
    /// </summary>
    Sa,

    /// <summary>
    /// Ends before.
    /// </summary>
    Eb,

    /// <summary>
    /// Approximate.
    /// </summary>
    Ap
}