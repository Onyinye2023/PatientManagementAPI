using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PatientManagement.Api.AutoMappings;
using PatientManagement.Application.IRepositories;
using PatientManagement.Application.Services;
using PatientManagement.Dal.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default"))
);

// Add Services
builder.Services.AddAutoMapper(typeof(PatientManagementMappingProfile));

builder.Services.AddScoped<IPatientManagementRepo, PatientManagementService>();
builder.Services.AddScoped<IPatientRecordRepo, PatientRecordService>();

// Add Controllers with JSON Enum Converter
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; 
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Patient Management API",
        Version = "v1",
        Description = "A simple CRUD API for managing patients and their records",
        Contact = new OpenApiContact
        {
            Name = "Onyinye Rosemary",
            Email = "ezeugenyionyinye@gmail.com",
            Url = new Uri("https://github.com/Onyinye2023") 
        }
    });

    c.SchemaFilter<EnumSchemaFilter>();
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient Management API v1");
    options.RoutePrefix = "swagger"; 
});
// Configure Middleware Pipeline
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
