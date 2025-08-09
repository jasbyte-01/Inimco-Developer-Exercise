namespace ProfileAnalyzer.Application.DTOs.Errors
{
    public abstract class BaseValidationError(string message)
    {
        public String Message { get; } = message;
    }
}
