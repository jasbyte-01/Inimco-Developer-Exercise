namespace ProfileAnalyzer.Application.DTOs.Errors
{
    public class RequiredFieldError(string fieldName)
        : BaseValidationError($"The field '{fieldName}' is required.")
    {
        public string FieldName { get; } = fieldName;
    }
}
