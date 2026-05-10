using app.Application;
using app.Infrastructure;
using app.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.ConfigurePersistenceServices(builder.Configuration);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Description = "Enter JWT Bearer token in the format: Bearer {token}"
            }
        };

        document.Security =
        [
            new OpenApiSecurityRequirement
            {
                {new OpenApiSecuritySchemeReference("Bearer"), new List<string>()}
            }
        ];

        return Task.CompletedTask;
    });
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
