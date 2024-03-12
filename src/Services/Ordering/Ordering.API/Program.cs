using Microsoft.AspNetCore.Diagnostics;
using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Extensions;
using Ordering.Persistence.Extensions;
using Ordering.Persistence.Repositories.Context;
using Shared.Exceptions;
using Shared.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApplicationServices();
await builder.Services.AddPersistenceServices(builder);
builder.Services.AddInfrastructureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await new ContextSeed(builder.Services.BuildServiceProvider()).SeedDatabaseAsync();
}

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
