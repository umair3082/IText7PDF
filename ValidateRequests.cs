using FluentValidation;
using System.Reflection;

namespace IText7PdfPOC
{
    public static class ValidateRequests
    {
        public static async Task<IResult> ValidateRequest<T>(this T request)
        {
            if (request == null)
                return Results.BadRequest("Request is null");

            // Get the type of the request
            var requestType = request.GetType();

            // Find the concrete validator type that implements IValidator<T> for this request type
            var validatorType = Assembly.GetExecutingAssembly()
                                        .GetTypes()
                                        .FirstOrDefault(t => t.GetInterfaces()
                                                              .Any(i => i.IsGenericType &&
                                                                        i.GetGenericTypeDefinition() == typeof(IValidator<>) &&
                                                                        i.GetGenericArguments()[0] == requestType));

            if (validatorType == null)
                return Results.BadRequest($"Validator for {requestType.Name} could not be found.");

            // Create an instance of the validator
            var validatorInstance = Activator.CreateInstance(validatorType) as IValidator;

            if (validatorInstance == null)
                return Results.BadRequest($"Validator for {requestType.Name} could not be instantiated.");

            var result = await validatorInstance.ValidateAsync(new ValidationContext<T>(request));

            if (!result.IsValid)
                return Results.ValidationProblem(result.ToDictionary());

            return null; // Indicates validation passed
        }

    }
}
