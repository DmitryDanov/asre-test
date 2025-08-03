namespace ASRE.PatientApi.Core
{
    // Perhaps it'd be better to create Success and Failure as extension methods. But keeping it simple for this test.
    public class OperationResult : IOperationResult
    {
        protected const string GenericErrorMessage = "An error has occurred.";

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>
        /// <param name="errors">A </param>
        public OperationResult(params string[] errors)
        {
            ArgumentNullException.ThrowIfNull(errors);

            Errors = errors;
        }

        /// <inheritdoc/>
        public ICollection<string> Errors { get; init; }

        /// <inheritdoc/>
        public bool IsSuccess => !Errors.Any();

        /// <inheritdoc/>
        public string GetErrors()
        {
            if (IsSuccess)
            {
                return String.Empty;
            }

            return String.Join("; ", Errors);
        }

        /// <summary>
        /// Creates an instance of <see cref="OperationResult"/> for a successfully completed operation.
        /// </summary>
        /// <returns>The instance of <see cref="OperationResult"/>.</returns>
        public static OperationResult Success()
        {
            return new OperationResult();
        }

        /// <summary>
        /// Creates an instance of <see cref="OperationResult"/> for a failed operation.
        /// </summary>
        /// <param name="errors">A collection of errors.</param>
        /// <returns>The instance of <see cref="OperationResult"/>.</returns>
        public static OperationResult Failure(params string[] errors)
        {
            ArgumentNullException.ThrowIfNull(errors);

            if (!errors.Any())
            {
                return new OperationResult(GenericErrorMessage);
            }

            return new OperationResult(errors);
        }

        /// <summary>
        /// Creates an instance of <see cref="OperationResult"/> for a failed operation.
        /// </summary>
        /// <param name="exception">An instance of the <see cref="Exception"/> class.</param>
        /// <returns>The instance of <see cref="OperationResult"/>.</returns>
        public static OperationResult Failure(Exception exception)
        {
            ArgumentNullException.ThrowIfNull(exception);

            return Failure(exception.Message);
        }
    }
}