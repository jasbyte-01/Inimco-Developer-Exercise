using ProfileAnalyzer.Application.DTOs.Errors;

namespace ProfileAnalyzer.Application.DTOs
{
    public class ApiResponse<T>(T? data, List<BaseValidationError> errors)
    {
        public T? Data { get; } = data;
        public List<BaseValidationError> Errors { get; } = errors ?? [];
        public bool IsSuccess => Errors.Count == 0;
        public bool IsFailure => !IsSuccess;

        public static ApiResponse<T> Success(T data) => new(data, []);

        public static ApiResponse<T> Error(List<BaseValidationError> errors) =>
            new(default, errors);
    }
}
