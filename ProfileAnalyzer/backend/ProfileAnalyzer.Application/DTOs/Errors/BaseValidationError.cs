namespace ProfileAnalyzer.Application.DTOs.Errors
{
    public abstract class BaseValidationError(string message)
    {
        public string Message { get; } = message;
    }
}
