using Microsoft.AspNetCore.Diagnostics;
using Ordering.Application.Extensions;
using Ordering.Logger.Extensions;
using Shared.Exceptions;
using Shared.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLoggerServices(builder);
builder.Services.AddApplicationServices();

var app = builder.Build();

#region Exception Handling
app.UseExceptionHandler(
    options =>
    {
        options.Run(async context =>
        {
            context.Response.ContentType = "application/json";
            var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionObject != null)
            {
                context.Response.StatusCode = exceptionObject.Error switch
                {
                    BadRequestException ex => StatusCodes.Status400BadRequest,
                    NotFoundException ex => StatusCodes.Status404NotFound,
                    ForbiddenException ex => StatusCodes.Status403Forbidden,
                    HttpRequestException ex => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };
                var errorMessage = $"{exceptionObject.Error.Message}";
                if (context.Response.StatusCode >= 500)
                    errorMessage = "An unexceptected error occurred! Please try again .";

                await context.Response
                    .WriteAsync(JsonSerializer.Serialize(new ErrorDetail
                    {
                        StatusCode = context.Response.StatusCode,
                        ErrorMessage = errorMessage
                    }))
                    .ConfigureAwait(false);
            }
        });
    }
);
#endregion

app.Run();
