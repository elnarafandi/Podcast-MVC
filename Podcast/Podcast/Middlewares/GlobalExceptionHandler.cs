using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Podcast.Middlewares
{
    public class GlobalExceptionHandler:IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Define a ProblemDetails object to hold our response
            var problemDetails = new ProblemDetails
            {
                Title = "An error occurred",
                Detail = exception.Message,
                Instance = httpContext.Request.Path // Adding the request path to the instance field for better debugging
            };

            // Handle specific exceptions
            switch (exception)
            {
                case ArgumentNullException argNullEx:
                    problemDetails.Title = "Bad Request";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = $"A required argument was null: {argNullEx.ParamName}";
                    break;

                case InvalidOperationException invalidOpEx:
                    problemDetails.Title = "Invalid Operation";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = $"An invalid operation occurred: {invalidOpEx.Message}";
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    problemDetails.Title = "Unauthorized";
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    problemDetails.Detail = unauthorizedEx.Message;
                    break;

                case KeyNotFoundException keyNotFoundEx:
                    problemDetails.Title = "Not Found";
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Detail = keyNotFoundEx.Message;
                    break;

                default:
                    // For unhandled exceptions, we return a generic internal server error
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Internal Server Error";
                    break;
            }

            // Set the response status code based on the exception type
            httpContext.Response.StatusCode = problemDetails.Status.Value;

            // Write the problem details to the response body as JSON
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
