namespace ProfileAnalyzer.Application.DTOs.Errors
{
    public class AtLeastOneRequiredError(string fieldName)
        : BaseValidationError($"At least one '{fieldName}' is required.")
    {
        public string FieldName { get; } = fieldName;
    }
}
