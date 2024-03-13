using Customer.API.Extensions;
using Customer.Business.Extensions;
using Customer.DataAccess.Extensions;
using Customer.DataAccess.Repositories.Context;
using Microsoft.AspNetCore.Diagnostics;
using Shared.Exceptions;
using Shared.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddBusinessServices();
builder.Services.AddDataAccessServices(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowedOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Mock database
    var seedDatabase = new ContextSeed(builder.Services.BuildServiceProvider());
    await seedDatabase.SeedDatabaseAsync();
}

app.UseHttpsRedirection();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
